using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.ObjectPoolSystem
{

    [Serializable]
    public abstract class ObjectPool<T> : MonoBehaviour where T : Component
    {

        public static ObjectPool<T> Instance { get; private set; }
        [Header("Pool Setting")]
        public T _prefab = null;
        [SerializeField] private int _startingPoolSize = 10;

        protected Queue<T> _objectPool = new Queue<T>();

        protected List<T> _objectUsed = new List<T>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
               
            CheckReferences();
            CreateInitialPool(_startingPoolSize);
        }
        private void OnDestroy()
        {
            DestroyObjectPooled();
        }
        public T ActivateFromPool()
        {
            if (_objectPool.Count == 0)
            {
                CreateNewPoolObject();
            }

            T newPoolObject = _objectPool.Dequeue();
            _objectUsed.Add(newPoolObject);
            return newPoolObject;
        }

        public void ReturnToPool(T objectToReturn)
        {
            ResetObjectDefaults(objectToReturn);
            objectToReturn.gameObject.SetActive(false);
            _objectUsed.Remove(objectToReturn);
            _objectPool.Enqueue(objectToReturn);
        }
        public List<T> ReturnActiveObject()
        {
            return new List<T> (_objectUsed);
        }
        private void DestroyObjectPooled()
        {
            foreach (var item in _objectPool)
            {
                Destroy(item.gameObject);
            }
            _objectPool.Clear();
            Instance = null;
        }

        protected virtual void ResetObjectDefaults(T poolObject)
        {
            poolObject.transform.parent = transform;
        }

        private void CheckReferences()
        {
            if (_prefab == null)
            {
                print(this + "no pool prefab defined");
                this.enabled = false;
                return;
            }
        }

        private void CreateInitialPool(int startingPollSize)
        {
            for (int i = 0; i < startingPollSize; i++)
            {
                CreateNewPoolObject();
            }

        }
        private void CreateNewPoolObject()
        {
            T newObject = Instantiate(_prefab);
            newObject.transform.SetParent(this.transform);
            newObject.gameObject.name = _prefab.gameObject.name;
            newObject.gameObject.SetActive(false);
            _objectPool.Enqueue(newObject);
        }
    }

}
