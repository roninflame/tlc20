using PolloScripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Triggers
{
    public class TriggerBoost : MonoBehaviour
    {
        [SerializeField] private Transform modelTra1;
        [SerializeField] private Transform modelTra2;
        [Space]
        [Header("****** Rotation ******")]
        [SerializeField] private DirectionType _directionType;
        [SerializeField] private float _rotatinoSpeed = 5f;
        [SerializeField] private DirectionType _directionType2;
        [SerializeField] private float _rotationSpeed2 = 10f;

        private Vector3 _direction;
        private Vector3 _direction2;

        private bool _contacted;

        private void Start()
        {
            SetDirection();
            SetDirection2();
        }

        private void Update()
        {
            if (_directionType != DirectionType.None)
            {
                modelTra1.transform.rotation *= Quaternion.AngleAxis(_rotatinoSpeed * Time.deltaTime, _direction);
            }

            if (_directionType2 != DirectionType.None)
            {
                modelTra2.transform.rotation *= Quaternion.AngleAxis(_rotationSpeed2 * Time.deltaTime, _direction2);
            }
        }

        private void SetDirection()
        {
            switch (_directionType)
            {
                case DirectionType.None:
                    break;
                case DirectionType.ForwardX:
                    _direction = Vector3.right;
                    break;
                case DirectionType.BackwardX:
                    _direction = Vector3.left;
                    break;
                case DirectionType.RightY:
                    _direction = Vector3.up;
                    break;
                case DirectionType.LeftY:
                    _direction = Vector3.down;
                    break;
                case DirectionType.LeftZ:
                    _direction = Vector3.forward;
                    break;
                case DirectionType.RightZ:
                    _direction = Vector3.back;
                    break;
            }
        }

        private void SetDirection2()
        {
            switch (_directionType2)
            {
                case DirectionType.None:
                    break;
                case DirectionType.ForwardX:
                    _direction2 = Vector3.right;
                    break;
                case DirectionType.BackwardX:
                    _direction2 = Vector3.left;
                    break;
                case DirectionType.RightY:
                    _direction2 = Vector3.up;
                    break;
                case DirectionType.LeftY:
                    _direction2 = Vector3.down;
                    break;
                case DirectionType.LeftZ:
                    _direction2 = Vector3.forward;
                    break;
                case DirectionType.RightZ:
                    _direction2 = Vector3.back;
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !_contacted)
            {
                _contacted = true;

                GameManager.Instance.boostActivated = !GameManager.Instance.boostActivated;
                PlayerManager.Instance.player.Boost(GameManager.Instance.boostActivated);
                StartCoroutine(IenDeactivate());
            }
        }
        IEnumerator IenDeactivate()
        {
            yield return new WaitForSeconds(2f);
            gameObject.SetActive(false);
        }
    }

}
