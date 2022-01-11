using DG.Tweening;
using PolloScripts.Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PolloScripts
{
    [Serializable]
    public class ColliderEvent : UnityEvent<Collider> { }
    public class RadarCollider : MonoBehaviour
    {
        public Transform parentTra;
        public bool contact;
        public Collider radarCol;
        public ColliderEvent OnRadarCollider;

        private void Update()
        {
            transform.position = parentTra.position;
        }
        public void EnableCollider(bool value)
        {
            radarCol.enabled = value;
        }
        public void CollisionTween()
        {
            transform.localScale = Vector3.one * 2;
            transform.DOScale(Vector3.one * 10, 1);
        }

        public void StopTween()
        {
            DOTween.Kill(transform);
        }
        private void OnTriggerEnter(Collider other)
        {

            if (!contact && other.GetComponentInParent<EnemyBase>())
            {
                contact = true;
                OnRadarCollider?.Invoke(other);
           
            }
               
        }
    }
}

