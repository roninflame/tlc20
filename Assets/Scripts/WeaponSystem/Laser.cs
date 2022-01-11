using PolloScripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PolloScripts.WeaponSystem
{
    public abstract class Laser : WeaponBase
    {
        [SerializeField] protected LayerMask _layerMask;
        [SerializeField] protected float _maxLength = 200f;


        [Space]
        [Header("FMOD")]
        [FMODUnity.EventRef]
        [SerializeField] protected string _weaponFMOD;

        //protected Transform _pivotRefTF;
        //public Action OnContact;
        public override WeaponType WeaponType => WeaponType.Laser;

        //FMOD
        protected FMOD.Studio.EventInstance aSoundInstance;
        protected abstract void PlayWeaponSound();
        protected abstract void StopWeaponSound();
        //public abstract LaserType LaserType { get; }
        public void SetLaser(TargetType targetType, Transform parent, Vector3 position, Vector3 rotation)
        {
            SetTarget(targetType);
            if (parent != null)
            {
                SetParent(parent);
                SetLocalPosition(position);
                SetLocalRotation(rotation);
            }
            else
            {
                SetPosition(position);
                SetRotation(rotation);
            }
        }
        public void SetDamage(int damage)
        {
            _damage = damage;
        }
        public void SetTarget(TargetType targetType)
        {
            _targetType = targetType;
        }
        public virtual void SetParent(Transform parent)
        {
            transform.parent = parent;
        }
        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
        public void SetRotation(Vector3 rotation)
        {
            transform.rotation = Quaternion.Euler(rotation);
        }
        public void SetLocalPosition(Vector3 position)
        {
            transform.localPosition = position;
        }
        public void SetLocalRotation(Vector3 rotation)
        {
            transform.localRotation = Quaternion.Euler(rotation);
        }
        public abstract void DisablePrepare();
        public abstract void ReturnToPool();
        public void SetRaycastLength(float value)
        {
            _maxLength = value;
        }
    }

}
