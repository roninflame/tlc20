using DG.Tweening;
using PolloScripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Enemies.Patterns
{
    
    public class Pattern_M05 : IPatternMovement
    {
        public Pattern_M05(int index)
        {
            _index = index;
        }
        private int _index;
        public Sequence Movement(Transform target, Action OnShoot, Action OnDeactivate, Action OnLookAt)
        {
            Sequence sMovement = DOTween.Sequence();

            Vector3 position1 = Vector3.zero;

            float waitToStart = 0f;
            float timeToMove1 = 7;


            Vector3[] localPath = null;

            position1 = new Vector3(40, -10, 0);
            localPath = new Vector3[6];
            localPath[0] = new Vector3(-20, -6, 60);
            localPath[1] = new Vector3(-40, -3, 140);
            localPath[2] = new Vector3(0, 0, 175);
            localPath[3] = new Vector3(56, 3, 155);
            localPath[4] = new Vector3(57, 6, 70);
            localPath[5] = new Vector3(5, 12, -5);

            if (_index == 1)
            {
                waitToStart = 0;
            }
            else if (_index == 2)
            {
                waitToStart = 1;
            }
            else if (_index == 3)
            {
                waitToStart = 2;
            }
            else if (_index == 4)
            {
                waitToStart = 3;
            }
            if (_index == 5)
            {
                waitToStart = 4;
            }
            else if (_index == 6)
            {
                waitToStart = 5;
            }
            else if (_index == 7)
            {
                waitToStart = 6;
            }
            else if (_index == 8)
            {
                waitToStart = 7;
            }

            // Initial Position
            target.localPosition = position1;
            target.localRotation = Quaternion.Euler(Vector3.zero);

            //Sequence
            sMovement.AppendInterval(waitToStart);
           
            sMovement.Append(target.DOLocalPath(localPath, timeToMove1, PathType.CatmullRom, PathMode.Full3D).SetLookAt(0.01f).SetEase(Ease.InSine));
            sMovement.InsertCallback(waitToStart + timeToMove1 / 2.5f, OnLookAt.Invoke);
            sMovement.InsertCallback(waitToStart + timeToMove1 / 2f, OnShoot.Invoke);
            sMovement.AppendCallback(OnDeactivate.Invoke);
            return sMovement;
        }
    }

}
