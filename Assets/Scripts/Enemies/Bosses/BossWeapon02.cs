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
    public class BossWeapon02 : MonoBehaviour
    {
        public Transform spawnPositionTR;
        public Transform targetTR;
        public List<Vector3> targetPositionList;

        public int damage;
        public float speed;
        public bool loop;

        private Laser _laser;
        Tween tween;

        private void Update()
        {
            loop = true;
            spawnPositionTR.LookAt(targetTR);
        }

        public void Activate()
        {
            if (_laser == null)
            {
                _laser = ObjectPoolManager.Instance.ActivateBossLaser(BossLaserType.LaserBeam);
                _laser.SetLaser(TargetType.Player, spawnPositionTR, Vector3.zero, Vector3.zero);
                _laser.SetDamage(damage);
            }

            loop = true;
            StartCoroutine(IenMoveWeapon());
        }
        public void Init(BossWeaponData2 data)
        {
            damage = data.damage;
            targetPositionList = data.targetPositionList;
            targetTR.transform.position = targetPositionList[0];
        }
        public void Stop()
        {
            tween.Kill();
            loop = false;
            StopAllCoroutines();
            _laser?.DisablePrepare();
        }
        IEnumerator IenMoveWeapon()
        {
            _laser.Activate();
            yield return new WaitForSeconds(1);
            _laser.Shoot();
  
            do
            {
                foreach (var item in targetPositionList)
                {
                    //tween = targetUI.DOAnchorPos(item, speedTarget);
                    tween = targetTR.transform.DOMove(item, speed);
                    yield return tween.WaitForCompletion();
                }
            } while (loop);

        }
    }


}
