using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Data
{
    [CreateAssetMenu(menuName = "Data/Enemy/SpaceCraft")]
    public class EnemySpaceCraftData : EnemyData
    {
        public float windSpeed;
        public float windStrength;

        [Space]
        [Header("==== SpaceCraft 2")]
        public int ammoAmount2;
        public float fireRate2;

        [Space]
        [Header("==== SpaceCraft 4, 6 , 7, 8")]
        public int ammoAmount = 2;
        public float fireRate = 1;

        [Space]
        [Header("==== SpaceCraft 3, 5 ====")]
        public float rotateToPlayer = 0.5f;
        public float waitToFollowPlayer = 0.5f;
        public float durationToFollowPlayer = 1.5f;
    }
}

