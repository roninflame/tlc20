using DG.Tweening;
using PolloScripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Enemies.Patterns
{
    
    public class Pattern_M04 : IPatternMovement
    {
        public Pattern_M04(int index)
        {
            _index = index;
        }
        private int _index;
        public Sequence Movement(Transform target, Action OnShoot, Action OnDeactivate, Action OnLookAt)
        {
            Sequence sMovement = DOTween.Sequence();

            Vector3 position1 = Vector3.zero;
            Vector3 position2 = Vector3.zero;
            //Vector3 position4 = Vector3.zero;
            Vector3 rotation1 = Vector3.zero;
            Vector3 rotation2 = Vector3.zero;

            float waitToStart = 0f;
            float timeToMove1 = 16;
            float rotationDuration = 0.5f;

            

            if (_index == 1)
            {
                waitToStart = 0;
                position1 = new Vector3(200, -40, 150);
                position2 = new Vector3(-200, -40, 150);
 
            }
            else if (_index == 2)
            {
                waitToStart = 1f;
                position1 = new Vector3(200, -20, 150);
                position2 = new Vector3(-200, -20, 150);
            }
            else if (_index == 3)
            {
                waitToStart = 2f;
                position1 = new Vector3(200, -40, 150);
                position2 = new Vector3(-200, -40, 150);
            }
            else if (_index == 4)
            {
                waitToStart = 3f;
                position1 = new Vector3(200, -20, 150);
                position2 = new Vector3(-200, -20, 150);
            }
            else if (_index == 5)
            {
                waitToStart = 4f;
                position1 = new Vector3(200, -40, 150);
                position2 = new Vector3(-200, -40, 150);
            }
            else if (_index == 6)
            {
                waitToStart = 5f;
                position1 = new Vector3(200, -20, 150);
                position2 = new Vector3(-200, -20, 150);
            }
            else if (_index == 7)
            {
                waitToStart = 6f;
                position1 = new Vector3(200, -40, 150);
                position2 = new Vector3(-200, -40, 150);
            }
            else if (_index == 8)
            {
                waitToStart = 7f;
                position1 = new Vector3(200, -20, 150);
                position2 = new Vector3(-200, -20, 150);
            }
            rotation1 = new Vector3(0, -90, 0);
            rotation2 = new Vector3(0, -180, 0);
            // Initial Position
            target.localPosition = position1;
            target.localRotation = Quaternion.Euler(rotation1);

            //Sequence
            sMovement.AppendInterval(waitToStart);
            sMovement.Append(target.DOLocalMove(position2, timeToMove1));
            sMovement.InsertCallback(timeToMove1 / 3f + waitToStart, OnShoot.Invoke);
            //sMovement.Join(target.DOLocalRotate(rotation2, rotationDuration).SetDelay(4).OnComplete(OnShoot.Invoke));
            sMovement.AppendCallback(OnDeactivate.Invoke);
            return sMovement;
        }
    }

}
