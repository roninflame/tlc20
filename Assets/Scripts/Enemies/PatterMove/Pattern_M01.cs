using DG.Tweening;
using PolloScripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Enemies.Patterns
{
    
    public class Pattern_M01 : IPatternMovement
    {
        public Pattern_M01(int index)
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
            Vector3 rotation1 = Vector3.zero;
            float waitToStart = 0f;
            float timeToMove1 = 3f;
            float timeToMove2 = 1f;

            float posZ = 110;
            //float rotationDuration = 1;

            if (_index == 1)
            {
                waitToStart = 0;
                position1 = new Vector3(-20, 60, posZ);
                position2 = new Vector3(-10, 30, posZ);
                position3 = new Vector3(-20, 60, posZ);
                //rotation1 = new Vector3(0, 180, 0);
            }
            else if (_index == 2)
            {
                waitToStart = 1f;
                position1 = new Vector3(20, 60, posZ);
                position2 = new Vector3(10, 30, posZ);
                position3 = new Vector3(20, 60, posZ);
                //rotation1 = new Vector3(0, -180, 0);
            }
            else if (_index == 3)
            {
                waitToStart = 2f;
                position1 = new Vector3(-80, 60, posZ);
                position2 = new Vector3(-30, 10, posZ);
                position3 = new Vector3(-80, 60, posZ);
                //rotation1 = new Vector3(0, 180, 0);
            }
            else if (_index == 4)
            {
                waitToStart = 3f;
                position1 = new Vector3(80, 60, posZ);
                position2 = new Vector3(30, 10, posZ);
                position3 = new Vector3(80, 60, posZ);
                //rotation1 = new Vector3(0, -180, 0);
            }
            if (_index == 5)
            {
                waitToStart = 4;
                position1 = new Vector3(-60, -60, posZ);
                position2 = new Vector3(-30, -10, posZ);
                position3 = new Vector3(-30, -60, posZ);
                //rotation1 = new Vector3(0, 180, 0);
            }
            else if (_index == 6)
            {
                waitToStart = 5f;
                position1 = new Vector3(60, -60, posZ);
                position2 = new Vector3(30, -10, posZ);
                position3 = new Vector3(30, -60, posZ);
                //rotation1 = new Vector3(0, -180, 0);
            }
            else if (_index == 7)
            {
                waitToStart = 6f;
                position1 = new Vector3(-60, -60, posZ);
                position2 = new Vector3(-10, -30, posZ);
                position3 = new Vector3(-50, -60, posZ);
                //rotation1 = new Vector3(0, 180, 0);
            }
            else if (_index == 8)
            {
                waitToStart = 7f;
                position1 = new Vector3(60, -60, posZ);
                position2 = new Vector3(10, -30, posZ);
                position3 = new Vector3(50, -60, posZ);
                //rotation1 = new Vector3(0, -180, 0);
            }
            rotation1 = new Vector3(0, 180, 0);
            // Initial Position
            target.localPosition = position1;
            target.localRotation = Quaternion.Euler(rotation1);

            //Sequence
            sMovement.AppendInterval(waitToStart);
            sMovement.Append(target.DOLocalMove(position2, timeToMove1).SetEase(Ease.OutCirc));
            //sMovement.Append(target.DOLocalRotate(rotation1, rotationDuration));
            sMovement.AppendInterval(1);
            sMovement.AppendCallback(OnShoot.Invoke);
            sMovement.AppendInterval(2);
            sMovement.Append(target.DOLocalMove(position3, timeToMove2));
            sMovement.AppendCallback(OnDeactivate.Invoke);
            return sMovement;
        }
    }

}
