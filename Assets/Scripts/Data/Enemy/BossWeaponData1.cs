using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Data
{
    [CreateAssetMenu(menuName = "Data/Enemy/Boss Weapon 1")]
    public class BossWeaponData1 : ScriptableObject
    {
        [Header("Weapon")]

        public float movementSpeedTarget = 2;
        public float fireRate;
        public float speed;
        public float lifeTime = 4f;
        public int damage = 1;

        public List<Vector3> targetDestinyList;

    }


}
