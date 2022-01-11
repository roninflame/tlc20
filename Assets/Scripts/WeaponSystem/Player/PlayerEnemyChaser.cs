
using PolloScripts.Data;
using PolloScripts.Enums;
using PolloScripts.ObjectPoolSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PolloScripts.WeaponSystem
{
 
    public class PlayerEnemyChaser : Projectile
    {

        /* Variables */
        public float smooth = 5; // Kind like turn speed

        void SmoothLookAt(Vector3 target)
        {

            Vector3 dir = target - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smooth);
        }
        private void Update()
        {
            if (_canMove)
            {
                if (timeRemaining < _lifeTime)
                {
                    timeRemaining += Time.deltaTime;
                }
                else
                {
                    Deactivate();
                }
            }

            if (_rb.isKinematic){
                if(_targetFollow != null)
                {
                    if (_targetFollow.gameObject.activeInHierarchy)
                    {
                        // Rotate in arc
                        SmoothLookAt(_targetFollow.position);
                    }
                }
              

                // Move
                transform.position = transform.position + transform.forward * _speed * Time.deltaTime;
            }
        }

        private void FixedUpdate()
        {
            if (!_rb.isKinematic && _canMove)
            {
                if (_targetFollow != null)
                {
                    if (_targetFollow.gameObject.activeInHierarchy)
                    {
                        // Rotate in arc
                        SmoothLookAt(_targetFollow.position);
                    }
                }

                // Move
                _rb.velocity = transform.forward * _speed;
            }
        }

        public void Init(ProjectileData data, TargetType targetType)
        {
            _targetType = targetType;
            _speed = data.speed;
            _damage = data.damage;
            _lifeTime = 5;
            //_fireRate = data.fireRate;
        }
        public override void Activate()
        {
            base.Activate();
        }
        public override void Deactivate()
        {
            base.Deactivate();
            _targetFollow = null;
        }
        public override void Shoot()
        {
            _canMove = true;
            transform.rotation = Quaternion.LookRotation((_targetFollow.position + _targetFollow.up * 100) - transform.position);
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
            ObjectPoolPlayerEnemyChaser.Instance.ReturnToPool(this);
        }

        protected override void PlayWeaponSound()
        {

        }
    }
}
