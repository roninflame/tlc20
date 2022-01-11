using PolloScripts.Data;
using PolloScripts.Enums;
using PolloScripts.ObjectPoolSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.WeaponSystem
{
    public class BulletMovement : Projectile
    {
        private void Update()
        {
            if (_canMove)
            {
                if (_rb.isKinematic)
                {
                    transform.position += transform.forward * _speed * Time.deltaTime;
                }

                if (timeRemaining < _lifeTime)
                {
                    timeRemaining += Time.deltaTime;
                }
                else
                {
                    Deactivate();
                }
            }
        }
      
        public void Init(ProjectileData data, TargetType targetType)
        {
            _targetType = targetType;
            _speed = data.speed;
            _damage = data.damage;
        }
        public override void Shoot()
        {
            base.Shoot();
            _canMove = true;
        }
        protected void OnTriggerEnter(Collider other)
        {
            if (DoDamage(other.gameObject))
            {
                Deactivate();
            }
        }
        public override void Deactivate()
        {
            StopAllCoroutines();
            _canMove = false;
            timeRemaining = 0;
            if (_rb != null)
            {
                _rb.velocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
            }
            _targetFollow = null;
            Destroy(gameObject);
        }
        protected override void ReturnToPool()
        {
            //ObjectPoolBullet.Instance.ReturnToPool(this);
        }

        protected override void PlayWeaponSound()
        {
            FMODUnity.RuntimeManager.PlayOneShot(_weaponFMOD, transform.position);
        }
    }

}

