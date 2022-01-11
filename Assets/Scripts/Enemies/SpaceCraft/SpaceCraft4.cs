using PolloScripts.Data;
using PolloScripts.Effects;
using PolloScripts.Enums;
using PolloScripts.Interfaces;
using PolloScripts.ObjectPoolSystem;
using PolloScripts.WeaponSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Enemies
{
    public class SpaceCraft4 : SpaceShip, IVisible
    {
        [Header("Weapon")]
        //[SerializeField] private float _timeToShoot = 1;
        [SerializeField] private int _ammoAmount;
        [SerializeField] private float _fireRate = 0.5f;

        //Local
        private bool _showModel;
        private float _distance;
        private bool _isShooting;
        public override SpaceCraftType SpaceCraftType => SpaceCraftType.SpaceCraft4;

        private void Start()
        {
            SetTargets();
            Activate();
            ShowModel(true);
        }

        protected void Update()
        {
            if (_canTakeDamage && _target.TargetEnable)
            {
                _target.UpdateTarget(transform.position, gameObject);
            }
            _holderGO.transform.LookAt(PlayerManager.Instance.player.hitPosition);
            if (_showModel && !_isShooting)
            {
                
                _distance = Vector3.Distance(transform.position, PlayerManager.Instance.player.hitPosition);
                if (_distance < _distanceToShoot && _distance > 20)
                {
                    Shoot();
                }
            }

        }
        public void Init(EnemySpaceCraftData data, Transform parent, Vector3 position, Quaternion rotation)
        {
            if(parent == null)
            {
                transform.position = position;
                transform.rotation = rotation;
            }
            else
            {
                transform.parent = parent;
                transform.localPosition = position;
                transform.localRotation = rotation;
            }


            _health = data.health;
            _scoreValue = data.scoreValue;
            _damage = data.damage;
            //weapon
            _ammoAmount = data.ammoAmount;
            //_timeToShoot = data.timeToShoot;
            _fireRate = data.fireRate;

            _windSpeed = data.windSpeed;
            _windStrength = data.windStrength;

            SetTargets();
        }
        public override void Activate()
        {
            base.Activate();
            ShowModel(false);
        }
        public override void Deactivate()
        {
            base.Deactivate();
            _showModel = false;
            _isShooting = false;
            ReturnToPool();
        }
        public override void Death()
        {
            base.Death();
            Deactivate();
            //Explosion
            ExplosionBase explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Energy, null, transform.position, Quaternion.identity);
            explosion.ReSize(12);
            explosion.Activate();
        }
        public override void ShowModel(bool show)
        {
            base.ShowModel(show);
            if (show)
            {
                _showModel = true;
    
            }
            _canTakeDamage = show;
        }
        public override void Move(IPatternMove pattern)
        {
            //pattern.Move(transform, Shoot, Deactivate);

        }
        public override void Shoot()
        {
            _isShooting = true;
            if (gameObject.activeInHierarchy)
                StartCoroutine(IenFire());
        }

        protected override void ReturnToPool()
        {
            ObjectPoolSpaceCraft4.Instance.ReturnToPool(this);
        }
      
        IEnumerator IenFire()
        {
            Projectile weapon;
            for (int i = 0; i < _ammoAmount; i++)
            {
      
                weapon = ObjectPoolManager.Instance.ActivateProjectile(ProjectileType.EnergyBall, TargetType.Player);
                _shootingPlaceTra.LookAt(UtilityClasses.PredictProjectilePosition(_shootingPlaceTra, weapon.Speed, 1));
                weapon.SetTransform(null, _shootingPlaceTra.position, _shootingPlaceTra.rotation.eulerAngles);
                weapon.SetKinematic(false);
                weapon.Activate();
                weapon.Shoot();
                yield return new WaitForSeconds(_fireRate);

                ////para dispara arma dirigida al player 
                //if (i == 0 || i + 1 == _ammoAmount)
                //{

                //    weapon.FollowPlayer(PlayerManager.Instance.player.transform);
                //}
                //else
                //{
                //    _shootingPlaceTra.LookAt(PlayerManager.Instance.player.hitPosition + GetTargetPositions());
                //}
            }
        }

        private List<Vector3> _targetPositionList;
        private void SetTargets()
        {
            _targetPositionList = new List<Vector3>();
            _targetPositionList.Add(new Vector3(-6, 4, 0));
            _targetPositionList.Add(new Vector3(-3, 4, 0));
            _targetPositionList.Add(new Vector3(0, 4, 0));
            _targetPositionList.Add(new Vector3(3, 4, 0));
            _targetPositionList.Add(new Vector3(6, 4, 0));
            _targetPositionList.Add(new Vector3(-6, -4, 0));
            _targetPositionList.Add(new Vector3(-3, -4, 0));
            _targetPositionList.Add(new Vector3(0, -4, 0));
            _targetPositionList.Add(new Vector3(6, -4, 0));
            _targetPositionList.Add(new Vector3(3, -4, 0));

        }

        private Vector3 GetTargetPositions()
        {
            return _targetPositionList[Random.Range(0, _targetPositionList.Count)];
        }

        protected override void HitSound()
        {
            FMODUnity.RuntimeManager.PlayOneShot(_hitSound, transform.position);
        }

        public override void Movement(IPatternMovement pattern)
        {
            throw new System.NotImplementedException();
        }
    }
}

