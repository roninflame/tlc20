using DG.Tweening;
using PolloScripts.Data;
using PolloScripts.Enums;
using PolloScripts.Interfaces;
using PolloScripts.ObjectPoolSystem;
using PolloScripts.WeaponSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Enemies
{
    public class SpaceCraft3 : SpaceShip
    {
        [SerializeField] private float _rotateToPlayer = 0.5f;
        private bool _shooting;
        Laser weapon;
        public override SpaceCraftType SpaceCraftType => SpaceCraftType.SpaceCraft3;
        private Sequence _sequence;
        protected void Update()
        {
            if (_canTakeDamage && _target.TargetEnable)
            {
                _target.UpdateTarget(transform.position, gameObject);
            }

            if (_shooting)
            {
                _shootingPlaceTra.rotation = Quaternion.RotateTowards(_shootingPlaceTra.rotation, Quaternion.LookRotation(PlayerManager.Instance.player.hitPosition - _shootingPlaceTra.position), _rotateToPlayer * Time.deltaTime);
            }
        }

        public void Init(EnemySpaceCraftData data, Transform parent, Vector3 position, Quaternion rotation)
        {
            _health = data.health;
            _scoreValue = data.scoreValue;
            _damage = data.damage;
            _rotateToPlayer = data.rotateToPlayer;
            _windSpeed = data.windSpeed;
            _windStrength = data.windStrength;
            if (parent != null)
            {
                transform.parent = parent;
                transform.localPosition = position;
                transform.localRotation = rotation;
            }
            else
            {
                transform.position = position;
                transform.rotation = rotation;
            }
           
        }
        public override void Activate()
        {
            base.Activate();
            _canTakeDamage = true;
            ShowModel(true);
            _shooting = false;
        }
        public override void Deactivate()
        {
            base.Deactivate();
            weapon?.Deactivate();
            _sequence?.Kill();
            _sequence = null;
            weapon = null;
            //OnDeactivate?.Invoke(this);
        }
        public override void Death()
        {
            base.Death();
          
            Deactivate();
            //Explosion
            ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, transform.position, Quaternion.identity).Activate();
        }
        private void DeactivateByTween()
        {
            Deactivate();
        }
        protected override void ReturnToPool()
        {
            ObjectPoolSpaceCraft3.Instance.ReturnToPool(this);
        }
        public override void Move(IPatternMove pattern)
        {
            _sequence = pattern.Move(transform, Shoot, DeactivateByTween, null);
        }

        public override void Shoot()
        {
            if (gameObject.activeInHierarchy)
                StartCoroutine(IenFire());
        }
       
        IEnumerator IenFire()
        {
            _shooting = true;
 
            weapon = ObjectPoolManager.Instance.ActivateLaser( LaserType.LaserBeam, TargetType.Player, transform.parent, _shootingPlaceTra.position, _shootingPlaceTra.rotation);
            weapon.SetLaser(TargetType.Player, _shootingPlaceTra, Vector3.zero, Vector3.zero);
            //weapon.OnContact += HitPlayer;
            weapon.Activate();
            yield return new WaitForSeconds(1);
            Vector3 protation = PlayerManager.Instance.player.hitPosition;
            yield return new WaitForSeconds(0.5f);
            _shootingPlaceTra.LookAt(protation);
            weapon.Shoot();
            yield return new WaitForSeconds(5);
            _shooting = false;
            weapon?.Deactivate();
        }

        protected override void HitSound()
        {
            //FMODUnity.RuntimeManager.PlayOneShot(_hitSound, transform.position);
        }

        public override void Movement(IPatternMovement pattern)
        {
            throw new System.NotImplementedException();
        }
    }

}
