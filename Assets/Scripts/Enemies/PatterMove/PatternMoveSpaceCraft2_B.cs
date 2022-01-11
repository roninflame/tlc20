using DG.Tweening;
using PolloScripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Enemies
{
    public class PatternMoveSpaceCraft2_B : IPatternMove
    {

        public PatternMoveSpaceCraft2_B(int index)
        {
            _index = index;
        }
        private int _index;

        public Sequence Move(Transform target, Action OnShoot, Action OnDeactivate, Action OnLookAt)
        {
            float waitForStart = 0;
            //position 1
            Vector3 startPosition1 = new Vector3(80, 0, 10);
            Vector3 endPosition1 = new Vector3(30, 0, 100);
            float timeToMove1 = 2f;

            //position 2
            //Vector3 startPosition2 = new Vector3(30, 0, 100);
            ////Vector3 _endPosition2;
            //float timeToMove2 = 2;

            //Rotation
            float timeToActivateRotate = 0.7f;
            float timeToRotate2 = 0.5f;

            //Shoot
            float timeToShoot = 0f;

            //Exit
            float timeToExit1 = 3;
            float timeToExit2 = 1;
            Sequence sec = DOTween.Sequence();

            if (_index == 1)
            {
                startPosition1.y = 0;
                startPosition1.x = -80;
                endPosition1.x = -30;
                endPosition1.y = startPosition1.y;
                //startPosition2.y = endPosition1.y;
            }
            else if (_index == 2)
            {
                waitForStart = 0.5f;
                startPosition1.y = 0;
                startPosition1.x = 80;
                endPosition1.x = 30;
                endPosition1.y = startPosition1.y;
                //startPosition2.y = endPosition1.y;
            }
            else if (_index == 3)
            {
                waitForStart = 1;
                startPosition1.y = 80;
                startPosition1.x = 0;
                endPosition1.x = 0;
                endPosition1.y = 25;
                //startPosition2.y = endPosition1.y;
            }
            else if (_index == 4)
            {
                waitForStart = 1.5f;
                startPosition1.y = -80;
                startPosition1.x = 0;
                endPosition1.x = 0;
                endPosition1.y = -25;
                //startPosition2.y = endPosition1.y;
            }

            //Initial Position
            target.localPosition = startPosition1;
            target.localRotation = Quaternion.Euler(0, 0, 0);
            //Sequence
            sec.AppendInterval(waitForStart);
            sec.Append(target.DOLocalMove(endPosition1, timeToMove1).SetEase(Ease.OutCirc));

            sec.Insert(waitForStart + timeToActivateRotate, target.DOLocalRotate(new Vector3(0, (_index % 2 == 0) ? -180 : 180, 0), timeToRotate2));
            sec.InsertCallback(waitForStart + timeToActivateRotate + timeToShoot + timeToRotate2, OnShoot.Invoke);

           // sec.Append(target.DOLocalMove(new Vector3((_index % 2 == 0) ? -startPosition2.x : startPosition2.x, startPosition2.y, startPosition2.z), timeToMove2));

            sec.AppendInterval(timeToExit1);
            sec.Append(target.DOLocalMoveZ(-10, timeToExit2));
            sec.AppendCallback(OnDeactivate.Invoke);

            return sec;
        }


    }

}
