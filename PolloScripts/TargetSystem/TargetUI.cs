using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.TargetSystem
{
    public class TargetUI : MonoBehaviour
    {
        private Transform _target;
        //int tweenId = 0;
        private RectTransform _rectt;
        private float _escalaTarget;
        void Update()
        {
            if (_target != null)
            {
                transform.position = Camera.main.WorldToScreenPoint(_target.position);
            }
        }
        public void SetTarget(Transform tra, float escalaTarget)
        {
            _escalaTarget = escalaTarget;
            _target = tra;
            gameObject.SetActive(true);
            if (_rectt == null)
            {
                _rectt = GetComponent<RectTransform>();
            }
            IniciaTargetAnima();
        }
        public void Desactivar()
        {
            DetenerTargetAnima();
            gameObject.SetActive(false);
            _target = null;
        }
        public void IniciaTargetAnima()
        {
            //if (tweenId == 0)
            //{
            //    tweenId = LeanTween.scale(_rectt, Vector3.one * _escalaTarget, 0.2f).setEaseInOutSine().setLoopPingPong().id;
            //}
            //else if (!LeanTween.isTweening(tweenId))
            //{
            //    tweenId = LeanTween.scale(_rectt, Vector3.one * _escalaTarget, 0.2f).setEaseInOutSine().setLoopPingPong().id;
            //}


        }
        public void DetenerTargetAnima()
        {
            //LeanTween.cancel(tweenId);
            _rectt.localScale = Vector3.one;
            //tweenId = 0;
        }
    }

}
