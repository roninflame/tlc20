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
    public class SpaceCraft7 : SpaceShip
    {
        [Space]
        [Header("SpaceCraft 7")]
        [SerializeField] protected Transform _shootingPlaceTra2;
        [SerializeField] protected Transform _shootingPlaceTra3;

        [Header("Weapon")]
        //[SerializeField] private float _timeToShoot = 1;
        [SerializeField] private int _ammoAmount;
        [SerializeField] private float _fireRate = 0.5f;

        private float _distance;
        private Sequence _sequence;
        private Tween _tween;
        public override SpaceCraftType SpaceCraftType => SpaceCraftType.SpaceCraft7;

        private void Start()
        {
            Activate();
            //_holderGO.transform.localRotation = Quaternion.Euler(new Vector3(20, 0, 0));
            //_tween = _holderGO.transform.DOLocalRotate(new Vector3(-20, 0, 0), 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

        }

        protected void Update()
        {
            if (_canTakeDamage && _target.TargetEnable)
            {
                _target.UpdateTarget(transform.position, gameObject);
            }
        }
        public void Init(EnemySpaceCraftData data, Transform parent)
        {

            transform.parent = parent;
           
            _health = data.health;
            _scoreValue = data.scoreValue;
            _damage = data.damage;

            _ammoAmount = data.ammoAmount;
            _fireRate = data.fireRate;

            _windSpeed = data.windSpeed;
            _windStrength = data.windStrength;

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
            _tween?.Kill();
            _tween = null;
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
            _modelGO.transform.localRotation = Quaternion.Euler(new Vector3(40, 0, 0));
            _tween = _modelGO.transform.DOLocalRotate(new Vector3(-40, 0, 0), 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            _sequence = pattern.Movement(transform, Shoot,()=> Deactivate(), null);
        }
        public override void Shoot()
        {
            if(gameObject.activeInHierarchy)
                StartCoroutine(IenFire());
        }

        protected override void ReturnToPool()
        {
            ObjectPoolSpaceCraft7.Instance.ReturnToPool(this);
        }

        IEnumerator IenFire()
        {
            Projectile weapon;
            for (int i = 0; i < _ammoAmount; i++)
            {

                weapon = ObjectPoolManager.Instance.ActivateProjectile(ProjectileType.EnergyBullet2, TargetType.Player);
                _shootingPlaceTra.LookAt(PlayerManager.Instance.player.hitPosition);
                weapon.SetTransform(null, _shootingPlaceTra.position, _shootingPlaceTra.rotation.eulerAngles);
                weapon.SetParent(transform.parent);
                weapon.transform.LookAt(_shootingPlaceTra.position);
                weapon.SetKinematic(true);
                weapon.Activate();
                weapon.Shoot();

                weapon = ObjectPoolManager.Instance.ActivateProjectile(ProjectileType.EnergyBullet2, TargetType.Player);
                _shootingPlaceTra2.LookAt(PlayerManager.Instance.player.hitPosition);
                weapon.SetTransform(null, _shootingPlaceTra2.position, _shootingPlaceTra2.rotation.eulerAngles);
                weapon.SetParent(transform.parent);
                weapon.transform.LookAt(PlayerManager.Instance.player.hitPosition - PlayerManager.Instance.player.hitTransform.right * 10);
                weapon.SetKinematic(true);
                weapon.Activate();
                weapon.Shoot();

                weapon = ObjectPoolManager.Instance.ActivateProjectile(ProjectileType.EnergyBullet2, TargetType.Player);
                _shootingPlaceTra3.LookAt(PlayerManager.Instance.player.hitPosition);
                weapon.SetTransform(null, _shootingPlaceTra3.position, _shootingPlaceTra3.rotation.eulerAngles);
                weapon.SetParent(transform.parent);
                weapon.transform.LookAt(PlayerManager.Instance.player.hitPosition + PlayerManager.Instance.player.hitTransform.right * 10);
                weapon.SetKinematic(true);
                weapon.Activate();
                weapon.Shoot();

                //weapon = ObjectPoolManager.Instance.ActivateProjectile(ProjectileType.EnergyBullet, TargetType.Player);
                //_shootingPlaceTra2.LookAt(PlayerManager.Instance.player.hitPosition);
                //weapon.SetTransform(null, _shootingPlaceTra2.position, _shootingPlaceTra2.rotation.eulerAngles);
                //weapon.SetParent(transform.parent);
                //weapon.SetKinematic(true);
                //weapon.Activate();
                //weapon.Shoot();

                yield return new WaitForSeconds(_fireRate);
            }
        }

        protected override void HitSound()
        {
            FMODUnity.RuntimeManager.PlayOneShot(_hitSound, transform.position);
        }


    }

}
