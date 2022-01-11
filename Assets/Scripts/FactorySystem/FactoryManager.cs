using PolloScripts.Data;
using PolloScripts.Enemies;
using PolloScripts.Enums;
using PolloScripts.Items;
using PolloScripts.ObjectPoolSystem;
using PolloScripts.WeaponSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PolloScripts.FactorySystem
{
    public class FactoryManager : MonoBehaviour
    {
        public static FactoryManager Instance;

        [SerializeField] private AssetManager _assetManager;

        [Space]
        [Header("Asset parameters")]
        [SerializeField] private FactoryData _fatoryData;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public void CreateAssets()
        {
            //_assetManager.CreateAssets();
            //_assetManager.CreateEnemies();
            //_assetManager.CreateItem();
            //_assetManager.CreateWeapons();
        }

        #region Assets
        public void GetAsset(AssetType assetType, int typeID, Transform parent, Vector3 position, Quaternion rotation)
        {
            Transform assetTransform = _assetManager.GetAsset(assetType, typeID, parent, position, rotation);

            switch (assetType)
            {
                case AssetType.None:
                    break;
                case AssetType.Effect:
                    //Explosion explosion = assetTransform.GetComponent<Explosion>();
                    //explosion?.Init(1f);
                    //explosion?.Activate();
                    break;

            }
        }

        #endregion

        #region ITEM
        public CommonItem GetItem(ItemType assetType, int typeID, Transform parent, Vector3 position, Quaternion rotation)
        {
            //Transform assetTransform = _assetManager.GetItem(assetType, typeID, parent, position, rotation);
              ItemBaseData param = _fatoryData.levelData[0].itemList.Single(x => x.typeID == typeID);
            switch (assetType)
            {
                case ItemType.None:
                    break;
                case ItemType.Common:
                   
                    if (typeID == 1)
                    {
                        EnergyItem energy = ObjectPoolEnergyItem.Instance.ActivateFromPool();
                        energy.Init(param.value, param.scoreValue,parent,position,rotation);
                        energy.transform.position = position;
                        energy.Activate();
                    }
                    else if (typeID == 2)
                    {
                        //HealthItem health = HealthItemObjectPool.Instance.ActivateFromPool();
                        //health.Init(param.value, param.scoreValue, parent, position, rotation);
                        //health.Activate();
                    }
                    else if (typeID == 3)
                    {
                        //WeaponItem weapon = WeaponItemObjectPool.Instance.ActivateFromPool();
                        //weapon.Init(param.value, param.scoreValue, parent, position, rotation);
                        //weapon.Activate();
                    }
                    break;
                case ItemType.Weapon:
                    break;
                default:
                    break;
            }
            return null;
        }
        //private ItemBase FactoryItem(ItemType assetType, int typeID, Transform assetTransform)
        //{
        //    ItemBase result = null;

        //    switch (assetType)
        //    {
        //        case ItemType.None:
        //            break;
        //        case ItemType.Common:
        //            ItemBaseData param = _fatoryData.levelData[0].itemList.Single(x => x.typeID == typeID);
        //            ItemCommon item = assetTransform.GetComponent<ItemCommon>();
        //            item.Init(param.value, param.scoreValue);
        //            item.Activate();
        //            result = item;
        //            break;
        //        case ItemType.Weapon:

        //            break;
        //        default:
        //            break;
        //    }
        //    return result;
        //}

        #endregion

        #region ENEMY
        public EnemyBase GetEnemy(EnemyType assetType, int typeID, Transform parent, Vector3 position, Quaternion rotation)
        {
            //Transform assetTransform = _assetManager.GetEnemy(assetType, typeID, parent, position, rotation);
            EnemyAsteroidData param = _fatoryData.levelData[0].asteroidList.Single(x => x.typeID == typeID);
            switch (assetType)
            {
                case EnemyType.None:
                    break;
                case EnemyType.Asteroid:
                    if(typeID == 1)
                    {
                        SmallAsteroid asteroid1 = ObjectPoolSmallAsteroid.Instance.ActivateFromPool();
                        asteroid1.Init(param, parent, position, rotation);
                        asteroid1.Activate();
                    }
                    else if(typeID == 2)
                    {
                        MediumAsteroid asteroid2 = ObjectPoolMediumAsteroid.Instance.ActivateFromPool();
                        asteroid2.Init(param, parent, position, rotation);
                        asteroid2.Activate();
                    }
                    else if(typeID == 3)
                    {
                        BigAsteroid asteroid3 = ObjectPoolBigAsteroid.Instance.ActivateFromPool();
                        asteroid3.Init(param, parent, position, rotation);
                        asteroid3.Activate();
                    } 
                    break;
                case EnemyType.SpaceCraft:
                    break;
                default:
                    break;
            }

            //EnemyBase weapon = FactoryEnemy(assetType, typeID, assetTransform);
            return null;
        }
        //private EnemyBase FactoryEnemy(EnemyType assetType, int typeID, Transform assetTransform)
        //{
        //    EnemyBase result = null;


        //    switch (assetType)
        //    {
        //        case EnemyType.None:
        //            break;
        //        case EnemyType.Asteroid:
        //            EnemyAsteroidData param = _fatoryData.levelData[0].asteroidList.Single(x => x.typeID == typeID);
        //            EnemyAsteroid asteroid = assetTransform.GetComponent<EnemyAsteroid>();
        //            asteroid.Init(param);
        //            //asteroid.Init(param.health, param.damageValue, param.scoreValue, param.windSpeed, param.windStrength);
        //            asteroid.Activate();
        //            result = asteroid;
        //            break;
        //        case EnemyType.SpaceCraft:

        //            break;
        //        default:
        //            break;
        //    }

        //    return result;
        //}
 
        #endregion

        #region WEAPON
        public WeaponBase GetWeapon(WeaponType weaponType, int typeID, TargetType targetType, Vector3 position, Quaternion rotation)
        {
            //Obtener el asset
            Transform weaponTransform = _assetManager.GetWeapon(weaponType, typeID, null, position, rotation);
            //Inicializar la clase
            WeaponBase weapon = FactoryWeapon(weaponType, typeID, weaponTransform, targetType);
            return weapon;
        }
        private WeaponBase FactoryWeapon(WeaponType weaponType, int typeID, Transform weaponTransform, TargetType target)
        {
            //EnemyWeaponData data = _fatoryData.we .Single(x => x.asset == weaponType && x.typeID == (int)weaponType);
            WeaponBase weaponBase = null;
            //WeaponBase wbase = null; 
            switch (weaponType)
            {
                case WeaponType.None:
                    break;
                case WeaponType.Projectile:
                    ProjectileData data;

                    if(target == TargetType.Player)
                    {
                        data = _fatoryData.playerData.doubleProjectile;
                    }
                    else
                    {
                        data = _fatoryData.projectileList.Single(x => x.typeID == (int)weaponType);
                    }
                    if(typeID == 1)
                    {
                        Projectile projectile = weaponTransform.GetComponent<Projectile>();
                        //projectile.Init(data, target);
                        ////projectile.Activate();
                        ////projectile.Shoot();
                        weaponBase = projectile;
                    }else if (typeID == 2)
                    {
                        DoubleEnergyBullet dproj = weaponTransform.GetComponent<DoubleEnergyBullet>();
                        //dproj.Init(target, data.speed, data.damage, data.fireRate);
                        //dproj.Activate();
                        //dproj.Shoot();
                        weaponBase = dproj;
                    }

                    //projectile.Init(target, data.weaponSpeed, data.weaponDamage);

                    break;
                case WeaponType.Laser:
                    //LaserBeam laserBeam = weaponTransform.GetComponent<LaserBeam>();
                    //LaserData data2 = _fatoryData.laserList.Single(x => x.typeID == (int)weaponType);
                    //laserBeam.Init(target, data2.damage);
                    //weaponBase = laserBeam;
                    break;
                default:
                    weaponBase = weaponTransform.GetComponent<WeaponBase>();
                    break;

            }
            return weaponBase;
        }
        #endregion

        public void ReturnToPool(AssetType asset, int assetType, int typeID, Transform objectToReturn)
        {
            _assetManager.ReturnToPool(asset, assetType, typeID, objectToReturn);
        }
    }


}
