using PolloScripts.Data;
using PolloScripts.Effects;
using PolloScripts.Enemies;
using PolloScripts.Enums;
using PolloScripts.WeaponSystem;
using System.Linq;
using UnityEngine;

namespace PolloScripts.ObjectPoolSystem
{
    public class ObjectPoolManager : MonoBehaviour
    {
        public static ObjectPoolManager Instance { get; private set; }
        [SerializeField] private LevelData _levelData01;
        [SerializeField] private LevelData _levelData02;
        [SerializeField] private LevelData _levelData03;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
        private LevelData GetData()
        {
            if(GameManager.Instance?.CurrentScene == SceneIndexes.LEVEL_01)
            {
                return _levelData01;
            }
            else if (GameManager.Instance?.CurrentScene == SceneIndexes.LEVEL_02)
            {
                return _levelData02;
            }
            else if (GameManager.Instance?.CurrentScene == SceneIndexes.LEVEL_03)
            {
                return _levelData03;
            }
            else
            {
                return _levelData01;
            }
        }
        public AttackPattern GetAttackPattern(int index)
        {
            if (GameManager.Instance?.CurrentScene == SceneIndexes.LEVEL_01)
            {
                return _levelData01.patternData.attackPatterns[index];
            }
            else if (GameManager.Instance?.CurrentScene == SceneIndexes.LEVEL_02)
            {
                return _levelData02.patternData.attackPatterns[index];
            }
            else if (GameManager.Instance?.CurrentScene == SceneIndexes.LEVEL_03)
            {
                return _levelData03.patternData.attackPatterns[index];
            }
            else
            {
                return _levelData01.patternData.attackPatterns[index];
            }
        }
        public ExplosionBase ActivateExplosion(ExplosionType itemID, Transform parent, Vector3 position, Quaternion rotation)
        {
            ObjectPoolEffect pool = new ObjectPoolEffect();
            ExplosionEffectData data = GetData().levelData[0].effectList.Single(x => x.typeID == (int)itemID);
            ExplosionBase result = null;

            result = pool.GetExplosion(itemID, data, parent, position, rotation);

            return result;
        }
        public EnemyBase ActivateEnemy(EnemyType itemType, int itemID, Transform parent, Vector3 position, Quaternion rotation)
        {
            ObjectPoolEnemy pool = new ObjectPoolEnemy();
                EnemyBase result = null;
            switch (itemType)
            {
                case EnemyType.None:
                    break;
                case EnemyType.Asteroid:
                    EnemyAsteroidData data = GetData().levelData[0].asteroidList.Single(x => x.typeID == (int)itemID);
                    pool.GetAsteroid((AsteroidType)itemID, data, parent, position, rotation);
                    break;
                case EnemyType.SpaceCraft:
                    EnemySpaceCraftData spaceData = GetData().levelData[0].spaceCraftList.Single(x => x.typeID == (int)itemID);
                    result = pool.GetSpaceCraft((SpaceCraftType)itemID, spaceData, parent, position, rotation);
                    break;
                case EnemyType.Cherub:
                    EnemySpaceCraftData cherubData = GetData().levelData[0].cherubimList.Single(x => x.typeID == (int)itemID);
                    result = pool.GetCherub(itemID, cherubData, parent);
                    break;
                default:
                    break;
            }

            return result;
        }
        public SpaceShip ActivateSpaceCraft(SpaceCraftType itemID, Transform parent, Vector3 position, Quaternion rotation)
        {
            ObjectPoolEnemy pool = new ObjectPoolEnemy();
            SpaceShip result = null;
            EnemySpaceCraftData spaceData = GetData().levelData[0].spaceCraftList.Single(x => x.typeID == (int)itemID);
            result = pool.GetSpaceCraft(itemID, spaceData, parent, position, rotation);

            return result;
        }
        public Projectile ActivateProjectile(ProjectileType typeID, TargetType targetType)
        {
            ObjectPoolWeapon pool = new ObjectPoolWeapon();
            Projectile result = null;

            ProjectileData data = GetData().weaponData.projectileList.Single(x => x.typeID == (int)typeID);
            result = pool.GetProjectile(typeID, targetType, data);
            return result;
        }
        public Laser ActivateLaser(LaserType typeID, TargetType targetType, Transform parent, Vector3 position, Quaternion rotation)
        {
            ObjectPoolWeapon pool = new ObjectPoolWeapon();
            Laser result = null;

            LaserData data = GetData().weaponData.laserList.Single(x => x.typeID == (int)typeID);
            result = pool.GetLaser(typeID, targetType, data, parent, position, rotation);
            return result;
        }

        #region Player
        public void ActivateItem(ItemType itemType, int itemID, Transform parent, Vector3 position, Quaternion rotation)
        {
            ObjectPoolItem pool = new ObjectPoolItem();
            ItemBaseData data = GetData().levelData[0].itemList.Single(x => x.typeID == (int)itemID);

            switch (itemType)
            {
                case ItemType.None:
                    break;
                case ItemType.Common:
                    pool.GetCommonItem((CommonItemType)itemID, data, parent, position, rotation);
                    break;
                default:
                    break;
            }
        }
        public void ActivateWeaponItem(WeaponItemType itemID, Transform parent, Vector3 position, Quaternion rotation)
        {
            ObjectPoolItem pool = new ObjectPoolItem();
            ItemBaseData data = GetData().levelData[0].weaponItemList.Single(x => x.typeID == (int)itemID);

            pool.GetWeaponItem(itemID, data, parent, position, rotation);
        }
        public void ActivateCharacterItem(CharacterItemType itemID, int trayectoria, Transform parent, Vector3 position, Quaternion rotation)
        {
            ObjectPoolItem pool = new ObjectPoolItem();
            ItemBaseData data = GetData().levelData[0].characterItemList.Single(x => x.typeID == (int)itemID);

            pool.GetCharacterItem(itemID, data, trayectoria, parent, position, rotation);
        }
        public Projectile ActivatePlayerProjectile(PlayerProjectileType typeID)
        {
            ObjectPoolWeapon pool = new ObjectPoolWeapon();
            Projectile result = null;

            ProjectileData data = GetData().playerData.projectileList.Single(x => x.typeID == (int)typeID);
            result = pool.GetPlayerProjectile(typeID,  TargetType.Enemy, data);
            return result;
        }

        public Laser ActivatePlayerLaser(PlayerLaserType typeID, Transform parent, Vector3 position, Quaternion rotation)
        {
            ObjectPoolWeapon pool = new ObjectPoolWeapon();
            Laser result = null;

            LaserData data = GetData().playerData.laserList.Single(x => x.typeID == (int)typeID);
            result = pool.GetPlayerLaser(typeID, TargetType.Enemy, data, parent, position, rotation);
            return result;
        }

        #endregion

        #region Boss
        public Projectile ActivateBossProjectile(BossProjectileType typeID)
        {
            ObjectPoolWeapon pool = new ObjectPoolWeapon();
            Projectile result = pool.GetBossProjectile(typeID); 
            return result;
        }
        public Laser ActivateBossLaser(BossLaserType typeID)
        {
            ObjectPoolWeapon pool = new ObjectPoolWeapon();
            Laser result = pool.GetBossLaser(typeID);
            return result;
        }
        public void DeactivateBossProjectile(BossProjectileType typeID)
        {
            ObjectPoolWeapon pool = new ObjectPoolWeapon();
            pool.DeactivateBossProjectile(typeID);
 
        }
        #endregion

        #region MiniBoss
        public Projectile ActivateMiniBossProjectile(ProjectileType typeID, TargetType targetType, ProjectileData data)
        {
            ObjectPoolWeapon pool = new ObjectPoolWeapon();
            Projectile result = null;
            result = pool.GetProjectile(typeID, targetType, data);
            return result;
        }
        //public Laser ActivateMiniBossLaser(BossLaserType typeID)
        //{
        //    ObjectPoolWeapon pool = new ObjectPoolWeapon();
        //    Laser result = pool.GetBossLaser(typeID);
        //    return result;
        //}
        //public void DeactivateBossProjectile(BossProjectileType typeID)
        //{
        //    ObjectPoolWeapon pool = new ObjectPoolWeapon();
        //    pool.DeactivateBossProjectile(typeID);

        //}
        #endregion
    }

}
