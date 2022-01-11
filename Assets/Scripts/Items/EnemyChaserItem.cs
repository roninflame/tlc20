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
    public class EnemyChaserItem : WeaponItem
    {
        public WeaponType weaponType => WeaponType.Projectile;
        public PlayerProjectileType projectileType => PlayerProjectileType.EnemyChaser;
        public float duration;
        public float coolDown;
        public float fireRate = 1f;
        public Sprite iconSprite;

        private float _followSpeed = 60;
        private void Update()
        {
            if (_canInteract)
            {
                _followSpeed = PlayerManager.Instance.DollySpeed - 20;
                transform.position = Vector3.MoveTowards(transform.position, PlayerManager.Instance.player.hitPosition, _followSpeed * Time.deltaTime);

            }
        }
        public void Init(int value, int scoreValue, Transform parent, Vector3 position, Quaternion rotation)
        {
            _scoreValue = scoreValue;
            _itemValue = value;
            _canInteract = false;
            _modelGO.SetActive(true);

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
            ObjectPoolEnemyChaserItem.Instance.ReturnToPool(this);
        }
        private void OnTriggerEnter(Collider other)
        {
            IPlayer player = other.GetComponentInParent<IPlayer>();
            if (player != null && !_canInteract)
            {
                PlayerManager.Instance.playerWeapons.AddWeapon((int)weaponType, (int)projectileType, iconSprite, duration, coolDown,fireRate);
                _canInteract = true;
                StartCoroutine(IenDeactivate());
            }
        }

        private IEnumerator IenDeactivate()
        {
            PlaySound();
            transform.parent = null;
            _modelGO.SetActive(false);
            _particle.Stop();

            _contactPS.transform.parent = PlayerManager.Instance.player.ModelTR;
            _contactPS.transform.localPosition = new Vector3(0, 1, -1.3f);
            _contactPS.gameObject.SetActive(true);

            yield return new WaitForSeconds(2f);
            _contactPS.transform.parent = null;
            _contactPS.gameObject.SetActive(false);
            _contactPS.transform.position = Vector3.zero;

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
            Vector3 scale1 = new Vector3(1.2f, 1.5f, 1);
            float timeScale1 = 0.75f;
            _modelGO.transform.localScale = Vector3.one;
            _modelGO.transform.DOScale(scale1, timeScale1).SetLoops(-1, LoopType.Yoyo);
        }
        public override void StopAnimation()
        {
            DOTween.Kill(_modelGO.transform);
            _modelGO.transform.localScale = Vector3.one;
        }

        protected override void PlaySound()
        {
            FMODUnity.RuntimeManager.PlayOneShot(_itemFMOD);
        }
    }

}
