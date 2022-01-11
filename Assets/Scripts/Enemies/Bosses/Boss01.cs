using DG.Tweening;
using PolloScripts.DialogueSystem;
using PolloScripts.Effects;
using PolloScripts.Enums;
using PolloScripts.ObjectPoolSystem;
using PolloScripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PolloScripts.Enemies.Bosses
{
    [Serializable]
    public class BossModule{
        public GameObject moduleGO;
        public List<BossHealth> bossHealthList;
        public List<List<Vector3>> explosionPointsList;
    }

    public class Boss01 : MonoBehaviour
    {
        public int _scoreValue = 5000;
        public Transform holder;
        public Transform targetTR;
        [Header("Modules")]
        public Boss01Module1 module01;
        public Boss01Module2 module02;
        public Boss01Module2 module03;
        public Boss01Module4 module04;

        public int modueleIndex;
        public int dataIndex;
        //[SerializeField] private float _movementDuration = 1f;
        //Sequence movementSeq;

        private Sequence _movement;
        private Tween _movSecundario;
        private Coroutine _movementCor;


        [Header("Phase 1")]
        public AnimationCurve animaCurve1;
        public float time1 = 3;

        [Header("Animator")]
        public Animator animator;

        [Space]
        [Header("End Level")]
        public ConversationIndex index;
        public GameObject tunnel;
        [SerializeField] private float _waitToEnd = 2;
        public Transform explotionPosition;

        [Space]
        [Header("***** Testing *****")]
        [Space]
        public bool testing;


        private int _opendDoorHash = Animator.StringToHash("OpenDoor");
        private int _robotHash = Animator.StringToHash("Robot");

        private void Start() {
            
        }
        public void Activate()
        {

            targetTR.SetParent(transform.parent);
            targetTR.localPosition = Vector3.zero;
            IniciarMovSecundario();

            if (!testing)
            {
                StartCoroutine(IenLlegada());
            }
            else
            {
                ChangeLevel();
            }
        }

        public void ChangeLevel()
        {

            if(modueleIndex == -2)
            {
                module03.StartModule();
                module02.StartModule();
                module02.CanTakeDamage(false);
                print("****** Start Level 1 ******");
                transform.localPosition = new Vector3(-160, 0, 60);
                transform.localRotation = Quaternion.Euler(new Vector3(0, 110, 0));
                
                _movement = DOTween.Sequence();
                //Ataque derecho
                _movement.AppendCallback(() => module03.CanTakeDamage(true));
                _movement.Append(transform.DOLocalMove(new Vector3(-150, 0, 90), 0));
                _movement.Append(transform.DOLocalRotate(new Vector3(0, 110, 0), 0));
                _movement.Append(transform.DOLocalMove(new Vector3(-40, 0, 90), 1.5f).SetEase(Ease.OutSine));
                _movement.AppendCallback(OpenDoorAnima);
                _movement.AppendInterval(0.5f);
                _movement.AppendCallback(module03.Attack);
                _movement.AppendInterval(8);
                _movement.AppendCallback(module03.StopModule);
                _movement.AppendCallback(CloseDoorAnima);

                _movement.Append(transform.DOLocalMove(new Vector3(180, 0, 90), 1).SetEase(Ease.InSine));
                _movement.AppendCallback(() => module03.CanTakeDamage(false));

                _movement.AppendInterval(0.5f);

                //Ataque izquierdo
                _movement.AppendCallback(() => module02.CanTakeDamage(true));
                _movement.Append(transform.DOLocalMove(new Vector3(150, 0, 90), 0));
                _movement.Append(transform.DOLocalRotate(new Vector3(0, -110, 0), 0));
                _movement.Append(transform.DOLocalMove(new Vector3(40, 0, 90), 1.5f).SetEase(Ease.OutSine));
                _movement.AppendCallback(OpenDoorAnima);
                _movement.AppendInterval(0.5f);
                _movement.AppendCallback(module02.Attack);
                _movement.AppendInterval(8);
                _movement.AppendCallback(module02.StopModule);
                _movement.AppendCallback(CloseDoorAnima);
                _movement.Append(transform.DOLocalMove(new Vector3(-180, 0, 90), 1).SetEase(Ease.InSine));
                _movement.AppendCallback(() => module02.CanTakeDamage(false));

                _movement.AppendInterval(0.5f);
                _movement.SetLoops(-1, LoopType.Restart);
            }
            if(modueleIndex == 2)
            {
      

                // transform.localPosition = new Vector3(0, -140, 90);
                // transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));

                print("****** Start Level 1 ******");
                Sequence s1 = DOTween.Sequence();
                // s1.Append(transform.DOLocalMove(new Vector3(0,6,150),2));
                // s1.Append(transform.DOLocalMove(new Vector3(40,6,100),1).SetEase(Ease.OutSine));
                s1.Append(transform.DOLocalMove(new Vector3(-80,6,-30),1).SetEase(Ease.OutSine));

                s1.AppendCallback(MovModule2);
            }
            else if (modueleIndex == 3)
            {
               
            }
            else if (modueleIndex == 4)
            {
                Sequence seq = DOTween.Sequence();
                seq.AppendInterval(0.5f);
                seq.Append(transform.DOLocalMove(new Vector3(0, -140, 90), 2));
                seq.Append(transform.DOLocalRotate(new Vector3(0, 180, 0), 0));
                seq.AppendCallback(CloseDoorAnima);
                seq.Append(transform.DOLocalMove(new Vector3(0, -45, 120), 3).SetEase(Ease.OutSine));
                seq.AppendCallback(RobotAnima);
                seq.AppendCallback(module04.StartModule);
            }
            //StartCoroutine(IenChangeLevel());
        }

        private void MovModule2()
        {
            module03.StartModule();
            module02.StartModule();
            module02.CanTakeDamage(false);
            module03.CanTakeDamage(false);

            Vector3[] localPath = new Vector3[2];
            localPath[0] = new Vector3(0,-10,120);
            localPath[1] = new Vector3(80,6,80);

             Vector3[] localPath2 = new Vector3[2];
            localPath2[0] = new Vector3(0,6,120);
            localPath2[1] = new Vector3(40,6,100);

            _movement = DOTween.Sequence();
            _movement.Append(transform.DOLocalMove(new Vector3(-120, 6, -20), 0));
            _movement.Append(transform.DOLocalRotate(new Vector3(0, 50, 0), 0));
            _movement.Append(transform.DOLocalMove(new Vector3(-80, 6, 80), 1).SetEase(Ease.OutSine));
            //Ataque derecho
            _movement.AppendCallback(() => module03.CanTakeDamage(true));

            _movement.AppendCallback(OpenDoorAnima);
            _movement.AppendInterval(1f);
            _movement.AppendCallback(module03.Attack);
            _movement.AppendInterval(6);
            _movement.AppendCallback(module03.PauseModule);
            _movement.AppendInterval(1);
            _movement.AppendCallback(CloseDoorAnima);

            _movement.Append(transform.DOLocalMove(new Vector3(-120,6,-20), 1));
            _movement.Append(transform.DOLocalRotate(new Vector3(0, -50, 0), 0, RotateMode.FastBeyond360));
            _movement.Append(transform.DOLocalMove(new Vector3(120, 6, -20), 0));
            _movement.AppendCallback(() => module03.CanTakeDamage(false));

            _movement.Append(transform.DOLocalMove(new Vector3(80, 6, 80), 1).SetEase(Ease.OutSine));

            _movement.AppendCallback(() => module02.CanTakeDamage(true));



            _movement.AppendCallback(OpenDoorAnima);
            _movement.AppendInterval(1f);
            _movement.AppendCallback(module02.Attack);
            _movement.AppendInterval(6);
            _movement.AppendCallback(module02.PauseModule);
            _movement.AppendCallback(CloseDoorAnima);

            _movement.Append(transform.DOLocalMove(new Vector3(120, 6, -20), 1));
            _movement.Append(transform.DOLocalRotate(new Vector3(0, 50, 0), 0, RotateMode.FastBeyond360));
            _movement.Append(transform.DOLocalMove(new Vector3(-120, 6, -20), 0));
            _movement.AppendCallback(() => module02.CanTakeDamage(false));

            _movement.SetLoops(-1);

        }
        public void ModuloDestruido2()
        {
            modueleIndex++;
            _movement.Kill();
            module02.CanTakeDamage(false);

            if (modueleIndex < 4)
            {
                module02.PauseModule();
                Sequence seq = DOTween.Sequence();
                seq.AppendInterval(0.5f);
                _movement.Append(transform.DOLocalMove(new Vector3(120, 6, -20), 1.5f));
                seq.AppendCallback(MovimientoModulo3);
            }
            else
            {
                module02.StopModule();
                ChangeLevel();
            }
           
        }
        public void ModuloDestruido3()
        {
            modueleIndex++;
            _movement.Kill();
            module03.CanTakeDamage(false);
  

            if (modueleIndex < 4)
            {
                module03.PauseModule();
                Sequence seq = DOTween.Sequence();
                seq.AppendInterval(0.5f);
                _movement.Append(transform.DOLocalMove(new Vector3(-120, 6, -20), 1.5f));
                seq.AppendCallback(MovimientoModulo2);
            }
            else
            {
                module03.StopModule();
                ChangeLevel();
            }
               
        }
        private void IniciarMovSecundario()
        {
            if(_movSecundario != null)
            {
                DetenerMovSecundario();
            }

            _movSecundario = holder.DOLocalMoveY(holder.localPosition.y + 2f, 2).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }

        private void DetenerMovSecundario()
        {
            _movSecundario.Kill();
        }

        private void MovimientoModulo2()
        {
            _movement = DOTween.Sequence();

            //Ataque derecho
            _movement.AppendInterval(1);
            _movement.AppendCallback(() => module02.CanTakeDamage(true));
            _movement.Append(transform.DOLocalMove(new Vector3(120, 6, -20), 0));
            _movement.Append(transform.DOLocalRotate(new Vector3(0, -50, 0), 0));
            _movement.Append(transform.DOLocalMove(new Vector3(80, 6, 80), 1).SetEase(Ease.OutSine));
         

            _movement.AppendCallback(OpenDoorAnima);
            _movement.AppendInterval(1f);
            _movement.AppendCallback(module02.Attack);

        }
        private void MovimientoModulo3()
        {
            _movement = DOTween.Sequence();
            //Ataque derecho
            _movement.AppendInterval(1);
            _movement.AppendCallback(() => module03.CanTakeDamage(true));
            _movement.Append(transform.DOLocalMove(new Vector3(-120, 6, -20), 0));
            _movement.Append(transform.DOLocalRotate(new Vector3(0, 50, 0), 0));
            _movement.Append(transform.DOLocalMove(new Vector3(-80, 6, 80), 1).SetEase(Ease.OutSine));
  

            _movement.AppendCallback(OpenDoorAnima);
            _movement.AppendInterval(1f);
            _movement.AppendCallback(module03.Attack);

        }

        private void MovimientoModulo4()
        {

        }

        private void OpenDoorAnima()
        {
            animator.SetBool(_opendDoorHash, true);
        }
        private void CloseDoorAnima()
        {
            animator.SetBool(_opendDoorHash, false);
        }
        private void RobotAnima()
        {
            animator.SetBool(_robotHash,true);
        }

        public void Death()
        {
            CanvasManager.Instance.OnAddScore?.Invoke(_scoreValue);
            CanvasManager.Instance.OnSaveScore.Invoke();
            StartCoroutine(IenDeath());
        }
        IEnumerator IenLlegada()
        {
            // transform.localPosition = new Vector3(160, 0, 50);
            // transform.localRotation = Quaternion.Euler(new Vector3(0, -40, 0));

            transform.localPosition = new Vector3(0, 150, 50);
            transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
            Tween t1 = transform.DOLocalMove(new Vector3(0, -6, 150), 2);

            // Tween t1 = transform.DOLocalMove(new Vector3(-300, 0, 300), 4);
            yield return t1.WaitForCompletion();

            //transform.localPosition = new Vector3(0, -140, 120);
            //transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));

            //Tween t2 = transform.DOLocalMove(new Vector3(0, 0, 120), 2);

            //yield return t2.WaitForCompletion();

   
            ChangeLevel();
   

        }

        IEnumerator IenModule2(){
            yield return null;

        }

        IEnumerator IenChangeLevel2()
        {
            float positionTime = 3f;
            float rotationTime = 2f;
            if (modueleIndex == 1)
            {
                transform.position = new Vector3(-150, -10, 0);
                Tween tmove = transform.DOMove(new Vector3(0, -10, 70), positionTime);
                transform.DORotate(new Vector3(0, 0, 0), rotationTime);

                yield return tmove.WaitForCompletion();

                print("****** Start Level 1 ******");
                //module01.StartModule();
            } 
            else if (modueleIndex == 2)
            {
                Tween tmove = transform.DOMove(new Vector3(0, -3, 70), positionTime);
                transform.DORotate(new Vector3(0, -65, 0), rotationTime);

                yield return tmove.WaitForCompletion();

                print("****** Start Level 2 ******");
                module02.StartModule();
            }
            else if (modueleIndex == 3)
            {
                Tween tmove1 = transform.DOMove(new Vector3(120, -3, 70), positionTime);
                transform.DORotate(new Vector3(0, 10, 0), rotationTime);

                yield return tmove1.WaitForCompletion();

                transform.position = new Vector3(-150, -3, 70);
                transform.rotation = Quaternion.Euler(0, 0, 0);

                yield return null;

                Tween tmove2 = transform.DOMove(new Vector3(0, -3, 70), positionTime);
                transform.DORotate(new Vector3(0, 65, 0), rotationTime);

                yield return tmove2.WaitForCompletion();

                print("****** Start Level 3 ******");
                module03.StartModule();
            }
            else if (modueleIndex == 4)
            {

                Tween tmove1 = transform.DOMove(new Vector3(-120, -3, 70), positionTime);
                transform.DORotate(new Vector3(0, -10, 0), rotationTime);

                yield return tmove1.WaitForCompletion();

                transform.position = new Vector3(0, -120, -80);
                transform.rotation = Quaternion.Euler(0, 180, 0);

                yield return null;

                Tween tmove2 = transform.DOMove(new Vector3(0, -30, 30), positionTime);
   
                yield return tmove2.WaitForCompletion();

                print("****** Start Level 4 ******");
                module04.StartModule();
            }
        }

        IEnumerator IenDeath()
        {
            ExplosionBase explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position, Quaternion.identity);
            explosion.ReSize(50);
            explosion.Activate();

            yield return new WaitForSeconds(0.2f);

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + transform.right * 40, Quaternion.identity);
            explosion.ReSize(50);
            explosion.Activate();

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + transform.right * 60, Quaternion.identity);
            explosion.ReSize(50);
            explosion.Activate();

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + (-transform.right * 40), Quaternion.identity);
            explosion.ReSize(50);
            explosion.Activate();

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + (-transform.right * 60), Quaternion.identity);
            explosion.ReSize(50);
            explosion.Activate();

            yield return new WaitForSeconds(0.2f);

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + (transform.up * 30), Quaternion.identity);
            explosion.ReSize(50);
            explosion.Activate();

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + (-transform.up * 30), Quaternion.identity);
            explosion.ReSize(50);
            explosion.Activate();

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + (transform.up * 40), Quaternion.identity);
            explosion.ReSize(50);
            explosion.Activate();

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + (-transform.up * 40), Quaternion.identity);
            explosion.ReSize(50);
            explosion.Activate();

            //Segunda ronda

            yield return new WaitForSeconds(0.1f);

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + explotionPosition.forward * 70, Quaternion.identity);
            explosion.ReSize(30);
            explosion.Activate();

            yield return new WaitForSeconds(0.1f);

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + explotionPosition.forward * 70 + explotionPosition.right * 40, Quaternion.identity);
            explosion.ReSize(30);
            explosion.Activate();

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + explotionPosition.forward * 70 + (-explotionPosition.right * 40), Quaternion.identity);
            explosion.ReSize(30);
            explosion.Activate();

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + explotionPosition.forward * 70 + explotionPosition.right * 60, Quaternion.identity);
            explosion.ReSize(30);
            explosion.Activate();

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + explotionPosition.forward * 70 + (-explotionPosition.right * 60), Quaternion.identity);
            explosion.ReSize(30);
            explosion.Activate();

            yield return new WaitForSeconds(0.2f);

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + explotionPosition.forward * 70 + (explotionPosition.up * 30), Quaternion.identity);
            explosion.ReSize(30);
            explosion.Activate();

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + explotionPosition.forward * 70 + (-explotionPosition.up * 30), Quaternion.identity);
            explosion.ReSize(30);
            explosion.Activate();

            //TERCERA
            yield return new WaitForSeconds(0.1f);

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + explotionPosition.forward * 80, Quaternion.identity);
            explosion.ReSize(30);
            explosion.Activate();

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + explotionPosition.forward * 80 + (explotionPosition.right * 30), Quaternion.identity);
            explosion.ReSize(30);
            explosion.Activate();

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + explotionPosition.forward * 80 + (-transform.right * 30), Quaternion.identity);
            explosion.ReSize(30);
            explosion.Activate();

            yield return new WaitForSeconds(0.1f);

            //CUARTA
            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position, Quaternion.identity);
            explosion.ReSize(50);
            explosion.Activate();

            yield return new WaitForSeconds(0.1f);

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + transform.right * 40, Quaternion.identity);
            explosion.ReSize(50);
            explosion.Activate();

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + transform.right * 60, Quaternion.identity);
            explosion.ReSize(50);
            explosion.Activate();

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + (-transform.right * 40), Quaternion.identity);
            explosion.ReSize(50);
            explosion.Activate();

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + (-transform.right * 60), Quaternion.identity);
            explosion.ReSize(50);
            explosion.Activate();

            yield return new WaitForSeconds(0.1f);

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + (transform.up * 30), Quaternion.identity);
            explosion.ReSize(50);
            explosion.Activate();

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + (-transform.up * 30), Quaternion.identity);
            explosion.ReSize(50);
            explosion.Activate();

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + (transform.up * 40), Quaternion.identity);
            explosion.ReSize(50);
            explosion.Activate();

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, explotionPosition.position + (-transform.up * 40), Quaternion.identity);
            explosion.ReSize(50);
            explosion.Activate();

            //ShowModel(false);
            holder.gameObject.SetActive(false);
            
            yield return new WaitForSeconds(2f);

            PlayerManager.Instance.player.Deactivate();

            StartCoroutine(IenEndLevel());
        }

        IEnumerator IenEndLevel()
        {
            DialogueManager dia = DialogueManager.instance;

            dia.StartDialogue(index);

            while (dia.InDialogue) { yield return null; }

            tunnel.SetActive(true);
            yield return new WaitForSeconds(_waitToEnd);

            GameManager.Instance.Death();

            //gameObject.SetActive(false);
        }
    }

}
