using PolloScripts.Data;
using PolloScripts.Enums;
using PolloScripts.ObjectPoolSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.WeaponSystem
{
    public class EnergyBullet2 : Projectile
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
        private void FixedUpdate()
        {
            if (_canMove && !_rb.isKinematic)
            {
                _rb.velocity = transform.forward * _speed;

                if (_targetFollow != null)
                {
                    float distanceTarget = Vector3.Distance(_targetFollow.position, transform.position);
                    if (distanceTarget > 10)
                    {
                        transform.LookAt(_targetFollow);
                    }
                    else
                    {
                        _targetFollow = null;
                    }

                }
            }
        }
        public void Init(ProjectileData data, TargetType targetType)
        {
            _targetType = targetType;
            _speed = data.speed;
            _damage = data.damage;
            _lifeTime = data.lifeTime;
        }
        public override void Shoot()
        {
            base.Shoot();
            _canMove = true;
            //if (!_rb.isKinematic)
            //{
            //    _rb.velocity = transform.forward * _speed;
            //}
            //_rb.AddForce(transform.forward * _speed, ForceMode.Impulse);
        }
        protected void OnTriggerEnter(Collider other)
        {
            if (DoDamage(other.gameObject))
            {
                Deactivate();
            }
        }
        protected override void ReturnToPool()
        {
            ObjectPoolBullet2.Instance.ReturnToPool(this);
        }

        protected override void PlayWeaponSound()
        {
            FMODUnity.RuntimeManager.PlayOneShot(_weaponFMOD, transform.position);
        }
    }

}
