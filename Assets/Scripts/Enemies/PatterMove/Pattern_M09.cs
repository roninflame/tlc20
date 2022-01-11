using DG.Tweening;
using PolloScripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Enemies.Patterns
{
    public class Pattern_M09 : IPatternMovement
    {
        public Pattern_M09(int index)
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
            float timeToMove1 = 20;
            float timeToMove2 = 1f;
            float timeToMove3 = 1f;
            float rotationDuration = 0.2f;
            Vector3[] localPath = null;

            //DOTweenPath path = new DOTweenPath();

            float posZ = 110f;


            if (_index == 1)
            {
                waitToStart = 0;
                position1 = new Vector3(60, -70, posZ);
                position2 = new Vector3(60, 70, posZ);
                rotation1 = new Vector3(0, 180, 0);
            }
            else if (_index == 2)
            {
                waitToStart = 1.5f;
                position1 = new Vector3(40, -70, posZ);
                position2 = new Vector3(40, 70, posZ);

                rotation1 = new Vector3(0, 180, 0);
            }
            else if (_index == 3)
            {
                waitToStart = 3;
                position1 = new Vector3(60, -70, posZ);
                position2 = new Vector3(60, 70, posZ);
                rotation1 = new Vector3(0, 180, 0);
            }
            else if (_index == 4)
            {
                waitToStart = 4.5f;
                position1 = new Vector3(40, -70, posZ);
                position2 = new Vector3(40, 70, posZ);
                rotation1 = new Vector3(0, 180, 0);
            }
            if (_index == 5)
            {
                waitToStart = 6;
                position1 = new Vector3(-60, -70, posZ);
                position2 = new Vector3(-60, 70, posZ);
                rotation1 = new Vector3(0, 180, 0);
            }
            else if (_index == 6)
            {
                waitToStart = 7.5f;
                position1 = new Vector3(-40, -70, posZ);
                position2 = new Vector3(-40, 70, posZ);

                rotation1 = new Vector3(0, 180, 0);
            }
            else if (_index == 7)
            {
                waitToStart = 9;
                position1 = new Vector3(-60, -70, posZ);
                position2 = new Vector3(-60, 70, posZ);
                rotation1 = new Vector3(0, 180, 0);
            }
            else if (_index == 8)
            {
                waitToStart = 9.5f;
                position1 = new Vector3(-40, -70, posZ);
                position2 = new Vector3(-40, 70, posZ);
                rotation1 = new Vector3(0, 180, 0);
            }
            // Initial Position
            target.localPosition = position1;
            target.localRotation = Quaternion.Euler(rotation1);

            //Sequence .SetLookAt(0.01f)
            sMovement.AppendInterval(waitToStart);
            sMovement.Append(target.DOLocalMove(position2, timeToMove1).SetEase(Ease.OutCubic));
            sMovement.InsertCallback(waitToStart + timeToMove1 - 15, OnShoot.Invoke);
            sMovement.AppendCallback(OnDeactivate.Invoke);
            return sMovement;
        }

    }

}
