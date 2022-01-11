using PolloScripts.Enums;
using UnityEngine;

namespace PolloScripts.Data
{
    [CreateAssetMenu(menuName = "Data/Weapon/Projectile")]
    public class ProjectileData : WeaponData
    {
        public float speed = 50;
        public float lifeTime = 2;
        //public float fireRate = 1f;

        //public WeaponType WeaponType => WeaponType.Projectile;
    }
}

