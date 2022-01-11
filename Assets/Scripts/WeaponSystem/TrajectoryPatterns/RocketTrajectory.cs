using DG.Tweening;
using PolloScripts.Interfaces;
using System;
using System.Collections;
using UnityEngine;

namespace PolloScripts.WeaponSystem
{
    public class RocketTrajectory : IRocketMovement
    {
        public Sequence Move(Transform target, Vector3 destiny, Vector3 rotation, Action OnLineRendererON, Action OnLineRendererOFF, Action OnDeactivate)
        {
            OnLineRendererOFF?.Invoke();
            Vector3 rotation1 = new Vector3(-90, 0, 0);
            Vector3 rotation2 = new Vector3(90, 0, 0);

            float speed = 2;
            //float newPosSpeed = 1;
            float speed2 = 1.5f;
            Sequence sec = DOTween.Sequence();

            target.rotation = Quaternion.Euler(rotation1);
   
            sec.Append(target.DOLocalMoveY(100, speed));
            sec.Append(target.DOLocalRotate(rotation2, 0));
            sec.Append(target.DOLocalMove(destiny, 0));

            sec.AppendCallback(()=>OnLineRendererON?.Invoke());
            sec.AppendInterval(1);

            sec.Append(target.DOLocalMoveY(-20, speed2));
            sec.AppendCallback(()=>OnLineRendererOFF?.Invoke());
            sec.AppendCallback(OnDeactivate.Invoke);
            return sec;
        }

       
    }
}