using PolloScripts.Enums;
using PolloScripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Enemies
{
    public abstract class Cherub : EnemyBase
    {
        [Space]
        [Header("Cherub")]
        [SerializeField] protected Transform _shootingPlace1Tra;
        [SerializeField] protected Transform _shootingPlace2Tra;
        [SerializeField] protected bool _turretMode;
        [SerializeField] protected float _distanceToShoot = 250f;
        //public abstract SpaceCraftType SpaceCraftType { get; }

        public bool TurretMode => _turretMode;
        public Action<SpaceShip> OnDeactivate;

        protected float _distance;

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
            if (!_isTakingDamage && _canTakeDamage)
            {
                int sum = _health - value;
                if (sum > 0)
                {
                    _health = sum;
                    StartCoroutine(IenDamageCoolDown());

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
        //public abstract void Move(IPatternMove pattern);
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


