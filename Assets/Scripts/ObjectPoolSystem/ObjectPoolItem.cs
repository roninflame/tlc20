using PolloScripts.Data;
using PolloScripts.Enums;
using PolloScripts.Items;
using UnityEngine;

namespace PolloScripts.ObjectPoolSystem
{
    public class ObjectPoolItem 
    {
        #region CommonItem
        public ItemBase GetCommonItem(CommonItemType itemID, ItemBaseData data, Transform parent, Vector3 position, Quaternion rotation)
        {
            ItemBase result = null;

            switch (itemID)
            {
                case CommonItemType.None:
                    break;
                case CommonItemType.Energy:
                    result = GetEnergyItem(data, parent, position, rotation);
                    break;
                case CommonItemType.Health:
                    result = GetHealthItem(data, parent, position, rotation);
                    break;
                case CommonItemType.WeaponPoweUp:
                    result = GetWeaponLvlUpItem(data, parent, position, rotation);
                    break;
                default:
                    break;
            }

            return result;
        }

        private ItemBase GetEnergyItem(ItemBaseData data, Transform parent, Vector3 position, Quaternion rotation)
        {
            EnergyItem energy = ObjectPoolEnergyItem.Instance.ActivateFromPool();
            energy.Init(data.value, data.scoreValue, parent, position, rotation);
            energy.Activate();

            return energy;
        }

        private ItemBase GetHealthItem(ItemBaseData data, Transform parent, Vector3 position, Quaternion rotation)
        {
            HealthItem health = ObjectPoolHealthItem.Instance.ActivateFromPool();
            health.Init(data.value, data.scoreValue, parent, position, rotation);
            health.Activate();

            return health;
        }
        private ItemBase GetWeaponLvlUpItem(ItemBaseData data, Transform parent, Vector3 position, Quaternion rotation)
        {
            WeaponPowerUpItem weapon = ObjectPoolWeaponPowerUpItem.Instance.ActivateFromPool();
            weapon.Init(data.value, data.scoreValue, parent, position, rotation);
            weapon.Activate();

            return weapon;
        }


        #endregion

        #region WeaponItem

        public ItemBase GetWeaponItem(WeaponItemType itemID, ItemBaseData data, Transform parent, Vector3 position, Quaternion rotation)
        {
            ItemBase result = null;

            switch (itemID)
            {
                case WeaponItemType.None:
                    break;
                case WeaponItemType.LaserBeam:
                    result = GetLaserBeamItem(data, parent, position, rotation);
                    break;
                case WeaponItemType.EnemyChaser:
                    result = GetEnemyChaserItem(data, parent, position, rotation);
                    break;
                default:
                    break;
            }

            return result;
        }

        private WeaponItem GetEnemyChaserItem(ItemBaseData data, Transform parent, Vector3 position, Quaternion rotation)
        {
            EnemyChaserItem weapon = ObjectPoolEnemyChaserItem.Instance.ActivateFromPool();
            weapon.Init(data.value, data.scoreValue, parent, position, rotation);
            weapon.Activate();

            return weapon;
        }
        private WeaponItem GetLaserBeamItem(ItemBaseData data, Transform parent, Vector3 position, Quaternion rotation)
        {
            LaserBeamItem weapon = ObjectPoolLaserBeamItem.Instance.ActivateFromPool();
            weapon.Init(data.value, data.scoreValue, parent, position, rotation);
            weapon.Activate();

            return weapon;
        }
        #endregion

        #region Character Item
        public ItemBase GetCharacterItem(CharacterItemType itemID, ItemBaseData data, int trayectoria, Transform parent, Vector3 position, Quaternion rotation)
        {
            ItemBase result = null;

            switch (itemID)
            {
                case CharacterItemType.None:
                    break;
                case CharacterItemType.Perro:
                    result = GetPerroItem(data, trayectoria, parent, position, rotation);
                    break;
               
                default:
                    break;
            }

            return result;
        }

        private CharacterItem GetPerroItem(ItemBaseData data, int trayectoria, Transform parent, Vector3 position, Quaternion rotation)
        {
            PerroItem perro = ObjectPoolPerroItem.Instance.ActivateFromPool();
            perro.Init(data.value,data.scoreValue, trayectoria, parent, position, rotation);
            perro.Activate();

            return perro;
        }


        #endregion


    }
}

