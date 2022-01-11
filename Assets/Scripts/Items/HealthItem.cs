using DG.Tweening;
using PolloScripts.Enums;
using PolloScripts.Interfaces;
using PolloScripts.ObjectPoolSystem;
using PolloScripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Items
{
    public class HealthItem : CommonItem
    {
        public void Init(int value, int scoreValue, Transform parent, Vector3 position, Quaternion rotation)
        {
            this._scoreValue = scoreValue;
            this._itemValue = value;
            //_particle.SetActive(false);
            _canInteract = false;

            if (parent != null)
            {
                transform.parent = parent;
                transform.localPosition = position;
                transform.localRotation = rotation;
            }
            else
            {
                transform.position = position;
                transform.rotation = rotation;
            }
        }
        protected override void ReturnToPool()
        {
            ObjectPoolHealthItem.Instance.ReturnToPool(this);
        }
        private void OnTriggerEnter(Collider other)
        {
            IPlayer player = other.GetComponentInParent<IPlayer>();
            if (player != null && _canInteract)
            {
                CanvasManager.Instance.healthBar?.AddValue(_itemValue);

                _canInteract = false;
                //PlayerManager.Instance.SetParticleForceField(2, 8);
                StartCoroutine(IenDeactivate());
            }
        }

        private IEnumerator IenDeactivate()
        {
            ShowModel(false);
            yield return new WaitForSeconds(2f);
            Deactivate();
        }
        public override void ShowModel(bool show)
        {
            base.ShowModel(show);
            if (show) StartAnimation();
        }
        public override void Deactivate()
        {
            base.Deactivate();
            StopAnimation();
        }
        public override void StartAnimation()
        {
            Vector3 scale1 = Vector3.one * 2f;
            float timeScale1 = 2;
            _modelHolderGO.transform.localScale = Vector3.one;
            _modelHolderGO.transform.DOScale(scale1, timeScale1);
        }
        public override void StopAnimation()
        {
            DOTween.Kill(_modelHolderGO.transform);
        }

        protected override void PlaySound()
        {
            throw new System.NotImplementedException();
        }
    }

}
