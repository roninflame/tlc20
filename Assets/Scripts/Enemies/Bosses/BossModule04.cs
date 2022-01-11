using PolloScripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Enemies.Bosses
{
    public class BossModule04 : MonoBehaviour
    {
        public BossModule module;

        [Header("Weapon 5")]
        public BossWeapon05 weapon5;
        public List<BossWeaponData5> weapon05DataList;

        [Header("Weapon 6")]
        public List<BossWeapon06> weapon6List;
        public List<BossWeaponData5> weapon06DataList;

        [Header("Weapon 7")]
        public BossWeapon07 weapon7;
        public List<BossWeaponData2> weapon07DataList;

        [Header("Weapon 8")]
        public BossWeapon08 weapon8;
        public List<BossWeaponData2> weapon08DataList;

        public bool restarting;
        public int currentHealth;
        public int destroyedHealth;
        public int currentDataIndex;
        
        public void SetData(int index)
        {
            currentDataIndex = index;
            weapon5.Init(weapon05DataList[index]);

            foreach (var item in weapon6List)
            {
                item.Init(weapon06DataList[index]);
            }

            weapon7.Init(weapon07DataList[index]);
        }

        public void SetModule()
        {
            foreach (var item in module.bossHealthList)
            {
                item.Activate();
                //item.OnDestroy += ModuleDestroyed;
                currentHealth++;
            }
        }

        public void ModuleDestroyed()
        {
            destroyedHealth++;

            if (!restarting)
            {
                restarting = true;
                StopWeapons();
            }

            if (currentHealth == destroyedHealth)
            {
                DeactivateModule();
            }
        }
        public void StopWeapons()
        {
            weapon5.Stop();
            foreach (var item in weapon6List)
            {
                item.Stop();
            }

            foreach (var item in module.bossHealthList)
            {
                item.Stop();
            }
            StartCoroutine(IenRestarting());
        }
        public void DeactivateModule()
        {
            module.moduleGO.SetActive(false);
            foreach (var item in module.bossHealthList)
            {
                //item.OnDestroy -= ModuleDestroyed;
                currentHealth++;
            }
        }
        public void ActivateWeapon()
        {
            //weapon5.Activate();
            //foreach (var item in weapon6List)
            //{
            //    item.Activate();
            //}
            //weapon7.Activate();
            weapon8.Activate();
        }
        IEnumerator IenRestarting()
        {
            yield return new WaitForSeconds(1);

            //SetData(currentDataIndex + 1);

            foreach (var item in module.bossHealthList)
            {
                item.Activate();
            }
            ActivateWeapon();
            restarting = false;
        }
    }
}