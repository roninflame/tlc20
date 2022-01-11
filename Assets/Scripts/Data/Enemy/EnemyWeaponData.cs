using PolloScripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Data
{
    [CreateAssetMenu(menuName = "Data/EnemyWeapon")]
    public class EnemyWeaponData : ScriptableObject
    {
        [Space]
        [Header("Base")]
        [Space]
        public WeaponType asset;
        public int typeID;
        public float weaponFireRate = 1;
        public float weaponSpeed = 50;
        public int weaponDamage = 1;
    }
}


