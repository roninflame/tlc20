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
    public class SpaceCraft5 : SpaceShip,IVisible
    {
        [SerializeField] private float _rotateToPlayer = 0.5f;
        [SerializeField] private float _waitToFollowPlayer = 0.5f;
        [SerializeField] private float _durationToFollowPlayer = 1.5f;

        private bool _shooting;
        private bool _isShootingRotation;
        private bool _autoRotation;
        private Vector3 _nextPlayerPos;
        Laser weapon;

        //Local
        private float _distance;
        //private bool _isShooting;
        public override SpaceCraftType SpaceCraftType => SpaceCraftType.SpaceCraft3;
        protected void Update()
        {
            if (_canTakeDamage && _target.TargetEnable)
            {
                _target.UpdateTarget(transform.position, gameObject);
            }
            //_holderGO.transform.LookAt(PlayerManager.Instance.player.hitPosition);
            _holderGO.transform.rotation = Quaternion.Lerp(_holderGO.transform.rotation, Quaternion.LookRotation(PlayerManager.Instance.player.hitPosition - _shootingPlaceTra.position), Time.deltaTime * 1.5f);

            if (!_shooting)
            {
                _distance = Vector3.Distance(transform.position, PlayerManager.Instance.player.hitPosition);
                if (_distance < _distanceToShoot && _distance > 20)
                {
                    Shoot();
                }
            }
            else if (_shooting)
            {
                if(_isShootingRotation && !_autoRotation)
                    _shootingPlaceTra.rotation = Quaternion.Lerp(_shootingPlaceTra.rotation, Quaternion.LookRotation(PlayerManager.Instance.player.hitPosition - _shootingPlaceTra.position), _rotateToPlayer * Time.deltaTime);
                else if (_isShootingRotation && _autoRotation)
                    _shootingPlaceTra.rotation = Quaternion.Lerp(_shootingPlaceTra.rotation, Quaternion.LookRotation(_nextPlayerPos - _shootingPlaceTra.position), _rotateToPlayer/2 * Time.deltaTime);
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
            _durationToFollowPlayer = data.durationToFollowPlayer;
            _waitToFollowPlayer = data.waitToFollowPlayer;
            _autoRotation = false;

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
            _isShootingRotation = false;
            ShowModel(false);
        }
        public override void Deactivate()
        {
            if (weapon != null)
            {
                weapon.Deactivate();
                weapon = null;
            }

            base.Deactivate();
            _shooting = false;
            _isShootingRotation = false;
            _autoRotation = false;
        }
        public override void ShowModel(bool show)
        {
            base.ShowModel(show);
            _holderGO.transform.LookAt(PlayerManager.Instance.player.hitPosition);
            _canTakeDamage = show;
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
        public override void Move(IPatternMove pattern)
        {
            
        }

        public override void Shoot()
        {
            _shooting = true;
            if (gameObject.activeInHierarchy)
                StartCoroutine(IenFire());
        }

        protected override void ReturnToPool()
        {
            ObjectPoolSpaceCraft5.Instance.ReturnToPool(this);
        }
      
        IEnumerator IenFire()
        {

            weapon = ObjectPoolManager.Instance.ActivateLaser(LaserType.LaserSmoke, TargetType.Player, transform.parent, _shootingPlaceTra.position, _shootingPlaceTra.rotation);
            weapon.SetLaser(TargetType.Player, _shootingPlaceTra, Vector3.zero, Vector3.zero);
            weapon.Activate();

            int dir = Random.Range(0, 2);
            Vector3 rotation = PlayerManager.Instance.player.hitPosition + new Vector3(0, dir == 0? 10:-10,0);
            _shootingPlaceTra.LookAt(rotation);
            weapon.Shoot();

            yield return new WaitForSeconds(_waitToFollowPlayer);
            _isShootingRotation = true;

            yield return new WaitForSeconds(_durationToFollowPlayer);

            _autoRotation = true;
            _nextPlayerPos = PlayerManager.Instance.player.objectTraker.GetProjectedPosition(1);
            yield return new WaitForSeconds(1f);

            weapon?.Deactivate();
            weapon = null;
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

