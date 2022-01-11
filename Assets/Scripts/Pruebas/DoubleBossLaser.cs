using PolloScripts.WeaponSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBossLaser : MonoBehaviour
{
    public BossLaser laserR;
    public BossLaser laserL;



    public void UpdateServer(bool option)
    {
        //laserL.UpdateSaver = option;
        //laserR.UpdateSaver = option;
    }

    public void DisablePrepare()
    {
        laserL.DisablePrepare();
        laserR.DisablePrepare();
    }
}
