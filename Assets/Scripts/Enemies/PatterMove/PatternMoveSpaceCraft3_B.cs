using DG.Tweening;
using PolloScripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Enemies
{
    public class PatternMoveSpaceCraft3_B : IPatternMove
    {
        private int _index;
        private float _waitForStart = 0;

        public PatternMoveSpaceCraft3_B(int index, float waitForStart)
        {
            _index = index;
            _waitForStart = waitForStart;
        }
        public Sequence Move(Transform target, Action OnShoot, Action OnDeactivate, Action OnLookAt)
        {

            //position 1
            Vector3 _startPosition1 = new Vector3(0, 50, 0);
            Vector3 _endPosition1 = new Vector3(0, 0, 120);
            float _timeToMove1 = 3f;

            //position 2
            Vector3 _endPosition2 = new Vector3(0, 0, 120);

            float _timeToMove2 = 3;

            if (_index == 0)
            {

            }
            else if (_index == 1)
            {
                _startPosition1.x = -50;
                _endPosition1.x = _startPosition1.x;
                _endPosition1.y = 30;
            }
            else if (_index == 2)
            {
                _startPosition1.x = 50;
                _endPosition1.x = _startPosition1.x;
                _endPosition1.y = 30;
            }

            target.localPosition = _startPosition1;
            target.localRotation = Quaternion.Euler(0, 180, 0);
            Sequence sec = DOTween.Sequence();

            sec.AppendInterval(_waitForStart);
            sec.Append(target.DOLocalMove(new Vector3(_endPosition1.x, _endPosition1.y, _endPosition1.z), _timeToMove1).SetEase(Ease.OutCirc));
            sec.InsertCallback(_timeToMove1 + _waitForStart, OnShoot.Invoke);
            sec.AppendInterval(6);
            sec.Append(target.DOLocalMove(_startPosition1, _timeToMove2));
            sec.AppendCallback(OnDeactivate.Invoke);
            return sec;
        }
    }

}
