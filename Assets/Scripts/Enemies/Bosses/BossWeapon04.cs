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
    public class BossWeapon04 : MonoBehaviour
    {
        public Transform spawnPositionTR;
        public Transform targetTR;
        public List<Vector3> targetPositionList;
        public float speedTarget;
        public int damage;
        private Laser _laser;

        Tween tween;
        bool loop;

        void Update()
        {
            spawnPositionTR.LookAt(targetTR);
        }
        public void Init(BossWeaponData2 data)
        {
            damage = data.damage;
            speedTarget = data.movementSpeedTarget;
            targetTR.parent = null;
            targetTR.transform.position = targetPositionList[0];
        }
        public void Activate()
        {
            loop = true;
            if (_laser == null)
            {
                _laser = ObjectPoolManager.Instance.ActivateBossLaser(BossLaserType.LaserSmoke);
                _laser.SetLaser(TargetType.Player, spawnPositionTR, Vector3.zero, Vector3.zero);
                _laser.SetDamage(damage);
            }

            StartCoroutine(IenMoveWeapon());
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
            _laser.SetRaycastLength(100f);
            _laser.Activate();
            yield return new WaitForSeconds(1);
            _laser.Shoot();

            do
            {
                foreach (var item in targetPositionList)
                {
                    //tween = targetUI.DOAnchorPos(item, speedTarget);
                    tween = targetTR.transform.DOMove(item, speedTarget);
                    yield return tween.WaitForCompletion();
                }
            } while (loop);

        }
    }

}
