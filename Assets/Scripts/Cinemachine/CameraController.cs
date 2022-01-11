using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PolloScripts
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance { get; private set; }
        public bool IsActive => _isActive;
        [SerializeField] private GameObject _playerCamera;
        [SerializeField] private GameObject _loadingCamera;

        private bool _isActive;


        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetActive(bool b)
        {
            gameObject.SetActive(b);
        }
        public void ActivatePlayerCamera()
        {
            _playerCamera.SetActive(true);
            _loadingCamera.SetActive(false);
        }
        public void ActivateLoadingCamera()
        {
            _playerCamera.SetActive(false);
            _loadingCamera.SetActive(true);
        }

        public void CamerasOFF()
        {
            _playerCamera.SetActive(false);
            _loadingCamera.SetActive(false);
        }

        
    }
}

