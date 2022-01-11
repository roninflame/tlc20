using PolloScripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolloScripts.Enums;
using System;
using PolloScripts.Enemies;

namespace PolloScripts.WeaponSystem
{
    public abstract class WeaponBase : MonoBehaviour
    {
        [Header("Base")]
        //[SerializeField] protected int _typeID;
        [SerializeField] protected int _damage = 10;
        ////protected WeaponType _weaponType;

        [SerializeField] protected TargetType _targetType;

        public AssetType AssetType => AssetType.Weapon;
        public abstract WeaponType WeaponType { get; }
        public int Damage => _damage;
        public TargetType TargetType => _targetType;
        public abstract void Activate();

        public abstract void Deactivate();
        public abstract void Shoot();



        protected bool DoDamage(GameObject other)
        {
            bool res = false;
            if(_targetType == TargetType.Player)
            {
                IPlayer player = other.GetComponentInParent<IPlayer>();
                if (player != null)
                {
                    if (player.CanTakeDamage)
                    {
                        player.DamageReceived(_damage);
                    }
                    res = true;
                }
            }
            else if(_targetType == TargetType.Enemy)
            {
                EnemyBase enemy = other.GetComponentInParent<EnemyBase>();
                if (enemy != null)
                {
                    if (enemy.CanTakeDamage)
                    {
                        enemy.DamageReceived(_damage);
                        res = true;
                    }
                    else
                    {
                        res = false;
                    }

                }
                else if(WeaponType == WeaponType.Laser)
                {
                    Props.PropController prop = other.GetComponentInParent<Props.PropController>();
                    if(prop != null)
                    {
                        prop.DamageReceived(Damage);
                    }
                }
            }
            return res;
        }
    }
}

