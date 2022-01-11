using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Data
{
    [CreateAssetMenu(menuName = "Data/Pattern/AttackPattern")]
    public class AttackPattern : ScriptableObject
    {
        [Space]
        [Header("==== Projectile ===")]
        public float fireRate = 0.5f;
        public int ammo;

        //[Space]
        //[Header("==== Laser ===")]

    }

}
