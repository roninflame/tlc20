using DG.Tweening;
using PolloScripts.Data;
using PolloScripts.Enums;
using PolloScripts.Interfaces;
using PolloScripts.ObjectPoolSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.WeaponSystem
{
    public class BossCrossLaser : Projectile
    {
        [SerializeField] private List<LaserSmoke> _laserSmokeList;
        private Sequence _sequence;
        public void Init(ProjectileData data, TargetType targetType, Transform parent, Vector3 position, Quaternion rotation)
        {
            _targetType = targetType;
            _speed = data.speed;
            _damage = data.damage;
            //_fireRate = data.fireRate;

            if (parent != null)
            {
                transform.parent = parent;
                transform.rotation = rotation;
                transform.position = position;
            }
            else
            {
                transform.rotation = rotation;
                transform.position = position;
            }
        }
        public override void Shoot()
        {
            _canMove = true;
        }
        public void Move(ITrajectoryPattern pattern, Vector3 destiny, Vector3 rotation)
        {
            _sequence = pattern.Move(transform, destiny, rotation, ()=>Deactivate());
        }

        public override void Deactivate()
        {
            base.Deactivate();
            _sequence?.Kill();
            _sequence = null;
        }
        private void DeactivateByTween()
        {
            Deactivate();
        }
        //protected void OnTriggerEnter(Collider other)
        //{
        //    if (DoDamage(other.gameObject))
        //    {
        //        Deactivate();
        //    }
        //}
        protected override void ReturnToPool()
        {
            ObjectPoolBossCrossLaser.Instance.ReturnToPool(this);
        }

        protected override void PlayWeaponSound()
        {

        }


    }

}
