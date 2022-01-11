using PolloScripts.Enums;
using PolloScripts.Interfaces;
using System;
using UnityEngine;

namespace PolloScripts.Items
{
    public abstract class CommonItem : ItemBase, IVisible
    {
        [Header("***** FMOD *****")]
        [FMODUnity.EventRef]
        [SerializeField] protected string _itemFMOD;
        public override ItemType ItemType => ItemType.Common;

        //public abstract CommonItemType CommonItemType { get; }

        #region IItem
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
            if(gameObject.activeInHierarchy)
                ReturnToPool();
        }
        protected abstract void ReturnToPool();
        #endregion

    }
}

