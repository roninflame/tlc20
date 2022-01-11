using PolloScripts.Enums;
using PolloScripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Enemies
{
    public abstract class SpaceShip : EnemyBase
    {
        [Space]
        [Header("SpaceCraft")]
        [SerializeField] protected Transform _shootingPlaceTra;
        [SerializeField] protected bool _turretMode;
        [SerializeField] protected float _distanceToShoot = 250f;
        [SerializeField] protected GameObject _hitEffect;

        public abstract SpaceCraftType SpaceCraftType { get; }
        //public Action<SpaceShip> OnDeactivate;

        
        public override void Activate()
        {
            base.Activate();
            _isDead = false;
        }
        public override void Deactivate()
        {
            base.Deactivate();
            ResetMaterialHit();
            ReturnToPool();
        }
        public override void DamageReceived(int value)
        {
            if (!_isTakingDamage && _canTakeDamage && !_isDead)
            {
                int sum = _health - value;
                if (sum > 0)
                {
                    _health = sum;
                    StartCoroutine(IenDamageCoolDown());
                    HitSound();
                    GameObject go = Instantiate(_hitEffect, transform.position, Quaternion.identity);
                    go.transform.SetParent(transform);
                    Destroy(go, 1f);
                    if (_colorMaterialHitCor == null)
                    {
                        _colorMaterialHitCor = StartCoroutine(IenColorMaterialHit());
                    }
                    else
                    {
                        StopCoroutine(_colorMaterialHitCor);
                        _colorMaterialHitCor = StartCoroutine(IenColorMaterialHit());
                    }
                }
                else
                {
                    AddScore();
                    Death();
                }
            }
        }
        public abstract void Move(IPatternMove pattern);
        public abstract void Movement(IPatternMovement pattern);
        public abstract void Shoot();

        protected abstract void ReturnToPool();

        protected void OnTriggerEnter(Collider other)
        {
            IPlayer player = other.GetComponentInParent<IPlayer>();
            if (player != null)
            {
                player.DamageReceived(_damage);
                Death();
            }
        }
    }

}
