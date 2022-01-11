using DG.Tweening;
using PolloScripts.Data;
using PolloScripts.Enums;
using PolloScripts.FactorySystem;
using PolloScripts.Interfaces;
using PolloScripts.ObjectPoolSystem;
using PolloScripts.WeaponSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Enemies
{
    public class SpaceCraft2 : SpaceShip
    {
        //[Header("SpaceCraft 2")]
        //[SerializeField] protected Transform _shootingPlaceTra2;

        [Header("Weapon")]
        //[SerializeField] private float _timeToShoot = 1;
        [SerializeField] private int _ammoAmount;
        [SerializeField] private float _fireRate = 0.5f;

        private float _distance;
        private Sequence _sequence;
        private bool _canLook;
        public override SpaceCraftType SpaceCraftType => SpaceCraftType.SpaceCraft2;

        protected void Update()
        {
            if (_canTakeDamage && _target.TargetEnable)
            {
                _target.UpdateTarget(transform.position, gameObject);
            }
            if (_canLook)
            {
                _holderGO.transform.rotation = Quaternion.RotateTowards(_holderGO.transform.rotation, Quaternion.LookRotation(PlayerManager.Instance.player.hitPosition - _holderGO.transform.position), 250 * Time.deltaTime);
                //_holderGO.transform.LookAt(PlayerManager.Instance.player.hitPosition);
            }
                
        }
        public void Init(EnemySpaceCraftData data, Transform parent)
        {

            transform.parent = parent;
           
            _health = data.health;
            _scoreValue = data.scoreValue;
            _damage = data.damage;

            _ammoAmount = data.ammoAmount2;
            _fireRate = data.fireRate2;

            _windSpeed = data.windSpeed;
            _windStrength = data.windStrength;
            _canLook = false;
        }
        public void SetAttackPattern(AttackPattern data)
        {
            _ammoAmount = data.ammo;
            _fireRate = data.fireRate;
        }
        public override void Activate()
        {
            base.Activate();
            ShowModel(true);
            _canTakeDamage = true;
        }
        public override void Deactivate()
        {
            _sequence?.Kill();
            _sequence = null;
            _canLook = false;
            base.Deactivate();
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
        public override void Move(IPatternMove pattern)
        {
            _sequence = pattern.Move(transform,Shoot, DeactivateByTween, null);
        }
        public override void Movement(IPatternMovement pattern)
        {
            _sequence = pattern.Movement(transform, Shoot,()=> Deactivate(), LookPlayer);
        }
        private void LookPlayer()
        {
            _canLook = true;
        }
        public override void Shoot()
        {
            if(gameObject.activeInHierarchy)
                StartCoroutine(IenFire());
        }

        protected override void ReturnToPool()
        {
            ObjectPoolSpaceCraft2.Instance.ReturnToPool(this);
        }

        IEnumerator IenFire()
        {
            Projectile weapon;
            for (int i = 0; i < _ammoAmount; i++)
            {

                weapon = ObjectPoolManager.Instance.ActivateProjectile(ProjectileType.DEnergyBullet, TargetType.Player);
                _shootingPlaceTra.LookAt(PlayerManager.Instance.player.hitPosition);
                weapon.SetTransform(null, _shootingPlaceTra.position, _shootingPlaceTra.rotation.eulerAngles);
                weapon.SetParent(transform.parent);
                weapon.SetKinematic(true);
                weapon.Activate();
                weapon.Shoot();

                yield return new WaitForSeconds(_fireRate);
            }
        }

        protected override void HitSound()
        {
            FMODUnity.RuntimeManager.PlayOneShot(_hitSound, transform.position);
        }


    }

}
