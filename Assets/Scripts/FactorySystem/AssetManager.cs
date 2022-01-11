using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolloScripts.Enums;
using PolloScripts.Data;
using System.Linq;

namespace PolloScripts.FactorySystem
{
    [Serializable]
    public class AssetSpawnData
    {
        public AssetType Asset;
        public int Type;
        public GameObject Prefab;
        public int PooledAmount;
    }
    [Serializable]
    public class ItemSpawnData
    {
        public ItemType Asset;
        public int Type;
        public GameObject Prefab;
        public int PooledAmount;
    }

        [Serializable]
    public class EnemySpawnData
    {
        public EnemyType Asset;
        public int Type;
        public GameObject Prefab;
        public int PooledAmount;
    }
    [Serializable]
    public class WeaponSpawnData
    {
        public WeaponType Asset;
        public int Type;
        public GameObject Prefab;
        public int PooledAmount;
    }

    public class AssetManager : MonoBehaviour
    {
        private Dictionary<AssetType, Dictionary<int, Queue<Transform>>> assetPooled = new Dictionary<AssetType, Dictionary<int, Queue<Transform>>>();
        
        private Dictionary<ItemType, Dictionary<int, Queue<Transform>>> itemPooled = new Dictionary<ItemType, Dictionary<int, Queue<Transform>>>();

        private Dictionary<EnemyType, Dictionary<int, Queue<Transform>>> enemyPooled = new Dictionary<EnemyType, Dictionary<int, Queue<Transform>>>();

        private Dictionary<WeaponType, Dictionary<int, Queue<Transform>>> weaponPooled = new Dictionary<WeaponType, Dictionary<int, Queue<Transform>>>();

        [SerializeField] private Transform _assetHolder;
        [Space]
        [Header("***** Asset *****")]
     
        [SerializeField] private List<AssetSpawnData> _assetList;

        [Space]
        [Header("***** Item *****")]
        [SerializeField] private List<ItemSpawnData> _itemList;

        [Space]
        [Header("***** Enemy *****")]
        [SerializeField] private List<EnemySpawnData> _enemyList;

        [Space]
        [Header("***** Weapon *****")]
        [SerializeField] private List<WeaponSpawnData> _weaponList;


        public void CreateAssets()
        {
            Queue<Transform> objects;
            //Dictionary<int, Queue<Transform>> dObjects = new Dictionary<int, Queue<Transform>>();

            //pooledAssets.Add(E_Assets.Asteroid, new Dictionary<int, Queue<Transform>>());
            foreach (AssetSpawnData item in _assetList)
            {
                if (!assetPooled.ContainsKey(item.Asset))
                    assetPooled.Add(item.Asset, new Dictionary<int, Queue<Transform>>());

                objects = new Queue<Transform>();

                for (int i = 0; i < item.PooledAmount; i++)
                {
                    GameObject go = Instantiate(item.Prefab, Vector3.zero, Quaternion.identity) as GameObject;
                    go.transform.parent = _assetHolder;
                    go.name = go.name + "_" + i;
                    go.SetActive(false);
                    objects.Enqueue(go.transform);
                }
                assetPooled[item.Asset].Add(item.Type, objects);
            }
        }

        public void CreateItem()
        {
            Queue<Transform> objects;
            //Dictionary<int, Queue<Transform>> dObjects = new Dictionary<int, Queue<Transform>>();

            //pooledAssets.Add(E_Assets.Asteroid, new Dictionary<int, Queue<Transform>>());
            foreach (ItemSpawnData item in _itemList)
            {
                if (!itemPooled.ContainsKey(item.Asset))
                    itemPooled.Add(item.Asset, new Dictionary<int, Queue<Transform>>());

                objects = new Queue<Transform>();

                for (int i = 0; i < item.PooledAmount; i++)
                {
                    GameObject go = Instantiate(item.Prefab, Vector3.zero, Quaternion.identity) as GameObject;
                    go.transform.parent = _assetHolder;
                    go.name = go.name + "_" + i;
                    go.SetActive(false);
                    objects.Enqueue(go.transform);
                }
                itemPooled[item.Asset].Add(item.Type, objects);
            }
        }

        public void CreateEnemies()
        {
            Queue<Transform> objects;
            //Dictionary<int, Queue<Transform>> dObjects = new Dictionary<int, Queue<Transform>>();

            //pooledAssets.Add(E_Assets.Asteroid, new Dictionary<int, Queue<Transform>>());
            foreach (EnemySpawnData item in _enemyList)
            {
                if (!enemyPooled.ContainsKey(item.Asset))
                    enemyPooled.Add(item.Asset, new Dictionary<int, Queue<Transform>>());

                objects = new Queue<Transform>();

                for (int i = 0; i < item.PooledAmount; i++)
                {
                    GameObject go = Instantiate(item.Prefab, Vector3.zero, Quaternion.identity) as GameObject;
                    go.transform.parent = _assetHolder;
                    go.name = go.name + "_" + i;
                    go.SetActive(false);
                    objects.Enqueue(go.transform);
                }
                enemyPooled[item.Asset].Add(item.Type, objects);
            }
        }
        public void CreateWeapons()
        {
            Queue<Transform> objects;

            foreach (WeaponSpawnData item in _weaponList)
            {
                if (!weaponPooled.ContainsKey(item.Asset))
                    weaponPooled.Add(item.Asset, new Dictionary<int, Queue<Transform>>());

                objects = new Queue<Transform>();

                for (int i = 0; i < item.PooledAmount; i++)
                {
                    GameObject go = Instantiate(item.Prefab, Vector3.zero, Quaternion.identity) as GameObject;
                    go.transform.parent = _assetHolder;
                    go.name = go.name + "_" + i;
                    go.SetActive(false);
                    objects.Enqueue(go.transform);
                }
                weaponPooled[item.Asset].Add(item.Type, objects);
            }
        }

        public Transform GetAsset(AssetType asset, int typeID, Transform parent, Vector3 newPosition, Quaternion newRotation)
        {
            Transform tra = null;
            //var result =(T)(object)null;

            if (!assetPooled[asset][typeID].Peek().gameObject.activeInHierarchy)
            {
                tra = assetPooled[asset][typeID].Dequeue();
            }
            else
            {
                GameObject go = Instantiate(GetPrefab(asset, typeID), Vector3.zero, Quaternion.identity) as GameObject;
                go.name = go.name + "_" + assetPooled[asset][typeID].Count;
                tra = go.transform;
            }
            //assetPooled[asset][typeID].Enqueue(tra);

            if (parent != null)
            {
                tra.parent = parent;
            }
            switch (asset)
            {
                case AssetType.Effect:
                    tra.position = newPosition;
                    tra.rotation = newRotation;
                    break;
                default:
                    break;
            }
            return tra;
            //return (T)(object)result;
        }

        public Transform GetItem(ItemType asset, int typeID, Transform parent, Vector3 newPosition, Quaternion newRotation)
        {
            Transform tra = null;
            //var result =(T)(object)null;
            if (itemPooled[asset][typeID].Count == 0)
            {
                GameObject go = Instantiate(GetPrefab(asset, typeID), Vector3.zero, Quaternion.identity) as GameObject;
                go.name = go.name + "_" + itemPooled[asset][typeID].Count;
                tra = go.transform;
                itemPooled[asset][typeID].Enqueue(tra);

            }


            if (!itemPooled[asset][typeID].Peek().gameObject.activeInHierarchy)
            {
               
                    tra = itemPooled[asset][typeID].Dequeue();
            }
            else
            {
                GameObject go = Instantiate(GetPrefab(asset, typeID), Vector3.zero, Quaternion.identity) as GameObject;
                go.name = go.name + "_" + itemPooled[asset][typeID].Count;
                tra = go.transform;
            }
            //itemPooled[asset][typeID].Enqueue(tra);

            if (parent != null)
            {
                tra.parent = parent;
                tra.localPosition = newPosition;
                tra.localRotation = newRotation;
            }
            else
            {
                tra.position = newPosition;
                tra.rotation = newRotation;
            }
            return tra;
            //return (T)(object)result;
        }

        public Transform GetEnemy(EnemyType asset, int typeID, Transform parent, Vector3 newPosition, Quaternion newRotation)
        {
            Transform tra = null;
            //var result =(T)(object)null;

            if (!enemyPooled[asset][typeID].Peek().gameObject.activeInHierarchy)
            {
                tra = enemyPooled[asset][typeID].Dequeue();
            }
            else
            {
                GameObject go = Instantiate(GetPrefab(asset, typeID), Vector3.zero, Quaternion.identity) as GameObject;
                go.name = go.name + "_" + enemyPooled[asset][typeID].Count;
                tra = go.transform;
            }
            //enemyPooled[asset][typeID].Enqueue(tra);

            if (parent != null)
            {
                tra.parent = parent;
            }

            switch (asset)
            {
                case EnemyType.None:
                    break;
                case EnemyType.Asteroid:
                    tra.localPosition = newPosition;
                    tra.localRotation = newRotation;
                    break;
                case EnemyType.SpaceCraft:
                    tra.localPosition = new Vector3(0, 50, -10);
                    break;
                default:
                    break;
            }
            return tra;
            //return (T)(object)result;
        }

        public Transform GetWeapon(WeaponType asset, int typeID, Transform parent, Vector3 newPosition, Quaternion newRotation)
        {
            Transform tra = null;
            //var result =(T)(object)null;

            if (!weaponPooled[asset][typeID].Peek().gameObject.activeInHierarchy)
            {
                tra = weaponPooled[asset][typeID].Dequeue();
            }
            else
            {
                GameObject go = Instantiate(GetPrefab(asset, typeID), Vector3.zero, Quaternion.identity) as GameObject;
                go.name = go.name + "_" + weaponPooled[asset][typeID].Count;
                tra = go.transform;
            }
            //weaponPooled[asset][typeID].Enqueue(tra);

            if (parent != null)
            {
                tra.parent = parent;
                tra.localPosition = newPosition;
                tra.localRotation = newRotation;
            }
            else
            {
                switch (asset)
                {
                    case WeaponType.None:
                        break;
                    case WeaponType.Projectile:
                        tra.position = newPosition;
                        tra.rotation = newRotation;
                        break;
                    case WeaponType.Laser:
                        tra.position = newPosition;
                        tra.rotation = newRotation;
                        break;
                   
                    default:
                        break;
                }
            }
           
            return tra;
            //return (T)(object)result;
        }


        public void ReturnToPool(AssetType asset, int assetType, int typeID,  Transform objectToReturn)
        {
            objectToReturn.gameObject.SetActive(false);

            switch (asset)
            {
                case AssetType.None:
                    break;
                case AssetType.Item:
                    itemPooled[(ItemType)assetType][typeID].Enqueue(objectToReturn);
                    break;
                case AssetType.Weapon:
                    weaponPooled[(WeaponType)assetType][typeID].Enqueue(objectToReturn);
                    break;
                case AssetType.Enemy:
                    enemyPooled[(EnemyType)assetType][typeID].Enqueue(objectToReturn);
                    break;
                case AssetType.Effect:
                    assetPooled[(AssetType)assetType][typeID].Enqueue(objectToReturn);
                    break;
                default:
                    break;
            }
            objectToReturn.parent = _assetHolder;
        }

        private GameObject GetPrefab(ItemType asset, int type)
        {
            return _itemList.Single(x => x.Asset == asset && x.Type == type).Prefab;
        }

        private GameObject GetPrefab(AssetType asset, int type)
        {
            return _assetList.Single(x => x.Asset == asset && x.Type == type).Prefab;
        }
        private GameObject GetPrefab(EnemyType asset, int type)
        {
            return _enemyList.Single(x => x.Asset == asset && x.Type == type).Prefab;
        }
        private GameObject GetPrefab(WeaponType asset, int type)
        {
            return _weaponList.Single(x => x.Asset == asset && x.Type == type).Prefab;
        }
    }
}

