using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RingController : MonoBehaviour
{
    public Transform Model;

    private void Awake()
    {
        Model.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void Move(Vector3 pos)
    {
        StartCoroutine(IenMove(pos));
    }

    IEnumerator IenMove(Vector3 pos)
    {
        Sequence sec = DOTween.Sequence();

        sec.Append(transform.DOLocalMove(pos, 1f));
        sec.Insert(0.5f,Model.DOScale(Vector3.one * 2,0.5f));
        sec.Insert(0.8f,transform.DOLocalMoveZ(-150, 3));
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
        //transform.parent = null;
    }

}
