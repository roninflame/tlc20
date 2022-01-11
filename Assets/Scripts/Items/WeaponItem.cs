using PolloScripts.Enums;
using PolloScripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Items
{
    public abstract class WeaponItem : ItemBase, IVisible
    {
        [Header("***** FMOD *****")]
        [FMODUnity.EventRef]
        [SerializeField] protected string _itemFMOD;
        public override ItemType ItemType => ItemType.Weapon;
        protected abstract void PlaySound();
        public override void Activate()
        {
            base.Activate();
            ShowModel(false);
        }
        public override void Deactivate()
        {
            StopAllCoroutines();
            _canInteract = false;
            if (gameObject.activeInHierarchy)
                ReturnToPool();
        }

        protected abstract void ReturnToPool();
    }

}
