using PolloScripts.Data;
using PolloScripts.Enums;
using PolloScripts.ObjectPoolSystem;
using PolloScripts.WeaponSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Enemies.Bosses
{
    public class BossWeapon06 : MonoBehaviour
    {
        public Transform spawnPositionTR;
        public List<Vector3> targetPositionList;

        [Header("Weapon")]
        public Vector3 rotation;
        public int damage;
        public float fireRate;


        public Transform bossParent;

        private float nextFire;
        private bool _isFiring;

        private List<BossCrossLaser> _crossList;

        public void Init(BossWeaponData5 data)
        {
            _crossList = new List<BossCrossLaser>();
            targetPositionList = data.targetDestinyList;
            fireRate = data.fireRate;
            damage = data.damage;
        }
        public void Activate()
        {
            _isFiring = true;
            Fire();
        }
        public void Stop()
        {
            _isFiring = false;
            nextFire = 0;
            StopAllCoroutines();

            foreach (var item in _crossList)
            {
                if (item.gameObject.activeInHierarchy)
                    item.Deactivate();
            }
        }
        private void Fire()
        {
            StartCoroutine(IenFire());
        }
        IEnumerator IenFire()
        {
            _crossList = new List<BossCrossLaser>();
            foreach (var destiny in targetPositionList)
            {
                Vector3 newPosition;
                if (rotation.z > 0)
                {
                    newPosition = new Vector3(destiny.x, destiny.y, destiny.z);
                }
                else
                {
                    newPosition = new Vector3(destiny.x * -1, destiny.y, destiny.z);
                }

               
                BossCrossLaser weapon = (BossCrossLaser)ObjectPoolManager.Instance.ActivateBossProjectile( BossProjectileType.CrossLaser);
                weapon.SetProjectile(TargetType.Player, bossParent, spawnPositionTR.position, spawnPositionTR.rotation.eulerAngles, damage, 0, 0);
                //weapon.SetParent(bossParent);
                //weapon.SetPosition(spawnPositionTR.position);
                //weapon.SetRotation(spawnPositionTR.rotation.eulerAngles);
                //weapon.SetTarget(TargetType.Player);
                //weapon.SetDamage(damage);

                weapon.Activate();
 
                weapon.Move(new CrossTrajectory(), newPosition, rotation);
                _crossList.Add(weapon);
                yield return new WaitForSeconds(fireRate);
            }
        }
    }
}