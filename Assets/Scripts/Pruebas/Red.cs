using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red : MonoBehaviour
{
    public GameObject laser1;
    public GameObject laser2;
    public GameObject laser3;
    //public float speed = 3f;
    public float positionZ = -5f;
    private void Awake()
    {
        Activate(false);
    }

    public void Activate(bool option)
    {
        laser1.SetActive(option);
        laser2.SetActive(option);
        laser3.SetActive(option);
    }

    private void Activate2()
    {
        laser1.SetActive(true);
        laser2.SetActive(true);
        laser3.SetActive(true);
    }

    public void MoveTo(Vector3 target, float speed)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(target, 1));
        seq.AppendCallback(Activate2);
        seq.Append(transform.DOMoveZ(positionZ, speed));
        seq.AppendCallback(Death);
    }
    
    void Death()
    {
        Destroy(gameObject);
    }
}
