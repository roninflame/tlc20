using PolloScripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Data
{
    [CreateAssetMenu(menuName = "Data/Weapon/LaserBeam")]
    public class LaserData : WeaponData
    {
        //public float charge = 1f;
        //public int duration = 5;
        //public float rotateToPlayer = 0.5f;

        public WeaponType WeaponType => WeaponType.Laser;
    }
}

