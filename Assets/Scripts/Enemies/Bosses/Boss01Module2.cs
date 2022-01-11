using PolloScripts.Data;
using PolloScripts.Effects;
using PolloScripts.ObjectPoolSystem;
using PolloScripts.UI;
using PolloScripts.WeaponSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PolloScripts.Enemies.Bosses
{
    public class Boss01Module2 : BossBaseModule
    {
        public Transform bossTra;
        public BossHealth bossHealth;
        [Header("Weapon 3")]
        public List<BossWeapon03> weapon3List;
        public List<BossWeaponData1> weapon3DataList;

         [Header("Weapon 9")]
        public List<BossWeapon09> weapon9List;
        public List<BossWeaponData1> weapon9DataList;

        [Header("Weapon 4")]
        public List<BossWeapon04> weapon4List;
        public List<BossWeaponData2> weapon4DataList;

        public UnityEvent OnChangeLevel;

        public override void OnDeath()
        {
            print("**** DEATH ****");
            AddScore();

            CanvasManager.Instance.OnSaveScore.Invoke();

            //StopWeapons();
            PauseModule();
            bossHealth.Deactivate();
            StartCoroutine(IenExplosions());
            //ObjectPoolManager.Instance.DeactivateBossProjectile(Enums.BossProjectileType.EnergyBullet);
        }

        public override void OnLostHealth()
        {
            print("**** LOST HEALTH ****");
            _currentLevel++;
            StopWeapons();
            bossHealth.Stop();
            StartCoroutine(IenRestartModule());
        }

        public override void StartModule()
        {
            bossHealth.Activate();
            SetWeaponData(0);
            foreach (var item in weapon9List)
            {
                item.bossParent = bossTra.parent;
            }
            //StartCoroutine(IenStartModule());
        }

        public void CanTakeDamage(bool value)
        {
            bossHealth.SetTakeDamage(value);
        }
        public void StopModule()
        {
            StopWeapons();
            //bossHealth.Stop();
        }
        public void PauseModule()
        {
            foreach (var item in weapon9List)
            {
                item.Pause();
            }
        }
        public void Attack()
        {
            ActivateWeapons();
        }
        protected override void ActivateWeapons()
        {
            foreach (var item in weapon9List)
            {
                item.Activate();
            }

            //foreach (var item in weapon4List)
            //{
            //    item.Activate();
            //}
            // foreach (var item in weapon3List)
            // {
            //     item.Activate();
            // }
        }

        protected override void SetWeaponData(int level)
        {
            // foreach (var item in weapon4List)
            // {
            //     item.Init(weapon4DataList[level]);
            // }

            // foreach (var item in weapon3List)
            // {
            //     item.Init(weapon3DataList[level]);
            // }

             foreach (var item in weapon9List)
            {
                item.Init(weapon9DataList[level]);
            }
        }

        protected override void StopWeapons()
        {
            // foreach (var item in weapon4List)
            // {
            //     item.Stop();
            // }
            // foreach (var item in weapon3List)
            // {
            //     item.Stop();
            // }
             foreach (var item in weapon9List)
            {
                item.Stop();
            }
        }

        

        //IEnumerator IenStartModule()
        //{
        //    bossHealth.Activate();
        //    SetWeaponData(0);
        //    yield return new WaitForSeconds(2f);
        //    ActivateWeapons();
        //}
        IEnumerator IenRestartModule()
        {
            print("//// ReStart ////");
            yield return new WaitForSeconds(2f);
            bossHealth.ReStart();
            SetWeaponData(_currentLevel);
            ActivateWeapons();

        }

        protected override IEnumerator IenExplosions()
        {
            print("//// Explosions ////");
            foreach (var item in _explosionPointsList)
            {
                ExplosionBase explosion = ObjectPoolManager.Instance.ActivateExplosion(Enums.ExplosionType.Fire, bossTra.parent, item, Quaternion.identity);
                explosion.ReSize(20);
                explosion.Activate();
                yield return new WaitForSeconds(0.2f);
            }
            OnChangeLevel.Invoke();
        }
    }

}
