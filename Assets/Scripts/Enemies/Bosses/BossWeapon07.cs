using DG.Tweening;
using PolloScripts.Data;
using PolloScripts.Enums;
using PolloScripts.ObjectPoolSystem;
using PolloScripts.WeaponSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Enemies.Bosses
{
    public class BossWeapon07 : MonoBehaviour
    {
        public Transform spawnPositionTR;
        public float speedTarget;
        public int damage;
        private BossFireLaser _laser;
        public Transform targetTR;
        public List<Vector3> targetPositionList;

        public bool IsFiring => _isFiring;

        private bool _isFiring;
        private void Update()
        {
            spawnPositionTR.LookAt(targetTR);
        }
        public void Init(BossWeaponData2 data)
        {
            damage = data.damage;
            speedTarget = data.movementSpeedTarget;
            targetPositionList = data.targetPositionList;
            _laser = null;
            _isFiring = false;
        }

        public void Activate()
        {
            if(_laser == null)
            {
                _laser = (BossFireLaser)ObjectPoolManager.Instance.ActivateBossLaser( BossLaserType.FireLaser);

            }

            _laser.SetLaser(TargetType.Player, spawnPositionTR, Vector3.zero, Vector3.zero);
            _laser.SetDamage(damage);

            targetTR.parent = null;
            targetTR.position = targetPositionList[0];
            _laser.Activate();
            _isFiring = true;

            //StartCoroutine(IenMoveWeapon());
        }
        public void Stop()
        {
            StopAllCoroutines();
            DOTween.Kill(targetTR);
            if(_laser != null)
            {
                _laser.Deactivate();
                _laser = null;
            }

            _isFiring = false;
        }

        public void Fire()
        {
            StartCoroutine(IenFire());
        }
        IEnumerator IenFire()
        {
            StartCoroutine(IenLaserScale());
            _laser.Shoot();


            for (int i = 1; i < targetPositionList.Count; i++)
            {
                Tween move1 = targetTR.DOMove(targetPositionList[i], speedTarget);
                yield return move1.WaitForCompletion();
            }

            yield return null;


        }

        IEnumerator IenMoveWeapon()
        {
          
            targetTR.parent = null;
            targetTR.position = targetPositionList[0];

            _laser.Activate();
            _isFiring = true;
            yield return new WaitForSeconds(1);
            StartCoroutine(IenLaserScale());
            _laser.Shoot();

            //DOTween.To(() => balance, x => balance = x, to, 2)

            for (int i = 1; i < targetPositionList.Count; i++)
            {
                Tween move1 = targetTR.DOMove(targetPositionList[i], speedTarget);
                yield return move1.WaitForCompletion();
            }

           
            //yield return new WaitForSeconds(4);
            //_laser.SetLaserScale(50);
            //do
            //{
            //    foreach (var item in targetPositionList)
            //    {
            //        //tween = targetUI.DOAnchorPos(item, speedTarget);
            //        tween = targetTR.transform.DOMove(item, speed);
            //        yield return tween.WaitForCompletion();
            //    }
            //} while (loop);

        }

        IEnumerator IenLaserScale()
        {
            float laserScaleMax = _laser.LaserScale;
            _laser.SetLaserScale(5f);
            while (_laser.LaserScale < laserScaleMax)
            {
                _laser.SetLaserScale(_laser.LaserScale + 0.5f);
                //print("% : " + _laser.LaserScale);
                yield return new WaitForSeconds(0.01f);
            }

            _laser.SetLaserScale(laserScaleMax);
            print("fin scale");
        }
    }

}
