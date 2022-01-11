using DG.Tweening;
using PolloScripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Enemies.Patterns
{
    public class Pattern_M02 : IPatternMovement
    {
        public Pattern_M02(int index)
        {
            _index = index;
        }
        private int _index;
        public Sequence Movement(Transform target, Action OnShoot, Action OnDeactivate, Action OnLookAt)
        {
            Sequence sMovement = DOTween.Sequence();

            Vector3 position1 = Vector3.zero;
            Vector3 position2 = Vector3.zero;
            Vector3 position3 = Vector3.zero;
            Vector3 position4 = Vector3.zero;
            Vector3 rotation1 = Vector3.zero;
            float waitToStart = 0f;
            float timeToMove1 = 4f;
            float timeToMove2 = 1f;
            float timeToMove3 = 1f;
            float rotationDuration = 0.2f;
            Vector3[] localPath = null;

            //DOTweenPath path = new DOTweenPath();

            float posZ = 150f;


            if (_index == 1)
            {
                localPath = new Vector3[2];
                localPath[0] = new Vector3(20, 30, 80);
                localPath[1] = new Vector3(-70, 40, posZ);
                waitToStart = 0;
                position1 = new Vector3(30, 30, 5);
                position2 = new Vector3(-40, 30, 0);
                rotation1 = new Vector3(0, -180, 0);
            }
            else if (_index == 2)
            {
                localPath = new Vector3[2];
                localPath[0] = new Vector3(-20, 30, 80);
                localPath[1] = new Vector3(70, 40, posZ);
                waitToStart = 1;
                position1 = new Vector3(-30, 30, 5);
                position2 = new Vector3(40, 30, 0);
                rotation1 = new Vector3(0, 180, 0);
            }
            else if (_index == 3)
            {
                localPath = new Vector3[2];
                localPath[0] = new Vector3(20, -30, 80);
                localPath[1] = new Vector3(-70, -40, posZ);
                waitToStart = 2;
                position1 = new Vector3(30, -30, 5);
                position2 = new Vector3(-40, -30, 0);
                rotation1 = new Vector3(0, -180, 0);
            }
            else if (_index == 4)
            {
                localPath = new Vector3[2];
                localPath[0] = new Vector3(-20, -30, 80);
                localPath[1] = new Vector3(70, -40, posZ);
                waitToStart = 3;
                position1 = new Vector3(-30, -30, 5);
                position2 = new Vector3(40, -30, 0);
                rotation1 = new Vector3(0, 180, 0);
            }
            if (_index == 5)
            {
                localPath = new Vector3[2];
                localPath[0] = new Vector3(20, 20, 80);
                localPath[1] = new Vector3(-70, 0, posZ);
                waitToStart = 4;
                position1 = new Vector3(30, 30, 5);
                position2 = new Vector3(-40, 0, 0);
                rotation1 = new Vector3(0, -180, 0);
            }
            else if (_index == 6)
            {
                localPath = new Vector3[2];
                localPath[0] = new Vector3(-20, -30, 80);
                localPath[1] = new Vector3(70, 0, posZ);
                waitToStart = 5;
                position1 = new Vector3(-30, -30, 5);
                position2 = new Vector3(40, 0, 0);
                rotation1 = new Vector3(0, 180, 0);
            }
            else if (_index == 7)
            {
                localPath = new Vector3[2];
                localPath[0] = new Vector3(20, 30, 80);
                localPath[1] = new Vector3(0, 50, posZ);
                waitToStart = 6;
                position1 = new Vector3(30, 30, 5);
                position2 = new Vector3(0, 30, 0);
                rotation1 = new Vector3(0, -180, 0);
            }
            else if (_index == 8)
            {
                localPath = new Vector3[2];
                localPath[0] = new Vector3(-20, -30, 80);
                localPath[1] = new Vector3(0, -50, posZ);
                waitToStart = 7;
                position1 = new Vector3(-30, -30, 5);
                position2 = new Vector3(0, -30, 0);
                rotation1 = new Vector3(0, 180, 0);
            }
            // Initial Position
            target.localPosition = position1;
            target.localRotation = Quaternion.Euler(0, 0, 0);

            //Sequence .SetLookAt(0.01f)
            sMovement.AppendInterval(waitToStart);
            sMovement.Append(target.DOLocalPath(localPath, timeToMove1, PathType.CatmullRom, PathMode.Full3D).SetEase(Ease.OutQuad));
            sMovement.Append(target.DOLocalRotate(rotation1, rotationDuration));
            sMovement.AppendInterval(1);
            sMovement.AppendCallback(OnShoot.Invoke);
            sMovement.AppendInterval(2);
            sMovement.Append(target.DOLocalMove(position2, timeToMove2).SetEase(Ease.InSine));
            sMovement.AppendCallback(OnDeactivate.Invoke);
            return sMovement;
        }


    }

}
