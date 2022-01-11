using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Data
{
    [Serializable]
    public class LevelAssetData
    {
        public List<EnemyAsteroidData> asteroidList;
        public List<EnemySpaceCraftData> spaceCraftList;
        public List<EnemySpaceCraftData> cherubimList;
        public List<ItemData> itemList;
        public List<ItemData> weaponItemList;
        public List<ItemData> characterItemList;
        public List<ExplosionEffectData> effectList;

    }
    [Serializable]
    public class LevelPlayerData
    {
        public List<ProjectileData> projectileList;
        public List<LaserData> laserList;
    }
    [Serializable]
    public class LevelWeaponData
    {
        public List<ProjectileData> projectileList;
        public List<LaserData> laserList;

    }
    [Serializable]
    public class PatternData
    {
        public AttackPattern[] attackPatterns;
    }
    [CreateAssetMenu(menuName = "Data/Level/Level Data")]
    public class LevelData : ScriptableObject
    {

        public List<LevelAssetData> levelData;

        public LevelPlayerData playerData;

        public LevelWeaponData weaponData;

        public PatternData patternData;

    }



}
