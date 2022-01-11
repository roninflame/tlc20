using PolloScripts.Data;
using PolloScripts.Enums;
using PolloScripts.WeaponSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.ObjectPoolSystem
{
    public class ObjectPoolWeapon
    {
        #region Projectile
        public Projectile GetProjectile(ProjectileType itemID, TargetType targetType, ProjectileData data)
        {
            Projectile result = null;
            switch (itemID)
            {
                case ProjectileType.None:
                    break;
                case ProjectileType.EnergyBullet:
                    result = GetBullet(data, targetType);
                    break;
                case ProjectileType.DoubleEnergyBullet:
                    result = GetDoubleBullet(data, targetType);
                    break;
                case ProjectileType.EnergyBall:
                    result = GetEnergyBall(data, targetType);
                    break;
                case ProjectileType.EnergyBullet2:
                    result = GetBullet2(data, targetType);
                    break;
                case ProjectileType.DEnergyBullet:
                    result = GetDEnergy(data, targetType);
                    break;
                default:
                    break;
            }

            return result;
        }
        private Projectile GetBullet(ProjectileData data, TargetType target)
        {
            EnergyBullet obj = ObjectPoolBullet.Instance.ActivateFromPool();
            obj.Init(data, target);
            return obj;
        }
        private Projectile GetBullet2(ProjectileData data, TargetType target)
        {
            EnergyBullet2 obj = ObjectPoolBullet2.Instance.ActivateFromPool();
            obj.Init(data, target);
            return obj;
        }
        private Projectile GetEnergyBall(ProjectileData data, TargetType target)
        {
            EnergyBall obj = ObjectPoolEnergyBall.Instance.ActivateFromPool();
            obj.Init(data, target);
            return obj;
        }
        private Projectile GetDEnergy(ProjectileData data, TargetType target)
        {
            DoubleEnergyBall obj = ObjectPoolDoubleEnergyBall.Instance.ActivateFromPool();
            obj.Init(data, target);
            return obj;
        }
        private Projectile GetDoubleBullet(ProjectileData data, TargetType target)
        {
            DoubleEnergyBullet obj = ObjectPoolDoubleBullet.Instance.ActivateFromPool();
            obj.Init(data, target);
            return obj;
        }

        #endregion

        #region Lasers
        public Laser GetLaser(LaserType itemID, TargetType targetType, LaserData data, Transform parent, Vector3 position, Quaternion rotation)
        {
            Laser result = null;

            switch (itemID)
            {
                case LaserType.None:
                    break;
                case LaserType.LaserBeam:
                    result = GetLaserBeam(data, targetType);
                    break;
                case LaserType.LaserSmoke:
                    result = GetLaserSmoke(data, targetType);
                    break;
                default:
                    break;
            }

            return result;
        }
        private Laser GetLaserBeam(LaserData data, TargetType target)
        {
            LaserBeam obj = ObjectPoolLaserBeam.Instance.ActivateFromPool();
            obj.Init(data, target);
            return obj;
        }
        private Laser GetLaserSmoke(LaserData data, TargetType target)
        {
            LaserSmoke obj = ObjectPoolLaserSmoke.Instance.ActivateFromPool();
            obj.Init(data, target);
            return obj;
        }
        #endregion

        #region Player Projectiles
        public Projectile GetPlayerProjectile(PlayerProjectileType itemID, TargetType targetType, ProjectileData data)
        {
            Projectile result = null;
            switch (itemID)
            {
                case PlayerProjectileType.None:
                    break;
                case PlayerProjectileType.Basic:
                    result = GetPlayerBasic(data, targetType);
                    break;
                case PlayerProjectileType.EnemyChaser:
                    result = GetPlayerEnemyChaser(data, targetType);
                    break;
                default:
                    break;
            }
            return result;
        }
        private Projectile GetPlayerBasic(ProjectileData data, TargetType target)
        {
            PlayerBasicProjectile obj = ObjectPoolPlayerBasic.Instance.ActivateFromPool();
            obj.Init(data, target);
            return obj;
        }
        private Projectile GetPlayerEnemyChaser(ProjectileData data, TargetType target)
        {
            PlayerEnemyChaser obj = ObjectPoolPlayerEnemyChaser.Instance.ActivateFromPool();
            obj.Init(data, target);
            return obj;
        }

        #endregion

        #region Player Lasers
        public Laser GetPlayerLaser(PlayerLaserType itemID, TargetType targetType, LaserData data, Transform parent, Vector3 position, Quaternion rotation)
        {
            Laser result = null;

            switch (itemID)
            {
                case PlayerLaserType.None:
                    break;
                case PlayerLaserType.LaserBeam:
                    result = GetPlayerLaserBeam(data, targetType);
                    break;
               
                default:
                    break;
            }

            return result;
        }
        private Laser GetPlayerLaserBeam(LaserData data, TargetType target)
        {
            PlayerLaserBeam obj = ObjectPoolPlayerLaserBeam.Instance.ActivateFromPool();
            obj.Init(data, target);
            return obj;
        }
        #endregion

        #region Boss
        public Projectile GetBossProjectile(BossProjectileType itemID)
        {
            Projectile result = null;


            switch (itemID)
            {
                case BossProjectileType.None:
                    break;
                case BossProjectileType.EnergyBullet:
                    result = ObjectPoolBossEnergyBullet.Instance.ActivateFromPool();
                    break;
                case BossProjectileType.Rocket:
                    result = ObjectPoolBossRocket.Instance.ActivateFromPool();
                    break;
                case BossProjectileType.CrossLaser:
                    result = ObjectPoolBossCrossLaser.Instance.ActivateFromPool();
                    break;
                case BossProjectileType.EnergyBall:
                    result = ObjectPoolBossEnergyBall.Instance.ActivateFromPool();
                    break;
                case BossProjectileType.Rocket1:
                    result = ObjectPoolBossRocket1.Instance.ActivateFromPool();
                    break;
                default:
                    break;
            }
            return result;
        }
        public Laser GetBossLaser(BossLaserType itemID)
        {
            Laser result = null;


            switch (itemID)
            {
                case BossLaserType.None:
                    break;
                case BossLaserType.LaserBeam:
                    result = ObjectPoolBossLaserBeam.Instance.ActivateFromPool();
                    break;
                case BossLaserType.LaserSmoke:
                    result = ObjectPoolBossLaserSmoke.Instance.ActivateFromPool();
                    break;
                case BossLaserType.FireLaser:
                    result = ObjectPoolBossFireLaser.Instance.ActivateFromPool();
                    break;
                case BossLaserType.DoubleLaserBeam:
                    result = ObjectPoolBossDoubleLaserBeam.Instance.ActivateFromPool();
                    break;
                default:
                    break;
            }
            return result;
        }

        public void DeactivateBossProjectile(BossProjectileType itemID)
        {

            switch (itemID)
            {
                case BossProjectileType.None:
                    break;
                case BossProjectileType.EnergyBullet:
    
                    List<BossEnergyBullet> pool = ObjectPoolBossEnergyBullet.Instance.ReturnActiveObject();

                    foreach (var item in pool)
                    {
                        if (item.gameObject.activeInHierarchy)
                            item.Deactivate();
                    }
                    break;
                case BossProjectileType.Rocket:
                    List<BossRocket> pool2 = ObjectPoolBossRocket.Instance.ReturnActiveObject();

                    foreach (var item in pool2)
                    {
                        if (item.gameObject.activeInHierarchy)
                            item.Deactivate();
                    }
                    break;
                case BossProjectileType.CrossLaser:
                    List<BossCrossLaser> pool3 = ObjectPoolBossCrossLaser.Instance.ReturnActiveObject();

                    foreach (var item in pool3)
                    {
                        if (item.gameObject.activeInHierarchy)
                            item.Deactivate();
                    }
                    break;
                case BossProjectileType.EnergyBall:

                    List<BossEnergyBall> pool4 = ObjectPoolBossEnergyBall.Instance.ReturnActiveObject();

                    foreach (var item in pool4)
                    {
                        if (item.gameObject.activeInHierarchy)
                            item.Deactivate();
                    }
                    break;
                default:
                    break;
            }
         
        }

        #endregion

        #region MiniBoss
        public Projectile GetMiniBossProjectile(ProjectileType itemID, TargetType targetType, ProjectileData data)
        {
            Projectile result = null;
            switch (itemID)
            {
                case ProjectileType.None:
                    break;
                case ProjectileType.EnergyBullet:
                    result = GetBullet(data, targetType);
                    break;
                case ProjectileType.DoubleEnergyBullet:
                    result = GetDoubleBullet(data, targetType);
                    break;
                case ProjectileType.EnergyBall:
                    result = GetEnergyBall(data, targetType);
                    break;
                default:
                    break;
            }

            return result;
        }
        #endregion
    }
}