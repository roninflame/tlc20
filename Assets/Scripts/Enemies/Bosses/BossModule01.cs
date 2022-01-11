using PolloScripts.Data;
using PolloScripts.Effects;
using PolloScripts.ObjectPoolSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Enemies.Bosses
{
    public class BossModule01 : MonoBehaviour
    {
        public BossModule module;
        [Header("Weapon 1")]
        public List<BossWeapon01> weapon1List;
        public List<BossWeaponData1> weaponDataList;

        [Header("Weapon 2")]
        public BossWeapon02 weapon2;
        public List<BossWeaponData2> weaponData2List;

        public bool restarting;

        public int currentHealth;
        public int destroyedHealth;
        public int currentDataIndex;

        public void SetData(int index)
        {
            currentDataIndex = index;
            foreach (var item in weapon1List)
            {
                item.Init(weaponDataList[index]);
            }

            weapon2.Init(weaponData2List[index]);

            setExplosionPoint();
        }

        public void SetModule()
        {
            foreach (var item in module.bossHealthList)
            {
                item.Activate();
                //item.OnDestroy += ModuleDestroyed;
                //currentHealth = item.healthPointsList.Count;
            }
        }
        public void ActivateWeapon()
        {
            foreach (var item in weapon1List)
            {
                item.Activate();
            }
            weapon2.Activate();
        }
        public void ModuleDestroyed()
        {
            destroyedHealth++;

            if (!restarting)
            {
                restarting = true;
                StopWeapons();
                RestartWeapons();
            }
        }
        public void StopWeapons()
        {
            foreach (var item in weapon1List)
            {
                item.Stop();
            }

            foreach (var item in module.bossHealthList)
            {
                item.Stop();
            }

            weapon2.Stop();
        }
        public void RestartWeapons()
        {
            StartCoroutine(IenRestarting());
        }
        public void DeactivateModule()
        {
            ////module.moduleGO.SetActive(false);
            foreach (var item in module.bossHealthList)
            {
                //item.OnDestroy -= ModuleDestroyed;
                //currentHealth++;
            }
        }
        IEnumerator IenRestarting()
        {
            
            foreach (var item in module.explosionPointsList[destroyedHealth - 1])
            {
                ExplosionBase explosion = ObjectPoolManager.Instance.ActivateExplosion(Enums.ExplosionType.Fire, null, item, Quaternion.identity);
                explosion.ReSize(20);
                explosion.Activate();
                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitForSeconds(2);

            if(currentHealth != destroyedHealth)
            {
                SetData(currentDataIndex + 1);

                foreach (var item in module.bossHealthList)
                {
                    item.Activate();
                }
                ActivateWeapon();
                restarting = false;
            }
            else
            {
                DeactivateModule();
            }

           
        }
        private void setExplosionPoint()
        {
            module.explosionPointsList = new List<List<Vector3>>();
            List<Vector3> exp1 = new List<Vector3>();
            exp1.Add(new Vector3(0, 0, 75));
            exp1.Add(new Vector3(15, 15, 75));
            exp1.Add(new Vector3(-15, -15, 75));
            exp1.Add(new Vector3(5, 5, 75));

            module.explosionPointsList.Add(exp1);

            List<Vector3> exp2 = new List<Vector3>();
            exp2.Add(new Vector3(0, 0, 75));
            exp2.Add(new Vector3(15, 15, 75));
            exp2.Add(new Vector3(-15, -15, 75));
            exp2.Add(new Vector3(5, 5, 75));

            module.explosionPointsList.Add(exp2);

            List<Vector3> exp3 = new List<Vector3>();
            exp3.Add(new Vector3(0, 0, 75));
            exp3.Add(new Vector3(15, 15, 75));
            exp3.Add(new Vector3(-15, -15, 75));
            exp3.Add(new Vector3(5, 5, 75));

            module.explosionPointsList.Add(exp3);
        }
    }

}
