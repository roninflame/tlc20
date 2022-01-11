using DG.Tweening;
using PolloScripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Enemies.Patterns
{
    
    public class Pattern_M07 : IPatternMovement
    {
        public Pattern_M07(int index)
        {
            _index = index;
        }
        private int _index;
        public Sequence Movement(Transform target, Action OnShoot, Action OnDeactivate, Action OnLookAt)
        {
            Sequence sMovement = DOTween.Sequence();

            Vector3 position1 = Vector3.zero;

            float waitToStart = 0f;
            float timeToMove1 = 6;


            Vector3[] localPath = null;

            position1 = new Vector3(-40, 10, 0);
            localPath = new Vector3[6];
            localPath[0] = new Vector3(20, 6, 60);
            localPath[1] = new Vector3(26, 3, 94);
            localPath[2] = new Vector3(-2, 0, 115);
            localPath[3] = new Vector3(-47, -3, 107);
            localPath[4] = new Vector3(-57, -6, 70);
            localPath[5] = new Vector3(-5, -12, -5);

            if (_index == 1)
            {
                position1 = new Vector3(0, 120, 200);
                localPath = new Vector3[4];
                localPath[0] = new Vector3(0, 60, 190);
                localPath[1] = new Vector3(0, 20, 150);
                localPath[2] = new Vector3(0, 5, 100);
                localPath[3] = new Vector3(0, 0, 0);
                waitToStart = 0;
           
            }
            else if (_index == 2)
            {
                position1 = new Vector3(100, 120, 200);
                localPath = new Vector3[4];
                localPath[0] = new Vector3(80, 51, 185);
                localPath[1] = new Vector3(47, 14, 147);
                localPath[2] = new Vector3(17, -2, 87);
                localPath[3] = new Vector3(-20, -15, 0);
                waitToStart = 1;
            }
            else if (_index == 3)
            {
                position1 = new Vector3(-100, 120, 200);
                localPath = new Vector3[4];
                localPath[0] = new Vector3(-80, 51, 185);
                localPath[1] = new Vector3(-47, 14, 147);
                localPath[2] = new Vector3(-17, 5, 87);
                localPath[3] = new Vector3(20, 15, 0);
                waitToStart = 2;
            }
            else if (_index == 4)
            {
                position1 = new Vector3(140, 10, 150);
                localPath = new Vector3[4];
                localPath[0] = new Vector3(60, 10, 140);
                localPath[1] = new Vector3(15, 10, 108);
                localPath[2] = new Vector3(-11, 10, 50);
                localPath[3] = new Vector3(-83, 20, 0);
                waitToStart = 3;
            }
            if (_index == 5)
            {
                position1 = new Vector3(-140, -10, 150);
                localPath = new Vector3[4];
                localPath[0] = new Vector3(-60, -10, 140);
                localPath[1] = new Vector3(-15, -10, 108);
                localPath[2] = new Vector3(11, -10, 50);
                localPath[3] = new Vector3(83, -20, 0);
                waitToStart = 4;
            }
            else if (_index == 6)
            {
                position1 = new Vector3(140, 60, 150);
                localPath = new Vector3[4];
                localPath[0] = new Vector3(71, 0, 140);
                localPath[1] = new Vector3(15, 40, 108);
                localPath[2] = new Vector3(-36, 0, 50);
                localPath[3] = new Vector3(-83, 30, 0);
                waitToStart = 5;
            }
            else if (_index == 7)
            {
                position1 = new Vector3(-140, -60, 150);
                localPath = new Vector3[4];
                localPath[0] = new Vector3(-71, 0, 140);
                localPath[1] = new Vector3(-15, -40, 108);
                localPath[2] = new Vector3(36, 0, 50);
                localPath[3] = new Vector3(83, -30, 0);
                waitToStart = 6;
            }
            else if (_index == 8)
            {
                position1 = new Vector3(0, 120, 200);
                localPath = new Vector3[5];
                localPath[0] = new Vector3(-5, 100, 190);
                localPath[1] = new Vector3(-13, 65, 170);
                localPath[2] = new Vector3(13, 38, 148);
                localPath[3] = new Vector3(-16, 7, 100);
                localPath[4] = new Vector3(0, 0, -4);
                waitToStart = 7;
            }

            // Initial Position
            target.localPosition = position1;
            target.localRotation = Quaternion.Euler(Vector3.zero);

            //Sequence
            sMovement.AppendInterval(waitToStart);

            sMovement.Append(target.DOLocalPath(localPath, timeToMove1, PathType.CatmullRom, PathMode.Full3D).SetLookAt(0.01f).SetEase(Ease.InSine));
            sMovement.InsertCallback(waitToStart + timeToMove1 -1.5f, OnShoot.Invoke);
            sMovement.AppendCallback(OnDeactivate.Invoke);
            return sMovement;
        }
    }

}
