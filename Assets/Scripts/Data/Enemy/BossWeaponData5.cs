using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Data
{
    [CreateAssetMenu(menuName = "Data/Enemy/Boss Weapon 5")]
    public class BossWeaponData5 : ScriptableObject
    {
        [Header("Weapon")]
        public int damage =1;
        public float fireRate;
        public List<Vector3> targetDestinyList;
    }


}
