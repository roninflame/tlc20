using PolloScripts.Data;
using PolloScripts.Effects;
using PolloScripts.ObjectPoolSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Enemies.Bosses
{
    public class BossModule02 : MonoBehaviour
    {
        public BossModule module;

        [Header("Weapon 3")]
        public List<BossWeapon03> weapon3List;
        public List<BossWeaponData1> weaponData3List;

        [Header("Weapon 4")]
        public List<BossWeapon04> weaponList4;
        public List<BossWeaponData2> weaponDataList4;

        public int currentDataIndex;
        public int currentHealth;
        public void SetModule()
        {
            foreach (var item in module.bossHealthList)
            {
                item.Activate();
                //item.OnDestroy += ModuleDestroyed;
                //currentHealth = item.healthPointsList.Count;
            }

        }
        public void SetData(int index)
        {
            currentDataIndex = index;
            foreach (var item in weaponList4)
            {
                item.Init(weaponDataList4[index]);
            }
            foreach (var item in weapon3List)
            {
                item.Init(weaponData3List[index]);
            }


            setExplosionPoint();
        }

        public void ActivateWeapon()
        {
            foreach (var item in weaponList4)
            {
                //item.folllowPlayer = true;
                item.Activate();

            }

            foreach (var item in weapon3List)
            {
                //item.folllowPlayer = true;
                item.Activate();

            }
        }

        public void ModuleDestroyed()
        {
            //DeactivateModule();
            StopWeapons();
            RestartWeapons();
        }
        public void StopWeapons()
        {
            foreach (var item in weaponList4)
            {
                item.Stop();
            }

            foreach (var item in weapon3List)
            {
                item.Stop();
            }
        }
        public void RestartWeapons()
        {
            StartCoroutine(IenRestarting());
        }
        public void DeactivateModule()
        {
            module.moduleGO.SetActive(false);
            foreach (var item in module.bossHealthList)
            {
                //item.OnDestroy -= ModuleDestroyed;
                //currentHealth++;
            }

            //StopWeapons();
        }

        IEnumerator IenRestarting()
        {

            foreach (var item in module.explosionPointsList[0])
            {
                ExplosionBase explosion = ObjectPoolManager.Instance.ActivateExplosion(Enums.ExplosionType.Fire, null, item, Quaternion.identity);
                explosion.ReSize(20);
                explosion.Activate();
                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitForSeconds(2);

            DeactivateModule();

        }

        private void setExplosionPoint()
        {
            module.explosionPointsList = new List<List<Vector3>>();
            List<Vector3> exp1 = new List<Vector3>();
            exp1.Add(new Vector3(3, 0, 65));
            exp1.Add(new Vector3(18, 0, 53));
            exp1.Add(new Vector3(38,0 , 35));
            exp1.Add(new Vector3(20, 5, 47));

            module.explosionPointsList.Add(exp1);

            //List<Vector3> exp2 = new List<Vector3>();
            //exp2.Add(new Vector3(0, 0, 75));
            //exp2.Add(new Vector3(15, 15, 75));
            //exp2.Add(new Vector3(-15, -15, 75));
            //exp2.Add(new Vector3(5, 5, 75));

            //module.explosionPointsList.Add(exp2);

            //List<Vector3> exp3 = new List<Vector3>();
            //exp3.Add(new Vector3(0, 0, 75));
            //exp3.Add(new Vector3(15, 15, 75));
            //exp3.Add(new Vector3(-15, -15, 75));
            //exp3.Add(new Vector3(5, 5, 75));

            //module.explosionPointsList.Add(exp3);
        }
    }


}
