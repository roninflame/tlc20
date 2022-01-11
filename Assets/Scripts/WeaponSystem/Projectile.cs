using PolloScripts.Enums;
using PolloScripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PolloScripts.WeaponSystem
{
    public abstract class Projectile : WeaponBase
    {
        public override WeaponType WeaponType => WeaponType.Projectile;
        [Space]
        [Header("Projectile")]
        //[SerializeField] protected float _fireRate;
        [SerializeField] protected float _speed;
        [SerializeField] protected float _lifeTime = 3f;
        public float timeRemaining = 10;
        protected Transform _targetFollow;

        [Space]
        [Header("FMOD")]
        [FMODUnity.EventRef]
        [SerializeField] protected string _weaponFMOD;

        public float Speed => _speed;
        public bool CanMove => _canMove;
        protected abstract void PlayWeaponSound();
        //public abstract ProjectileType ProjectileType { get; }
        //public float FireRate => _fireRate;

        protected Rigidbody _rb;
        protected bool _canMove;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }
        public override void Activate()
        {
            gameObject.SetActive(true);
            timeRemaining = 0;
        }
        public override void Deactivate()
        {
            StopAllCoroutines();
            _canMove = false;
            timeRemaining = 0;
            if(_rb != null)
            {
                _rb.velocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
            }
            _targetFollow = null;
            ReturnToPool();
        }
        public void FollowPlayer(Transform target)
        {
            _targetFollow = target;
        }

        public override void Shoot()
        {
            PlayWeaponSound();
        }
        public void SetProjectile(TargetType targetType, Transform parent, Vector3 position, Vector3 rotation, int damage, float speed, float lifeTime)
        {
            SetTarget(targetType);
            if(parent != null)
            {
                SetParent(parent);
                SetLocalPosition(position);
                SetLocalRotation(rotation);
            }
            else
            {
                SetPosition(position);
                SetRotation(rotation);
            }
            SetDamage(damage);
            SetSpeed(speed);
            SetLifeTime(lifeTime);
        }
        public void SetDamage(int damage)
        {
            _damage = damage;
        }
        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
        public void SetRotation(Vector3 rotation)
        {
            transform.rotation = Quaternion.Euler(rotation);
        }
        public void SetLocalPosition(Vector3 position)
        {
            transform.localPosition = position;
        }
        public void SetLocalRotation(Vector3 rotation)
        {
            transform.localRotation = Quaternion.Euler(rotation);
        }
        public void SetParent(Transform parent)
        {
            transform.parent = parent;
        }
        public void SetSpeed(float speed)
        {
            _speed = speed;
        }
        public void SetLifeTime(float lifeTime)
        {
            _lifeTime = lifeTime;
        }
        public void SetTarget(TargetType targetType)
        {
            _targetType = targetType;
        }

        public void SetTransform(Transform parent, Vector3 position, Vector3 rotation)
        {
            if(parent != null)
            {
                transform.SetParent(parent);
                transform.localPosition = position;
                transform.localRotation = Quaternion.Euler(rotation);
            }
            else
            {
                transform.position = position;
                transform.rotation = Quaternion.Euler(rotation);
            }
        }
        public void SetKinematic(bool value)
        {
            _rb.isKinematic = value;
        }
        
        protected virtual IEnumerator IenLifeTime()
        {
            yield return new WaitForSeconds(_lifeTime);
            Deactivate();
        }

        protected abstract void ReturnToPool();

    }
}

