using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PolloScripts.WeaponSystem;
using PolloScripts.ObjectPoolSystem;
using PolloScripts.Enums;
using PolloScripts.Data;

namespace PolloScripts.Enemies.Bosses
{
    public class BossWeapon05 : MonoBehaviour
    {
        public Transform spawnPositionTR;
        public List<Vector3> targetPositionList;

        [Header("Weapon")]
        public int damage;
        public float fireRate;
        public bool IsFireing => _isFiring;
        //private float nextFire;
        private bool _isFiring;

        private List<BossRocket> _rocketList;

        public Transform bossParent;

        public void Init(BossWeaponData5 data)
        {
            _rocketList = new List<BossRocket>();
            targetPositionList = data.targetDestinyList;
            damage = data.damage;
            fireRate = data.fireRate;
        }
        //public void Activate()
        //{
        //    //_isFiring = true;
        //    Fire();
        //}
        public void Pause()
        {
            StopAllCoroutines();
        }

        public void StopFiring()
        {
            _isFiring = false;
        }
        public void Stop()
        {
            //_isFiring = false;
            //nextFire = 0;
            StopAllCoroutines();

            foreach (var item in _rocketList)
            {
                if(item.gameObject.activeInHierarchy)
                    item.Deactivate();
            }
        }
        public void Fire()
        {
            StartCoroutine(IenFire());
        }

        IEnumerator IenFire()
        {
            _isFiring = true;
            _rocketList = new List<BossRocket>();
            foreach (var destiny in targetPositionList)
            {
                BossRocket weapon = (BossRocket) ObjectPoolManager.Instance.ActivateBossProjectile(BossProjectileType.Rocket);
                weapon.SetParent(bossParent);
                weapon.SetPosition(spawnPositionTR.position);
                weapon.SetRotation(spawnPositionTR.rotation.eulerAngles);
                weapon.SetTarget(TargetType.Player);
                weapon.SetDamage(damage);
                weapon.SetSpeed(0);
                weapon.SetLifeTime(0);

                //weapon.SetProjectile(TargetType.Player, bossParent, spawnPositionTR.position, spawnPositionTR.rotation.eulerAngles, damage, 0, 0);
                weapon.Activate();
                //weapon.Shoot();

                weapon.Move(new RocketTrajectory(), destiny, Vector3.zero);
                _rocketList.Add(weapon);
                yield return new WaitForSeconds(fireRate);
            }

            //_isFiring = false;
        }
    }
}