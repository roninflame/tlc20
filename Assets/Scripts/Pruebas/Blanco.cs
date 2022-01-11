using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PolloScripts;
using PolloScripts.DialogueSystem;
using PolloScripts.UI;
using PolloScripts.WeaponSystem;
using PolloScripts.Enums;

public class Blanco : MonoBehaviour
{
    public AnimationCurve curvaMov;
    public Transform model;
    public BossFireLaser laser;
    public Transform laserParent;

    public ConversationsData dialogo1;

    public ConversationsData msj1;

    public ConversationsData msj2;

    private Sequence _sq;

    private bool mirarPlayer;

    void Update()
    {
        if (mirarPlayer)
        {
            model.LookAt(PlayerManager.Instance.player.hitTransform);
        }
    }

    public void Inicio()
    {
        laser.gameObject.SetActive(false);
        transform.position = new Vector3(-30, 0, -5);
        StartCoroutine(PrimerSecuencia());
    }

    void Movimiento1()
    {
        float tmov = 0.5f;
        Ease emov = Ease.OutBounce;

        _sq = DOTween.Sequence();
        _sq.Append(transform.DOMove(new Vector3(25, 0, 50),tmov).SetEase(curvaMov));
        _sq.Append(transform.DOMove(new Vector3(-25, 20, 50), tmov).SetEase(curvaMov));
        _sq.Append(transform.DOMove(new Vector3(-25, -20, 50), tmov).SetEase(curvaMov));
        _sq.Append(transform.DOMove(new Vector3(25, 20, 50), tmov).SetEase(curvaMov));
        _sq.Append(transform.DOMove(new Vector3(0, -20, 50), tmov).SetEase(curvaMov));
        _sq.Append(transform.DOMove(new Vector3(-25, 20, 50), tmov).SetEase(curvaMov));
        _sq.Append(transform.DOMove(new Vector3(25, -20, 50), tmov).SetEase(curvaMov));
        _sq.Append(transform.DOMove(new Vector3(0, 5, 50), tmov).SetEase(curvaMov));
        _sq.SetLoops(-1);
    }

    IEnumerator PrimerSecuencia()
    {

        PlayerManager.Instance.playerController.EnableMenuControls();

        PlayerManager.Instance.player.transform.DOLocalMove(Vector3.zero, 2);

        CanvasManager.Instance.SetActiveUI(false);

        Tween t1 = transform.DOMove(new Vector3(0, 5, 50), 2f).SetEase(Ease.OutSine);
       
        yield return t1.WaitForCompletion();

        model.DORotate(new Vector3(0, 180, 0), 1);
        //dialogo inicial
        DialogueManager dia = DialogueManager.instance;

        dia.StartDialogue(dialogo1);

        while (dia.InDialogue) { yield return null; }

        //movimiento del enemigo

        PlayerManager.Instance.playerController.EnableGameplayControls();

        Movimiento1();

        ActivarLaser();

        //mensajes
        MessageBox.instance.StartMessage(msj1);

        yield return new WaitForSeconds(6f);

        MessageBox.instance.StartMessage(msj2);

        yield return new WaitForSeconds(6f);

        _sq.Kill();
        
        //dialogo final

        PlayerManager.Instance.player.transform.DOLocalMove(Vector3.zero, 2);

        Tween t2 = transform.DOMove(new Vector3(0, 5, 50), 1f);

        dia.StartDialogue(dialogo1);

        while (dia.InDialogue) { yield return null; }

        laser.SetMaxLenght(100);
        yield return new WaitForSeconds(1f);

        //disparo

        laser.Shoot();

        yield return new WaitForSeconds(1);

        //mostrar pantalla negra


    }

    private void ActivarLaser()
    {
        laser.gameObject.SetActive(true);
        laser.SetLaser(TargetType.Player, laserParent, Vector3.zero, Vector3.zero);
        laser.SetDamage(1);
        laser.Activate();
        laser.transform.localScale = Vector3.one * 0.01f;
        laser.transform.DOScale(0.5f, 20);

    }

}
