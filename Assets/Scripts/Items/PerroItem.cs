using DG.Tweening;
using PolloScripts.DialogueSystem;
using PolloScripts.Enums;
using PolloScripts.Interfaces;
using PolloScripts.ObjectPoolSystem;
using PolloScripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Items
{

    public class PerroItem : CharacterItem
    {
        [Space]
        [Header("***** Perro data *****")]
        [Space]
        [SerializeField] private EnergyShield _shield;
        public int trayectoria = 0;
        public int timepoTrayectoria = 4;

        [Space]
        [Header("***** Weapon data *****")]
        [Space]
        public float duration;
        public float coolDown;
        public Sprite iconSprite;

        public WeaponType weaponType => WeaponType.Laser;
        public PlayerLaserType laserType => PlayerLaserType.LaserBeam;

        [Space]
        [Header("***** Message data *****")]
        [Space]
        public ConversationsData rescueMessage;
        public ConversationsData abandonmentMessage;

        private Sequence animationSeq;
        private bool animationStarted;
        //private void Start()
        //{

        //    StartAnimation();
        //}
        public void Init(int value, int scoreValue, int trayectoria, Transform parent, Vector3 position, Quaternion rotation)
        {
            _scoreValue = scoreValue;
            _itemValue = value;
            _canInteract = false;
            _modelGO.SetActive(true);
            animationStarted = false;
            this.trayectoria = trayectoria;

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
        public override void ShowModel(bool show)
        {
            base.ShowModel(show);
            if (show) StartAnimation();
        }
        public override void Deactivate()
        {
            if (animationSeq != null)
                animationSeq.Kill();
            if (!_canInteract)
                MessageBox.instance.StartMessage(abandonmentMessage);

            base.Deactivate();

           
        }
        public override void StartAnimation()
        {
            _modelHolderGO.transform.localRotation = Quaternion.Euler(Vector3.zero);

            Vector3 rotacion1 = new Vector3(20, 360, 45);

            if(!animationStarted)
            {
                animationStarted = true;
    
                animationSeq = DOTween.Sequence();

                animationSeq.Append(_modelHolderGO.transform.DOLocalRotate(rotacion1, 2, RotateMode.FastBeyond360).SetEase(Ease.Linear));
                animationSeq.SetLoops(-1, LoopType.Incremental);
            }
        }

        public override void StopAnimation()
        {
            animationSeq.Kill();
            Vector3 rotacion1 = new Vector3(0, 0, 0);
            animationSeq = DOTween.Sequence();

            animationSeq.Append(_modelHolderGO.transform.DOLocalRotate(rotacion1, 1));

        }

        protected override void ReturnToPool()
        {
            ObjectPoolPerroItem.Instance.ReturnToPool(this);
        }
        private void OnTriggerEnter(Collider other)
        {
            IPlayer player = other.GetComponentInParent<IPlayer>();
            if (player != null && !_canInteract)
            {
                PlayerManager.Instance.playerWeapons.AddWeapon((int)weaponType, (int)laserType, iconSprite, duration, coolDown,0);
                _canInteract = true;
                AddScore();
                StartCoroutine(IenDeactivate());
            }
        }

        private IEnumerator IenDeactivate()
        {
            StopAnimation();
            PlaySound();
            _particle.Stop();

            //Agradecimiento
            MessageBox.instance.StartMessage(rescueMessage);

            //Restablecer posicion antes de iniciar trayectoria
            transform.SetParent(PlayerManager.Instance.playerHolder.transform);

            Tween t1 = transform.DOLocalMoveZ(5, 1);

            yield return t1.WaitForCompletion();

            //Activar escudos
            _shield.Show();
            yield return new WaitForSeconds(2f);

            //Movmiento de salida
            Vector3[] localPath = new Vector3[2];

            if(trayectoria == 0)
            {
                localPath[0] = new Vector3(transform.localPosition.x, 5, 20);
                localPath[1] = new Vector3(transform.localPosition.x, 30, 30);
            }
            else if (trayectoria == 1)
            {
                localPath[0] = new Vector3(10, transform.localPosition.y, 20);
                localPath[1] = new Vector3(40, transform.localPosition.y, 30);
            }
            else
            {
                localPath[0] = new Vector3(-10, transform.localPosition.y, 20);
                localPath[1] = new Vector3(-40, transform.localPosition.y, 30);
            }

            Tween t2 = transform.DOLocalPath(localPath, timepoTrayectoria, PathType.CatmullRom, PathMode.Full3D).SetEase(Ease.InCubic);

            yield return t2.WaitForCompletion();

            _shield.Hide();

            Deactivate();
        }
        protected override void PlaySound()
        {
            FMODUnity.RuntimeManager.PlayOneShot(_itemFMOD);
        }
    }
}


