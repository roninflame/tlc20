using PolloScripts.Data;
using PolloScripts.Enums;
using PolloScripts.ObjectPoolSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.WeaponSystem
{
    public class BossEnergyBall : Projectile
    {
        private void Update()
        {
            if (_canMove)
            {
                if (_rb.isKinematic)
                {
                    transform.position += transform.forward * _speed * Time.deltaTime;

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
        public override void Shoot()
        {
            _canMove = true;
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
            ObjectPoolBossEnergyBall.Instance.ReturnToPool(this);
        }

        protected override void PlayWeaponSound()
        {

        }
    }

}
