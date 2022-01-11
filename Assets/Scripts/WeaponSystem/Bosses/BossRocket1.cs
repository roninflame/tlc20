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
    public class BossRocket1 : Projectile
    {
        [Space]
        [Header("***** Rocket *****")]
        public float degree;
        private float _timeToFollow;
        private float _timeToDie;
        private bool _die;
        private void Update()
        {
            if(!_canMove)
            return;

             if (Vector3.Distance(transform.position, PlayerManager.Instance.player.hitPosition) > 20 && !_die)
            {
                if (_timeToFollow < 0.5f)
                {
                    _timeToFollow += Time.deltaTime;
                }
                else
                {
                    Quaternion rotPlayer = Quaternion.LookRotation(PlayerManager.Instance.player.hitPosition - transform.position);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotPlayer, degree * Time.deltaTime);
                }
                transform.position = transform.position + transform.forward * Time.deltaTime * _speed/2;
            }
            else
            {
                _die = true;
                transform.position = transform.position + transform.forward * Time.deltaTime * _speed;
                if(_timeToDie < 2)
                {
                    _timeToDie += Time.deltaTime;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
        public void Init(ProjectileData data, TargetType targetType, Transform parent, Vector3 position, Quaternion rotation)
        {
            _targetType = targetType;
            _speed = data.speed;
            _damage = data.damage;
            _timeToFollow = 0;
            _timeToDie = 0;
            _die = false;
            _canMove = false;
            //_fireRate = data.fireRate;

            if (parent != null)
            {
                transform.parent = parent;
                transform.localRotation = rotation;
                transform.localPosition = position;

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
        public void Move(IRocketMovement pattern, Vector3 destiny, Vector3 rotation)
        {
            ///_sequence = pattern.Move(transform, destiny, rotation, LineRendererON, LineRendererOFF, DeactivateByTween);
        }
        public override void Deactivate()
        {
            base.Deactivate();
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
            ObjectPoolBossRocket1.Instance.ReturnToPool(this);
        }

        protected override void PlayWeaponSound()
        {
            
        }
    }

}

