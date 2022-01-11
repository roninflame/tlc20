using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PolloScripts.WeaponSystem;
using PolloScripts.ObjectPoolSystem;
using PolloScripts.Enums;
using PolloScripts.Data;
using UnityEngine.Events;

namespace PolloScripts.Enemies.Bosses
{
    public class BossWeapon01 : MonoBehaviour
    {
        public Transform spawnPositionTR;
        public float speedTarget = 2;
        public Transform targetTR;
        public List<Vector3> targetPositionList;

        [Header("Weapon")]
        public int damage;
        public float fireRate;
        public float speedWeapon;
        public float lifeTimeWeapon = 4f;
        //public bool loop;
        public bool folllowPlayer;
        public float speedRotation = 4;
        private float nextFire;

        Tween tween;
        //private int _currentTargetPosition;
        private bool _isFiring;
        //private bool _moveCrossHair;

        private void Update()
        {
            //if(_moveCrossHair)
            //    targetUI.transform.position = Camera.main.WorldToScreenPoint(targetTR.position);
            if (!folllowPlayer)
            {
                spawnPositionTR.LookAt(targetTR);
            }
            else
            {
                Quaternion rot1 = Quaternion.LookRotation(PlayerManager.Instance.player.hitPosition - spawnPositionTR.position);
                spawnPositionTR.rotation = Quaternion.RotateTowards(spawnPositionTR.rotation, rot1, Time.deltaTime * speedRotation);
                //spawnPositionTR.LookAt(targetTR);
            }

            if (Time.time > nextFire && _isFiring)
            {

                nextFire = Time.time + fireRate;
                Fire();
            }
        }
        public void Init(BossWeaponData1 data)
        {
            speedTarget = data.movementSpeedTarget;
            fireRate = data.fireRate;
            speedWeapon = data.speed;
            lifeTimeWeapon = data.lifeTime;
            damage = data.damage;
     
        }
        public void Activate()
        {
            _isFiring = true;
            spawnPositionTR.LookAt(PlayerManager.Instance.player.hitPosition + (Vector3.one * 5));
            StartCoroutine(IenMoveTarget());
        }
        public void Stop()
        {
            tween.Kill();
            _isFiring = false;
            nextFire = 0;
            StopAllCoroutines();
            folllowPlayer = false;
        }
        private void Fire()
        {
            Projectile weapon = ObjectPoolManager.Instance.ActivateBossProjectile( BossProjectileType.EnergyBall);
            weapon.SetProjectile(TargetType.Player, null, spawnPositionTR.position, spawnPositionTR.rotation.eulerAngles, damage, speedWeapon, lifeTimeWeapon);
            weapon.Activate();
            weapon.Shoot();
        }
        private void FireToPlayer()
        {
            Projectile weapon = ObjectPoolManager.Instance.ActivateBossProjectile(BossProjectileType.EnergyBullet);
            weapon.SetProjectile(TargetType.Player, null, spawnPositionTR.position, Quaternion.LookRotation(PlayerManager.Instance.player.hitPosition - spawnPositionTR.position).eulerAngles, damage, speedWeapon, lifeTimeWeapon);
            weapon.Activate();
            weapon.Shoot();
        }

        IEnumerator IenMoveTarget()
        {
            targetTR.transform.position = targetPositionList[targetPositionList.Count - 1];
            do
            {
                foreach (var item in targetPositionList)
                {
                    //tween = targetUI.DOAnchorPos(item, speedTarget);
                    tween = targetTR.transform.DOMove(item, speedTarget);
                    yield return tween.WaitForCompletion();
                }
                if (Random.Range(0, 1) == 0)
                    FireToPlayer();
                //yield return new WaitForSeconds(1f);
            } while (true);

        }

    }

}
