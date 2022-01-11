using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Data
{
    [CreateAssetMenu(menuName = "Data/Enemy/Boss Weapon 2")]
    public class BossWeaponData2 : ScriptableObject
    {
        [Header("Weapon")]

        public float movementSpeedTarget = 2;
        public int damage = 1;
        public List<Vector3> targetPositionList;

    }


}
