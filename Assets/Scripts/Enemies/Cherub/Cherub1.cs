using DG.Tweening;
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
    public class Cherub1 : Cherub
    {
        [Header("Weapon")]
        //[SerializeField] private float _timeToShoot = 1;
        [SerializeField] private GameObject _ringPrefab;
       
        [SerializeField] private int _ammoAmount = 6;
        [SerializeField] private float _speed = 200f;
        [SerializeField] private float _fireRate = 0.5f;

        private Sequence _sequence;
        //para el orden de movimiento

        protected void Update()
        {
            if (_canTakeDamage && _target.TargetEnable)
            {
                _target.UpdateTarget(transform.position, gameObject);
            }

            _holderGO.transform.LookAt(PlayerManager.Instance.player.hitPosition);
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

            base.Deactivate();
        }
        public override void Death()
        {
            base.Death();
            Deactivate();
            //Explosion
            ExplosionBase explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, transform.position, Quaternion.identity);
            explosion.ReSize(12);
            explosion.Activate();
        }

        public override void Movement(IPatternMovement pattern)
        {
            _sequence = pattern.Movement(transform, Shoot, () => Deactivate(), null);
        }
        public override void Shoot()
        {
            if (gameObject.activeInHierarchy)
                StartCoroutine(IenFire());
        }

        protected override void ReturnToPool()
        {
            ObjectPoolCherub1.Instance.ReturnToPool(this);
        }

        IEnumerator IenFire()
        {
          
            for (int i = 0; i < _ammoAmount; i++)
            {
                
                GameObject weapon;
                _shootingPlace1Tra.LookAt(PlayerManager.Instance.player.hitPosition + (PlayerManager.Instance.player.transform.right * 2f));
                weapon = Instantiate(_ringPrefab, _shootingPlace1Tra.position, _shootingPlace1Tra.rotation);
                weapon.transform.SetParent(transform.parent);
                
                weapon.GetComponent<BulletMovement>().Shoot();
                ///weapon.transform.parent = transform.parent;
                //weapon.GetComponent<Rigidbody>().AddForce(weapon.transform.forward * _speed);
                //Destroy(weapon, 3f);
                yield return new WaitForSeconds(_fireRate);


                GameObject weapon2;
                _shootingPlace2Tra.LookAt(PlayerManager.Instance.player.hitPosition + (-PlayerManager.Instance.player.transform.right * 2f));
                weapon2 = Instantiate(_ringPrefab, _shootingPlace2Tra.position, _shootingPlace2Tra.rotation);
                weapon2.transform.SetParent(transform.parent);
                weapon2.GetComponent<BulletMovement>().Shoot();
                //weapon2.transform.parent = transform.parent;

                //weapon2.GetComponent<Rigidbody>().AddForce(weapon2.transform.forward * _speed);
                //Destroy(weapon2, 3f);
                yield return new WaitForSeconds(_fireRate);
            }
        }

        protected override void HitSound()
        {
            FMODUnity.RuntimeManager.PlayOneShot(_hitSound, transform.position);
        }
    }
}

