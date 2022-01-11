using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Data
{
    [System.Serializable]
    public class FactoryLevelData
    {
        public List<EnemyAsteroidData> asteroidList;
        public List<EnemySpaceCraftData> spaceCraftList;
        public List<ItemData> itemList;
        
    }
    [System.Serializable]
    public class FactoryPlayerData
    {
        public ProjectileData doubleProjectile;
        public LaserData laserBeam;
    }
    [CreateAssetMenu(menuName = "Data/Level/FactoryData")]
    public class FactoryData : ScriptableObject
    {
        public List<FactoryLevelData> levelData;
        public List<LaserData> laserList;
        public List<ProjectileData> projectileList;
        public FactoryPlayerData playerData;

        
    }

}
