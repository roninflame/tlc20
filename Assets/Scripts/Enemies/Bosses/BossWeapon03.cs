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
    public class BossWeapon03 : MonoBehaviour
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

        private float nextFire;

        private List<Projectile> _weaponList;
        // private List<Vector3> _positionList;

        //Tween tween;
        //private int _currentTargetPosition;
        private bool _isFiring;

        private void Update()
        {
          
            //spawnPositionTR.LookAt(targetTR);

            //if (folllowPlayer)
            //{
            //    //Quaternion rot1 = Quaternion.LookRotation(PlayerManager.Instance.player.hitPosition - spawnPositionTR.position);
            //    //spawnPositionTR.rotation = Quaternion.RotateTowards(spawnPositionTR.rotation, rot1, Time.deltaTime * speedRotation);

            //    targetTR.transform.position = Vector3.MoveTowards(targetTR.transform.position
            //        , PlayerManager.Instance.player.hitPosition, Time.deltaTime * speedRotation);

            //}
        }
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
            //targetTR.position = PlayerManager.Instance.player.hitPosition + (new Vector3(1, 1, 0) * 5);
            StartCoroutine(IenFirePattern());
        }
        public void Stop()
        {
            //tween.Kill();
            _isFiring = false;
            nextFire = 0;
            StopAllCoroutines();
            folllowPlayer = false;

            if(_weaponList != null)
            {
                foreach (var item in _weaponList)
                {
                    item.Deactivate();
                }
            }
            _weaponList.Clear();
            // _positionList.Clear();
        }

        public void Pause()
        {
            _isFiring = false;
            nextFire = 0;
            StopAllCoroutines();
            folllowPlayer = false;
        }

        private IEnumerator IenFirePattern()
        {
            while (true)
            {
                foreach (var item in targetPositionList)
                {
                    SetBullets(item);
                    yield return new WaitForSeconds(1f);
                    Fire(item);
                    for (int i = 0; i < 2; i++)
                    {
                        SetBullets(item);
                        Fire(item);
                        yield return new WaitForSeconds(0.2f);
                    }
                   

                    yield return new WaitForSeconds(fireRate);
                }
            }
        }

        private void SetBullets(Vector3 itemPosition)
        {
         
            Vector3 weaponPos = new Vector3(6, 1f, 0);
            float targetPosY = itemPosition.y + 6.5f;
            _weaponList = new List<Projectile>();
            // _positionList = new List<Vector3>();

            // Vector3 _targetPosition;

            Vector3 _rotation2 = Vector3.zero;

            if (itemPosition.y > 0)
                spawnPositionTR.localPosition = new Vector3(spawnPositionTR.localPosition.x, 1.5f, spawnPositionTR.localPosition.z);
            else
                spawnPositionTR.localPosition = new Vector3(spawnPositionTR.localPosition.x, -1.5f, spawnPositionTR.localPosition.z);

            //int nameCont = 0;

            _rotation2 = new Vector3(-1.5f, -35, 0);

            for (int i = 0; i < 2; i++)
            {
                weaponPos.x = 7;
                // _targetPosition = new Vector3(28, targetPosY, itemPosition.z);
                //42
                _rotation2 = new Vector3(_rotation2.x, -50, 0);
                for (int j = 0; j < 8; j++)
                {
                    Projectile weapon;
                    weapon = ObjectPoolManager.Instance.ActivateBossProjectile(BossProjectileType.EnergyBall);

                    weapon.SetProjectile(TargetType.Player, spawnPositionTR, weaponPos, _rotation2, damage, speedWeapon, lifeTimeWeapon);
                    weapon.Activate();
                    //weapon.name = weapon.name + "_" + nameCont;
                    _weaponList.Add(weapon);

                    // _positionList.Add(_targetPosition);

                    // _targetPosition = new Vector3(_targetPosition.x - 4, targetPosY, _targetPosition.z);
                    weaponPos.x -= 2;
                    _rotation2.y -= 2;
                    //_rotation2.y -= 3;

                }
                _rotation2.x += 3;
                weaponPos.y -= 1f;
                targetPosY -= 6.5f;
            }
        }

        private void Fire(Vector3 itemPos)
        {
            if (itemPos.y > 0)
                targetTR.localPosition = new Vector3(targetTR.localPosition.x, 4f, targetTR.localPosition.z);
            else
                targetTR.localPosition = new Vector3(targetTR.localPosition.x, -4, targetTR.localPosition.z);

            //spawnPositionTR.LookAt(targetTR.position);

            for (int i = 0; i < _weaponList.Count; i++)
            {
                _weaponList[i].transform.parent = null;
                //_weaponList[i].transform.rotation = Quaternion.LookRotation(_positionList[i] - _weaponList[i].transform.position);
                _weaponList[i].Shoot();
                //print(_weaponList[i].name + "  " + _positionList[i]);
            }
        }

       
    }

}
