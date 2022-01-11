using DG.Tweening;
using PolloScripts.WeaponSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLaser : MonoBehaviour
{
    public GameObject laserPrefab;
    //public Transform lugarDisparo;
    public bool activate;
    public float powerLaser = 1;
    public float rapidezUpDown = 6f;

    private BossLaser _laser;
    void Start()
    {
       

        //if (activate)
        //    Fire();
    }

    public void Fire()
    {
        _laser = Instantiate(laserPrefab, transform.position, transform.rotation).GetComponent<BossLaser>();

        //_laser.UpdateSaver = true;
        //_laser.laserScale = 10;

        //StartCoroutine(IenFire());
        Sequence seq = DOTween.Sequence();
        _laser.transform.rotation = Quaternion.Euler(-30, 180, 0);
        seq.Append(_laser.transform.DORotate(new Vector3(30, 180,0), rapidezUpDown));
        seq.AppendInterval(2f);
        seq.Append(_laser.transform.DORotate(new Vector3(-30, 180, 0), rapidezUpDown));
        //seq.AppendCallback(Stop);

        //_laser.transform.DORotate(new Vector3(9.586f, 158.988f, -6.229f), rapidezUpDown);
        //_laser.transform.DORotate(new Vector3(3.41f, 159.257f, -3.86f), rapidezUpDown);
        //_laser.transform.DORotate(new Vector3(3.6f, 159, 0), rapidezUpDown);
    }
    private void Stop()
    {
        _laser.DisablePrepare();
    }
    //IEnumerator IenFire()
    //{
    //   // yield return new WaitForSeconds(1f);
    //    _laser.UpdateSaver = false;
    //    while(_laser.laserScale < 50)
    //    {
    //        _laser.laserScale += 0.01f * powerLaser;
    //        _laser.laserMat.SetFloat("_Scale", _laser.laserScale);
    //        yield return new WaitForSeconds(0.01f);
    //    }
    //    yield return null;
        
    //}

}
