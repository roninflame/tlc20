using PolloScripts.Interfaces;
using PolloScripts.TargetSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PolloScripts.Enemies.Bosses
{
    public class BossHealth : EnemyBase, IEnemy
    {
        public UnityEvent OnPointDestroyed;
        public UnityEvent OnDeath;
        public UnityEvent OnLevelUp;

        //public Action OnDestroy;
        //public bool destroyed;
        public int[] HealthPoints;

        private int _currentHealthPointIndex = 0;

        protected void Update()
        {
            if (_isDead)
                return;

            if (_canTakeDamage && _target.TargetEnable)
            {
                _target.UpdateTarget(transform.position, gameObject);
            }
        }
        public void SetTakeDamage(bool value)
        {
            _canTakeDamage = value;
        }
        public override void Activate()
        {
            base.Activate();
            _canTakeDamage = true;

            _health = HealthPoints[_currentHealthPointIndex];
            //_currentHealthPointIndex++;
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
                    Death();
                }
            }
        }
        public void ReStart()
        {
            _canTakeDamage = true;
            _target.ActivateTarget();
        }
        public void Stop()
        {
            ResetMaterialHit();
            _canTakeDamage = false;
            _target.DeactivateTarget();
        }
        public void LevelUP()
        {
            ResetMaterialHit();
            _canTakeDamage = false;

            _health = HealthPoints[_currentHealthPointIndex];
            _currentHealthPointIndex++;

            OnPointDestroyed?.Invoke();
            //OnDestroy.Invoke();
        }
        public override void Deactivate()
        {
            base.Deactivate();
            ResetMaterialHit();
            _canTakeDamage = false;
            //OnDestroy.Invoke();

        }

        public override void Death()
        {
            if(HealthPoints.Length == 1)
            {
                base.Death();
                OnDeath?.Invoke();
            }
            else 
            {
                if (_currentHealthPointIndex == 0)
                {
                    _currentHealthPointIndex++;
                    _health = HealthPoints[_currentHealthPointIndex];
                    OnLevelUp?.Invoke();
                }
                else
                {
                    base.Death();
                    OnDeath?.Invoke();
                }
                   
            }
           
        }

        protected override void HitSound()
        {
            //FMODUnity.RuntimeManager.PlayOneShot(_hitSound, transform.position);
        }

    }
}

