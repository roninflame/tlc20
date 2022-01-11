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
    public class BossWeapon08 : MonoBehaviour
    {
        public Transform spawnPositionR;
        public Transform spawnPositionL;
        public int damage;
        public float speedTarget;
        public Transform targetTR;
        public List<Vector3> targetPositionList;

        private BossDoubleLaserBeam _laserR;
        private BossDoubleLaserBeam _laserL;

        private void Update()
        {
            transform.LookAt(targetTR);
        }
        public void Init(BossWeaponData2 data)
        {
            damage = data.damage;
            speedTarget = data.movementSpeedTarget;
            targetPositionList = data.targetPositionList;
            _laserL = null;
            _laserR = null;
        }
        public void Activate()
        {
            if (_laserR == null)
            {
                _laserR = (BossDoubleLaserBeam)ObjectPoolManager.Instance.ActivateBossLaser(BossLaserType.DoubleLaserBeam);
                _laserR.SetLaser(TargetType.Player, spawnPositionR, Vector3.zero, Vector3.zero);
                _laserR.SetDamage(damage);
                _laserR.Activate();
                _laserR.SetLaserScale(20);
                _laserR.Shoot();
            }
            if (_laserL == null)
            {
                _laserL = (BossDoubleLaserBeam)ObjectPoolManager.Instance.ActivateBossLaser(BossLaserType.DoubleLaserBeam);
                _laserL.SetLaser(TargetType.Player, spawnPositionL, Vector3.zero, Vector3.zero);
                _laserL.SetDamage(damage);
                _laserL.Activate();
                _laserL.SetLaserScale(20);
                _laserL.Shoot();
            }
            StartCoroutine(IenMoveWeapon());
        }
        public void Stop()
        {
            StopAllCoroutines();
            _laserR.Deactivate();
            _laserL.Deactivate();

            _laserR = null;
            _laserL = null;
        }
        IEnumerator IenMoveWeapon()
        {
            targetTR.parent = null;
            targetTR.position = targetPositionList[0];

            yield return new WaitForSeconds(1);


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
    }

}
