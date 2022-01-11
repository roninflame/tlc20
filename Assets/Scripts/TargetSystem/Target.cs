using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.TargetSystem
{
    public class Target// : MonoBehaviour
    {
        private bool _isActivated = false;
        private bool _isTargetActivated = false;
        private float _distanceFromPlayer = 500f;
        private GameObject _targetGO = null;
        //share
        public bool TargetEnable => _isActivated;
        public float DistanceFromPlayer => _distanceFromPlayer;
        public GameObject TargetGO => _targetGO;
        public void UpdateTarget(Vector3 position, GameObject targetGO)
        {
            _targetGO = targetGO;
            _distanceFromPlayer = Vector3.Distance(PlayerManager.Instance.player.ModelPosition, position);
            if (TargetManager.Instance.IsInRange(_distanceFromPlayer))
            {
                if (TargetManager.Instance.IsInTargetArea(position))
                {
                    if (!_isTargetActivated)
                    {
                        _isTargetActivated = true;
                        TargetManager.Instance.AddSelected(this);
                    }
                }
                else
                {
                    if (_isTargetActivated)
                    {
                        _isTargetActivated = false;
                        TargetManager.Instance.Deselect(_targetGO.GetInstanceID());
                    }
                }
            }
            else
            {
                if (_isTargetActivated)
                {
                    _isTargetActivated = false;
                    TargetManager.Instance.Deselect(_targetGO.GetInstanceID());
                }
            }
        }

        public void DeactivateTarget()
        {
            if(_targetGO != null)
            {
                TargetManager.Instance.Deselect(_targetGO.GetInstanceID());
                _isActivated = false;
                _isTargetActivated = false;
            }
           
        }
        public void ActivateTarget()
        {
            _isActivated = true;
            _isTargetActivated = false;
        }
    }

}
