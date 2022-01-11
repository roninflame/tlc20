using DG.Tweening;
using PolloScripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Enemies.Patterns
{
    public class SimplePatternMove : IPatternMove
    {
        private int _index;
        public SimplePatternMove(int index)
        {
            _index = index;
        }
        public Sequence Move(Transform target, Action OnShoot, Action OnDeactivate, Action OnLookAt)
        {
            Sequence sec = DOTween.Sequence();

            float waitForStart = 0;
            //position 1
            Vector3 startPosition1 = new Vector3(0, 0, 0);
            Vector3 endPosition1 = new Vector3(0, 0, 0);
            Quaternion startRotation1 = Quaternion.Euler(0,0, 0);
            float timeToMove1 = 2f;

            //Position 2
            Vector3 endPosition2 = new Vector3(0, 0, 0);
            float timeToMove2 = 2f;


            float shootiingTime = 4;
            //float timeToShoot = 2f;

            //Fix Positions
            if (_index == 0)
            {
                startPosition1 = new Vector3(-80, 0, 0);
                endPosition1 = new Vector3(-50, 0, 150);
                startRotation1 = Quaternion.Euler(0, 30, 0);
            }
            else if (_index == 1)
            {
                startPosition1 = new Vector3(80, 0, 0);
                endPosition1 = new Vector3(50, 0, 150);
                startRotation1 = Quaternion.Euler(0, -30, 0);
                waitForStart = 1;
            }
            else if (_index == 2)
            {
                startPosition1 = new Vector3(0, 80, 0);
                endPosition1 = new Vector3(0, 40, 150);
                startRotation1 = Quaternion.Euler(0, 0, 0);
                waitForStart = timeToMove1 + 1;
            }
            else if (_index == 3)
            {
                startPosition1 = new Vector3(0, -80, 0);
                endPosition1 = new Vector3(0, -40, 150);
                startRotation1 = Quaternion.Euler(0, 0, 0);
                waitForStart = timeToMove1 + 2;
            }
            endPosition2 = startPosition1;

            target.localPosition = startPosition1;
            target.localRotation = startRotation1;

            //Sequence
            //Move 1
            sec.AppendInterval(waitForStart);
            sec.Append(target.DOLocalMove(endPosition1, timeToMove1).SetEase(Ease.OutCirc));
            //Rotation 
            sec.Insert(waitForStart + timeToMove1 / 2, target.DORotate(new Vector3(0, _index % 2 == 0 ? 180 : -180, 0), 0.5f));
            //
            sec.AppendCallback(OnShoot.Invoke);
            sec.AppendInterval(shootiingTime);
            
            
            //Move 2
            sec.Append(target.DOLocalMove(endPosition2, timeToMove2).SetEase(Ease.OutCirc));
            //Exit
            sec.AppendCallback(OnDeactivate.Invoke);
            return sec;
        }
    }

}
