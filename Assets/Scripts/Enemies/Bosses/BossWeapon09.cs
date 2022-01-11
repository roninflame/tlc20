using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PolloScripts.WeaponSystem;
using PolloScripts.ObjectPoolSystem;
using PolloScripts.Enums;
using PolloScripts.Data;
using PolloScripts.Interfaces;

namespace PolloScripts.Enemies.Bosses
{
    public class BossWeapon09 : MonoBehaviour
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
        public bool folllowPlayer;
        public float speedRotation = 4;

        public Transform bossParent;

        private float nextFire;

        private List<BossRocket1> _weaponList;
        private bool _isFiring;

        private bool _waitForFiring;

        Coroutine corPattern;
     
        public void Init(BossWeaponData1 data)
        {
            speedTarget = data.movementSpeedTarget;
            fireRate = data.fireRate;
            speedWeapon = data.speed;
            lifeTimeWeapon = data.lifeTime;
            damage = data.damage;
            targetPositionList = data.targetDestinyList;
        }
        public void Activate()
        {
            _isFiring = true;

            if(corPattern == null){
                corPattern = StartCoroutine(IenFirePattern());
            }else{
                 corPattern = StartCoroutine(IenFirePattern());
            }
        }
        public void Pause()
        {
            nextFire = 0;
            StopCoroutine(corPattern);
            _isFiring = false;
            folllowPlayer = false;

            if (_weaponList != null)
            {
                foreach (BossRocket1 item in _weaponList)
                {
                    if (item != null)
                    {
                        if(!item.CanMove && item.gameObject.activeInHierarchy)
                            item.Deactivate();
                    }
                        
                }
            }
        }
        public void Stop()
        {
            _waitForFiring = false;
            _isFiring = false;
            nextFire = 0;
            StopCoroutine(corPattern);

            folllowPlayer = false;
            
            if(_weaponList != null)
            {
                foreach (BossRocket1 item in _weaponList)
                {
                    if(item != null)
                        item.Deactivate();
                }
            }
        }

        private void SetBullets()
        {
            Vector3 weaponPos = new Vector3(6, 1f, 0);
            float targetPosY = 6.5f;
            _weaponList = new List<BossRocket1>();

            for (int i = 0; i < 2; i++)
            {
                weaponPos.x = 7;
                for (int j = 0; j < 8; j++)
                {
                    BossRocket1 weapon = (BossRocket1)ObjectPoolManager.Instance.ActivateBossProjectile(BossProjectileType.Rocket1);
                    weapon.SetProjectile(TargetType.Player, spawnPositionTR, weaponPos, new Vector3(0,0,0), damage, speedWeapon, lifeTimeWeapon);
                    weapon.Activate();

                    _weaponList.Add(weapon);

                    weaponPos.x -= 2;

                }
                weaponPos.y -= 1f;
                targetPosY -= 6.5f;
            }
        }

        IEnumerator IenFirePattern(){

            SetBullets();
            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < _weaponList.Count; i++)
            {
                _weaponList[i].transform.parent = null;
                _weaponList[i].Shoot();
                if (!_isFiring)
                    break;
                yield return new WaitForSeconds(0.5f);

                if (!_isFiring)
                    break;
            }

            yield return new WaitForSeconds(fireRate);
        }

    }
}
     