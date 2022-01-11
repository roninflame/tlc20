using PolloScripts.Data;
using PolloScripts.Effects;
using PolloScripts.ObjectPoolSystem;
using PolloScripts.WeaponSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PolloScripts.Enemies.Bosses
{
    public class Boss01Module1 : BossBaseModule
    {
        public BossHealth bossHealth;
        [Header("Weapon 1")]
        public List<BossWeapon01> weapon1List;
        public List<BossWeaponData1> weaponDataList;

        [Header("Weapon 2")]
        public BossWeapon02 weapon2;
        public List<BossWeaponData2> weaponData2List;

        public UnityEvent OnChangeLevel;

        public override void StartModule()
        {
            StartCoroutine(IenStartModule());
        }

        #region Events
        public override void OnDeath()
        {
            print("**** DEATH ****");
            StopWeapons();
            bossHealth.Deactivate();

            StartCoroutine(IenExplosions());
            ObjectPoolManager.Instance.DeactivateBossProjectile(Enums.BossProjectileType.EnergyBullet);
            ObjectPoolManager.Instance.DeactivateBossProjectile(Enums.BossProjectileType.EnergyBall);
        }

        public override void OnLostHealth()
        {
            print("**** LOST HEALTH ****");
            _currentLevel++;
            StopWeapons();
            bossHealth.Stop();
            StartCoroutine(IenRestartModule());
        }
        #endregion


        protected override void SetWeaponData(int level)
        {
            foreach (var item in weapon1List)
            {
                item.Init(weaponDataList[level]);
            }

            //weapon2.Init(weaponData2List[level]);
        }

       
        protected override void ActivateWeapons()
        {
            foreach (var item in weapon1List)
            {
                item.Activate();
            }
            //weapon2.Activate();
        }

        protected override void StopWeapons()
        {
            foreach (var item in weapon1List)
            {
                item.Stop();
            }
            //weapon2.Stop();
        }
        IEnumerator IenStartModule()
        {
            print("//// Start ////");
            yield return new WaitForSeconds(1f);
            bossHealth.Activate();
            SetWeaponData(0);
            ActivateWeapons();
        }

        IEnumerator IenRestartModule()
        {
            print("//// ReStart ////");
            yield return new WaitForSeconds(1f);
            bossHealth.ReStart();
            SetWeaponData(_currentLevel);
            ActivateWeapons();

        }

        protected override IEnumerator IenExplosions()
        {
            print("//// Explosions ////");
            foreach (var item in _explosionPointsList)
            {
                ExplosionBase explosion = ObjectPoolManager.Instance.ActivateExplosion(Enums.ExplosionType.Fire, null, item, Quaternion.identity);
                explosion.ReSize(20);
                explosion.Activate();
                yield return new WaitForSeconds(0.2f);
            }
            OnChangeLevel.Invoke();
        }
    }
}