using DG.Tweening;
using PolloScripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.WeaponSystem
{
    public class CrossTrajectory : ITrajectoryPattern
    {
        public Sequence Move(Transform target, Vector3 destiny, Vector3 rotation, Action OnDeactivate)
        {

            Vector3 rotation1 = new Vector3(0, 0, 0);

            float speedRotation = 4;
            float speed = 2f;

            target.rotation = Quaternion.Euler(rotation1);

            Sequence sec = DOTween.Sequence();

            sec.Append(target.DOLocalMove(destiny, 1));
           
            sec.Append(target.DOLocalMoveZ(-15, speed));
            sec.Insert(0.5f, target.DORotate(rotation, speedRotation, RotateMode.WorldAxisAdd));
            sec.AppendCallback(OnDeactivate.Invoke);
            return sec;
        }
    }

}
