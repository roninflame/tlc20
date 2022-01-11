using PolloScripts.Data;
using PolloScripts.Enemies;
using PolloScripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.ObjectPoolSystem
{
    public class ObjectPoolEnemy 
    {
        #region Asteroid
        public EnemyBase GetAsteroid(AsteroidType itemID, EnemyAsteroidData data, Transform parent, Vector3 position, Quaternion rotation)
        {
            EnemyBase result = null;

            switch (itemID)
            {
                case AsteroidType.None:
                    break;
                case AsteroidType.Small:
                    result = GetAsteroid1(data, parent, position, rotation);
                    break;
                case AsteroidType.Medium:
                    result = GetAsteroid2(data, parent, position, rotation);
                    break;
                case AsteroidType.Big:
                    result = GetAsteroid3(data, parent, position, rotation);
                    break;
                case AsteroidType.Indestructible:
                    result = GetAsteroid4(data, parent, position, rotation);
                    break;
                default:
                    break;
            }

            return result;
        }

        private EnemyBase GetAsteroid1(EnemyAsteroidData data, Transform parent, Vector3 position, Quaternion rotation)
        {
            SmallAsteroid obj = ObjectPoolSmallAsteroid.Instance.ActivateFromPool();
            obj.Init(data, parent, position, rotation);
            obj.Activate();
            return obj;
        }
        private EnemyBase GetAsteroid2(EnemyAsteroidData data, Transform parent, Vector3 position, Quaternion rotation)
        {
            MediumAsteroid obj = ObjectPoolMediumAsteroid.Instance.ActivateFromPool();
            obj.Init(data, parent, position, rotation);
            obj.Activate();
            return obj;
        }
        private EnemyBase GetAsteroid3(EnemyAsteroidData data, Transform parent, Vector3 position, Quaternion rotation)
        {
            BigAsteroid obj = ObjectPoolBigAsteroid.Instance.ActivateFromPool();
            obj.Init(data, parent, position, rotation);
            obj.Activate();
            return obj;
        }
        private EnemyBase GetAsteroid4(EnemyAsteroidData data, Transform parent, Vector3 position, Quaternion rotation)
        {
            IndestructibleAsteroid obj = ObjectPoolIndestructibleAsteroid.Instance.ActivateFromPool();
            obj.Init(data, parent, position, rotation);
            obj.Activate();
            return obj;
        }
        #endregion

        public SpaceShip GetSpaceCraft(SpaceCraftType itemID, EnemySpaceCraftData data, Transform parent, Vector3 position, Quaternion rotation)
        {
            SpaceShip result = null;

            switch (itemID)
            {
                case SpaceCraftType.None:
                    break;
                case SpaceCraftType.SpaceCraft1:
                    break;
                case SpaceCraftType.SpaceCraft2:
                    result = GetSpaceCraft2(data, parent);
                    break;
                case SpaceCraftType.SpaceCraft3:
                    result = GetSpaceCraft3(data, parent, position, rotation);
                    break;
                case SpaceCraftType.SpaceCraft4:
                    result = GetSpaceCraft4(data, parent, position, rotation);
                    break;
                case SpaceCraftType.SpaceCraft5:
                    result = GetSpaceCraft5(data, parent, position, rotation);
                    break;
                case SpaceCraftType.SpaceCraft6:
                    result = GetSpaceCraft6(data, parent);
                    break;
                case SpaceCraftType.SpaceCraft7:
                    result = GetSpaceCraft7(data, parent);
                    break;
                case SpaceCraftType.SpaceCraft8:
                    result = GetSpaceCraft8(data, parent, position, rotation);
                    break;
                default:
                    break;
            }
            return result;
        }

        private SpaceShip GetSpaceCraft2(EnemySpaceCraftData data, Transform parent)
        {
            SpaceCraft2 obj = ObjectPoolSpaceCraft2.Instance.ActivateFromPool();
            obj.Init(data, parent);
            return obj;
        }

        private SpaceShip GetSpaceCraft3(EnemySpaceCraftData data, Transform parent, Vector3 position, Quaternion rotation)
        {
            SpaceCraft3 obj = ObjectPoolSpaceCraft3.Instance.ActivateFromPool();
            obj.Init(data, parent,position,rotation);
            return obj;
        }
        private SpaceShip GetSpaceCraft4(EnemySpaceCraftData data, Transform parent, Vector3 position, Quaternion rotation)
        {
            SpaceCraft4 obj = ObjectPoolSpaceCraft4.Instance.ActivateFromPool();
            obj.Init(data, parent, position, rotation);
            obj.Activate();
            return obj;
        }
        private SpaceShip GetSpaceCraft5(EnemySpaceCraftData data, Transform parent, Vector3 position, Quaternion rotation)
        {
            SpaceCraft5 obj = ObjectPoolSpaceCraft5.Instance.ActivateFromPool();
            obj.Init(data, parent, position, rotation);
            obj.Activate();
            return obj;
        }
        private SpaceShip GetSpaceCraft6(EnemySpaceCraftData data, Transform parent)
        {
            SpaceCraft6 obj = ObjectPoolSpaceCraft6.Instance.ActivateFromPool();
            obj.Init(data, parent);
            obj.Activate();
            return obj;
        }

        private SpaceShip GetSpaceCraft7(EnemySpaceCraftData data, Transform parent)
        {
            SpaceCraft7 obj = ObjectPoolSpaceCraft7.Instance.ActivateFromPool();
            obj.Init(data, parent);
            return obj;
        }
        private SpaceShip GetSpaceCraft8(EnemySpaceCraftData data, Transform parent, Vector3 position, Quaternion rotation)
        {
            SpaceCraft8 obj = ObjectPoolSpaceCraft8.Instance.ActivateFromPool();
            obj.Init(data, parent, position, rotation);
            obj.Activate();
            return obj;
        }
        public Cherub GetCherub(int itemID, EnemySpaceCraftData data, Transform parent)
        {
            Cherub result = null;

            switch (itemID)
            {
                case 0:
                    break;
                case 1:
                    result = GetCherub1(data, parent);
                    break;
               
                default:
                    break;
            }
            return result;
        }

        private Cherub GetCherub1(EnemySpaceCraftData data, Transform parent)
        {
            Cherub1 obj = ObjectPoolCherub1.Instance.ActivateFromPool();
            obj.Init(data, parent);
            //obj.Activate();
            return obj;
        }

    }

}
