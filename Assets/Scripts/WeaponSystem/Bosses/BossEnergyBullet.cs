using PolloScripts.Data;
using PolloScripts.Enums;
using PolloScripts.ObjectPoolSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.WeaponSystem
{
    public class BossEnergyBullet : Projectile
    {
        private void Update()
        {
            if (_canMove)
            {
                //transform.position += transform.forward * _speed * Time.deltaTime;

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
            _rb.AddForce(transform.forward * _speed, ForceMode.Impulse);
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
            ObjectPoolBossEnergyBullet.Instance.ReturnToPool(this);
        }

        protected override void PlayWeaponSound()
        {

        }
    }

}
