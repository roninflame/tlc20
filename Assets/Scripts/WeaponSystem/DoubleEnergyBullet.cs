
using PolloScripts.Data;
using PolloScripts.Enums;
using PolloScripts.ObjectPoolSystem;
using System.Collections;
using UnityEngine;

namespace PolloScripts.WeaponSystem
{
    public class DoubleEnergyBullet : Projectile
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
        public void Init(ProjectileData data, TargetType targetType)
        {
            _targetType = targetType;
            _speed = data.speed;
            _damage = data.damage;
            //_fireRate = data.fireRate;

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
            ObjectPoolDoubleBullet.Instance.ReturnToPool(this);
        }

        protected override void PlayWeaponSound()
        {

        }
    }
}
