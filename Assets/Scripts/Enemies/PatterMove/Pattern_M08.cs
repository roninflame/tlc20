using DG.Tweening;
using PolloScripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Enemies.Patterns
{
    public class Pattern_M08 : IPatternMovement
    {
        private int _index;
        public Pattern_M08(int index)
        {
            _index = index;
        }
        public Sequence Movement(Transform target, Action OnShoot, Action OnDeactivate, Action OnLookAt)
        {
            Sequence sMovement = DOTween.Sequence();

            Vector3 position1 = Vector3.zero;
            Vector3 position2 = Vector3.zero;
            float waitToStart = 0f;
            float timeToMove1 = 3;
            float timeToMove2 = 2;

            Vector3[] localPath = null;

            if (_index == 1)
            {
                position1 = new Vector3(70, -20, 0);
                position2 = new Vector3(-50, -40, 0);
                localPath = new Vector3[3];
                localPath[0] = new Vector3(65, -20, 50);
                localPath[1] = new Vector3(20, -7, 90);
                localPath[2] = new Vector3(-50, 5, 100);
                waitToStart = 0;

            }
            else
            if (_index == 2)
            {
                position1 = new Vector3(-70, -20, 0);
                position2 = new Vector3(50, -40, 0);
                localPath = new Vector3[3];
                localPath[0] = new Vector3(-65, -20, 50);
                localPath[1] = new Vector3(-20, -7, 90);
                localPath[2] = new Vector3(50, 5, 100);
                waitToStart = 1;

            }
            else if (_index == 3)
            {
                position1 = new Vector3(0, -40, 0);
                position2 = new Vector3(0, 40, 0);
                localPath = new Vector3[3];
                localPath[0] = new Vector3(0, -30, 55);
                localPath[1] = new Vector3(0, 12, 90);
                localPath[2] = new Vector3(0, 30, 100);
                waitToStart = 2;

            }
            else if (_index == 4)
            {
                position1 = new Vector3(70, -20, 0);
                position2 = new Vector3(-50, -40, 0);
                localPath = new Vector3[3];
                localPath[0] = new Vector3(65, -20, 50);
                localPath[1] = new Vector3(20, -7, 90);
                localPath[2] = new Vector3(-30, -30, 100);
                waitToStart = 3;

            }
            else
            {
                position1 = new Vector3(-70, -20, 0);
                position2 = new Vector3(50, -40, 0);
                localPath = new Vector3[3];
                localPath[0] = new Vector3(-65, -20, 50);
                localPath[1] = new Vector3(-20, -7, 90);
                localPath[2] = new Vector3(30, -30, 100);
                waitToStart = 4;

            }
            // Initial Position
            target.localPosition = position1;
            target.localRotation = Quaternion.Euler(Vector3.zero);

            //Sequence
            sMovement.AppendInterval(waitToStart);
            sMovement.Append(target.DOLocalPath(localPath, timeToMove1, PathType.CatmullRom, PathMode.Full3D).SetEase(Ease.InSine));
            sMovement.AppendInterval(1.5f);
            sMovement.AppendCallback(OnShoot.Invoke);
            sMovement.AppendInterval(2);
            sMovement.Append(target.DOLocalMove(position2, timeToMove2));
            sMovement.AppendCallback(OnDeactivate.Invoke);
            return sMovement;

        }
    }

}

