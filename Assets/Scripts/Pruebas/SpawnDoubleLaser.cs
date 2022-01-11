using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDoubleLaser : MonoBehaviour
{

    public GameObject laserPrefab;
    //public Transform lugarDisparo;
    public bool activate;
    public float esperaAtaque = 1;
    public float rapidezUpDown = 6f;

    private DoubleBossLaser _laser;

    public void Fire()
    {
        _laser = Instantiate(laserPrefab, transform.position, transform.rotation).GetComponent<DoubleBossLaser>();

        _laser.UpdateServer(false);

        Sequence seq = DOTween.Sequence();

        _laser.transform.rotation = Quaternion.Euler(0, 180, 0);
        seq.AppendInterval(esperaAtaque);
        seq.Append(_laser.transform.DORotate(new Vector3(16.5f, 180, 0), rapidezUpDown));
        seq.Append(_laser.transform.DORotate(new Vector3(16.5f, 177.5f, 0), rapidezUpDown));
        seq.Append(_laser.transform.DORotate(new Vector3(13.7f, 177.5f, 0), rapidezUpDown));
        seq.Append(_laser.transform.DORotate(new Vector3(13.3f, 182.5f, 0), rapidezUpDown));
        seq.Append(_laser.transform.DORotate(new Vector3(16.5f, 182.5f, 0), rapidezUpDown));
        seq.Append(_laser.transform.DORotate(new Vector3(13.7f, 177.5f, 0), rapidezUpDown));
        seq.Append(_laser.transform.DORotate(new Vector3(-30, 190, 0), rapidezUpDown));
    }
}
