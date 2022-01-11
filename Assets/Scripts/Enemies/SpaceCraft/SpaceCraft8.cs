using PolloScripts.Data;
using PolloScripts.Effects;
using PolloScripts.Enums;
using PolloScripts.Interfaces;
using PolloScripts.ObjectPoolSystem;
using PolloScripts.WeaponSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace PolloScripts.Enemies
{
    public class SpaceCraft8 : SpaceShip, IVisible
    {
        [Header("Animator")]
        public Animator animator;

        [Space]
        [Header("Weapon")]
        //[SerializeField] private float _timeToShoot = 1;
        [SerializeField] private int _ammoAmount;
        [SerializeField] private float _fireRate = 0.5f;
        [SerializeField] private EnemyAttackShield _shield;

        [SerializeField] private float _shieldScale1 = 12f;
        [SerializeField] private float _shieldScale2 = 30f;
        [SerializeField] private float _timeToScale1 = 0.5f;
        [SerializeField] private float _timeToScale2 = 1;
        //Local
        private bool _showModel;
        private float _distance;
        private bool _isShooting;
        private int _fireHash = Animator.StringToHash("Fire");
        public override SpaceCraftType SpaceCraftType => SpaceCraftType.SpaceCraft8;

        //private void Start()
        //{
        //    _shield.Hide();
        //    _isShooting = true;
        //    StartCoroutine(IenFire());
        //}

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

            animator.SetBool(_fireHash, false);
            _shield.Hide();
            _shield.transform.localPosition = Vector3.zero;
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
            StopAllCoroutines();
            _shield.Hide();
            _shield.transform.SetParent(transform);
            _shield.transform.localPosition = Vector3.zero;
            DOTween.Kill(_shield.transform);
            animator.SetBool(_fireHash, false);

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
            ObjectPoolSpaceCraft8.Instance.ReturnToPool(this);
        }
      
        IEnumerator IenFire()
        {
            while (_isShooting)
            {
                _shield.transform.SetParent(null);
                _shield.transform.position = transform.position;
                _shield.transform.localScale = Vector3.one * _shieldScale1;

                animator.SetBool(_fireHash, true);

                yield return new WaitForSeconds(0.2f);

                Tween t1 = _shield.transform.DOScale(Vector3.one * _shieldScale2, _timeToScale1);
                _shield.Show();
                yield return t1.WaitForCompletion();

                yield return new WaitForSeconds(_timeToScale2);

                animator.SetBool(_fireHash, false);

                t1 = _shield.transform.DOScale(Vector3.one * _shieldScale1, _timeToScale1 / 2f);

                yield return t1.WaitForCompletion();
                _shield.Hide();

                yield return new WaitForSeconds(_fireRate);
            }
            yield return null;
        }

        protected override void HitSound()
        {
            FMODUnity.RuntimeManager.PlayOneShot(_hitSound, transform.position);
        }

        public override void Movement(IPatternMovement pattern)
        {

        }
    }
}

