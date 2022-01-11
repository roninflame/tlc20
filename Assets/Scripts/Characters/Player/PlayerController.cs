
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace PolloScripts
{

    public class PlayerController : MonoBehaviour
    {

        public IntEvent OnWeapon = new IntEvent();
        public BoolEvent OnFiring = new BoolEvent();
        public BoolEvent OnBoosting = new BoolEvent();
        public BoolEvent OnZomming = new BoolEvent();
        public BoolEvent OnSlowing = new BoolEvent();

        //share
        public Vector2 InputXY => _rawInputMovement;
        public Vector2 InputLook => _rawInputLook;
        public bool IsZooming => _isZooming;
        public bool IsBoosting => _isBoosting;

        public bool IsFiring => _isFiring;
        public string CurrentControlScheme => _currentControlScheme;
        public string CurrentActionMap => _playerInput.currentActionMap.name;

        //[Header("Input Settings")]
        private PlayerInput _playerInput;

        // Input values
        private Vector3 _rawInputMovement;
        private Vector2 _rawInputLook;
        private bool _isZooming;
        private bool _isBoosting;
        private bool _isFiring;
      
        //Action Maps
        private string _actionMapPlayerControls = "Player Controls";
        private string _actionMapMenuControls = "Menu Controls";

        //Current Control Scheme
        private string _currentControlScheme;

        

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        void Start()
        {
            _currentControlScheme = _playerInput.currentControlScheme;
        }

        #region Metodos
        public string GetDeviceName()
        {

            string currentDeviceRawPath = _playerInput.devices[0].ToString();

            string newDisplayName = null;

            //for (int i = 0; i < listDeviceSets.Count; i++)
            //{

            //    if (listDeviceSets[i].deviceRawPath == currentDeviceRawPath)
            //    {
            //        newDisplayName = listDeviceSets[i].deviceDisplaySettings.deviceDisplayName;
            //    }
            //}

            //if (newDisplayName == null)
            //{
            //    newDisplayName = currentDeviceRawPath;
            //}

            return currentDeviceRawPath;

        }
        public void EnableMenuControls()
        {
            _playerInput.SwitchCurrentActionMap(_actionMapMenuControls);
        }
        public void EnableGameplayControls()
        {
            _playerInput.SwitchCurrentActionMap(_actionMapPlayerControls);
        }

        public bool MenuControlsActivated()
        {
            return (_playerInput.currentActionMap.name == _actionMapMenuControls) ? true : false;
        }
        #endregion

        #region Eventos Player Input
        public void OnMovement(InputAction.CallbackContext value)
        {
            _rawInputMovement = value.ReadValue<Vector2>();
        }
        public void OnLook(InputAction.CallbackContext context)
        {
            _rawInputLook = context.ReadValue<Vector2>();
        }
        public void OnZoom(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _isZooming = true;
                OnZomming?.Invoke(true);
            }
            else if (context.canceled)
            {
                _isZooming = false;
                OnZomming?.Invoke(false);
            }
        }
        public void OnBoost(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _isBoosting = true;
                OnBoosting?.Invoke(true);
            }
            else if (context.canceled)
            {
                _isBoosting = false;
                OnBoosting?.Invoke(false);
            }
        }
        public void OnFire1(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _isFiring = true;
                OnFiring?.Invoke(true);
            }
            else if (context.canceled)
            {
                _isFiring = false;
                OnFiring?.Invoke(false);
            }
        }

        public void OnWeapon1(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnWeapon?.Invoke(0);
            }

        }
        public void OnWeapon2(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnWeapon?.Invoke(1);
            }

        }

        public void OnWeapon3(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnWeapon?.Invoke(2);
            }
        }
        public void OnWeapon4(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnWeapon?.Invoke(3);
            }
        }
        public void OnSlowMotion(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                OnSlowing?.Invoke(true);
            }
            else if (context.canceled)
            {
                OnSlowing?.Invoke(false);
            }
        }


        public void OnDeviceLost()
        {
            print(_currentControlScheme + " <Lost>");
            //playerVisualsBehaviour.SetDisconnectedDeviceVisuals();
        }
        public void OnDeviceRegained()
        {
            print("regain");
            //StartCoroutine(WaitForDeviceToBeRegained());
        }
        public void OnControlsChanged()
        {
            if (_playerInput.currentControlScheme != _currentControlScheme)
            {
                _currentControlScheme = _playerInput.currentControlScheme;
                RemoveAllBindingOverrides();
            }
        }
        void RemoveAllBindingOverrides()
        {
            InputActionRebindingExtensions.RemoveAllBindingOverrides(_playerInput.currentActionMap);
        }
        #endregion


    }
}

