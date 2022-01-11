using Cinemachine;
using DG.Tweening;
using PolloScripts.Effects;
using PolloScripts.Enums;
using PolloScripts.Interfaces;
using PolloScripts.ObjectPoolSystem;
using PolloScripts.TargetSystem;
using PolloScripts.UI;
using PolloScripts.WeaponSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PolloScripts
{
    public class Player : MonoBehaviour, IPlayer
    {

        [Space]
        [Header("References")]
        [Space]
        public CinemachineDollyCart dollyCart;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private PlayerAnimations _playerAnima;
        public ObjectTracker objectTraker;

        [Space]
        [Header("Camera")]
        [Space]
        [SerializeField] private Transform _cameraParent;
        [SerializeField] private CinemachineVirtualCamera virtualCam;
        [SerializeField] private float _shakeAmplitude = 5;
        [SerializeField] private float _shakeFrequency = 5;

        [Space]
        [Header("Base Parameters")]
        [Space]
        [SerializeField] private EnergyShield _shield;
        [SerializeField] private int _health;
        [SerializeField] private int _energy;
        [SerializeField] private float _damageCoolDown = 1f;

        [Space]
        [Header("Movement")]
        [Space]
        [SerializeField] private Vector2 _limiteX = new Vector2(-12, 12);
        [SerializeField] private Vector2 _limiteY = new Vector2(-8, 8);
        [SerializeField] private float _horizontalLerpTime = 0.1f;
        [SerializeField] private float _horizontalLeanLimitX = 40;
        [SerializeField] private float _forwardSpeed = 50;
        [SerializeField] private float _xySpeed = 25;
        [SerializeField] private float _xySpeedKB = 16f;

        [Space]
        [Header("Boost")]
        [Space]
        [SerializeField] private float _boostSpeed = 80;
        [SerializeField] private float _camBoost = -1;

        [Space]
        [Header("Zoom")]
        [Space]
        [SerializeField] private float _camZoom = 7;


        [Space]
        [Header("===== CrossHair ====")]
        [Space]
        [SerializeField] private float gamePadRightSensitivity = 30;
        [SerializeField] private float gamePadlerpCrossHair = 20;
        [SerializeField] private float keyboardlerpCrossHair = 20;
        private float lerpCrossHair;
        private Vector2 newInputRot = Vector2.zero;
        private Vector2 screenPosition = new Vector2();

        [Space]
        [Header("Go References")]
        [Space]
        [SerializeField] private Transform _model;
        [SerializeField] private GameObject _modelBone;
        [SerializeField] private Transform _hitPlace;
        [SerializeField] private Transform aimTarget;
        [SerializeField] private Transform CrossHair;
        [SerializeField] private Transform _shotSpot;
        [SerializeField] private float _fireRate;
        [SerializeField] private ParticleSystemForceField _particleForceField;

        [Space]
        [Header("Particles")]
        [Space]
        [SerializeField] private List<ParticleSystem> trail;
        [SerializeField] private ParticleSystem circle;
        //[SerializeField] private ParticleSystem barrel;
        [SerializeField] private ParticleSystem stars;
        [SerializeField] private GameObject _ghostTrailGO;

        public UnityEvent OnDamageAnimation;
        public UnityEvent OnNormalAnimation;

        [Space]
        [Header("FMOD")]
        [FMODUnity.EventRef]
        [SerializeField] protected string _hitSound;

        //Share
        public int Health => _health;
        public int Energy => _energy;
        public Vector3 hitPosition => _hitPlace.position;
        public Transform hitTransform => _hitPlace;
        public Vector3 ModelPosition => _model.position;
        public Transform ModelTR => _model;

        //LOCAL
        private bool _isTakingDamage;
        private bool _canTakeDamage;
        private bool _isDeath;

        //Parametros para armas
        private float nextFire;

        private WeaponBase currentWeapon;
        

        void Update()
        {

            //if (UnityEngine.InputSystem.Keyboard.current.cKey.isPressed)
            //{
            //    PlayerGhost g = ObjectPoolSystem.ObjectPoolPlayerGhost.Instance.ActivateFromPool();
            //    g.Initialized(dollyCart.transform, _modelPrefab, _modelBone);
            //    g.CopyObjectBones();
            //}

            if (_isDeath)
                return;

            if (!_canTakeDamage)
                return;

            _playerAnima.MoveXAnimation(_playerController.InputXY.x);
            LocalMove();
            RotationLook();
            HorizontalLean();
            MoveCrossHair();

            if (_playerController.IsFiring)
            {
                PlayerManager.Instance.playerWeapons.Fire(Time.time);
                _playerAnima.ShootingAnimation(true);

                return;
            }
            _playerAnima.ShootingAnimation(PlayerManager.Instance.playerWeapons.specialWeaponActivated);

        }
        public void SetParticleForceField(float startRange, float endRange)
        {
            _particleForceField.startRange = startRange;
            _particleForceField.endRange = endRange;
        }
        public void PruebasDano()
        {
            StartCoroutine(IenDamageCoolDown());
        }
        private IEnumerator IenDamageCoolDown()
        {
            OnDamageAnimation.Invoke();
            _shield.Show();
            ShakeCamera(true);
            HitSound();
            yield return new WaitForSeconds(_damageCoolDown);
            OnNormalAnimation.Invoke();
            _shield.Hide();
            _isTakingDamage = false;
            ShakeCamera(false);
        }

        #region Camera

        public void ShakeCamera(bool active)
        {
            CinemachineBasicMultiChannelPerlin perlin = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (active)
            {
                perlin.m_AmplitudeGain = _shakeAmplitude;
                perlin.m_FrequencyGain = _shakeFrequency;
            }
            else
            {
                perlin.m_AmplitudeGain = 1;
                perlin.m_FrequencyGain = 1;
            }
            

        }

        #endregion

        #region Movement

        public void LocalMove()
        {
            if (_playerController.CurrentControlScheme == "Gamepad")
            {
                transform.localPosition += new Vector3(_playerController.InputXY.x, _playerController.InputXY.y) * _xySpeed * Time.deltaTime * CanvasManager.Instance.slowMotionBar.GetPlayerTimeScale();

            }
            else if (_playerController.CurrentControlScheme == "Keyboard&Mouse")
            {
                transform.localPosition += new Vector3(_playerController.InputXY.x, _playerController.InputXY.y) * _xySpeedKB * Time.deltaTime * CanvasManager.Instance.slowMotionBar.GetPlayerTimeScale();

            }
            ClampPosition();
        }
        private void ClampPosition()
        {
            transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, _limiteX.x, _limiteX.y)
                                , Mathf.Clamp(transform.localPosition.y, _limiteY.x, _limiteY.y)
                                , 0f);
        }
        public void HorizontalLean()
        {
            Vector3 targetEulerAngels = _model.localEulerAngles;
            float axis = _playerController.InputXY.x;
            _model.localEulerAngles = new Vector3(targetEulerAngels.x, targetEulerAngels.y, Mathf.LerpAngle(targetEulerAngels.z, -axis * _horizontalLeanLimitX, _horizontalLerpTime));
        }
        public void RotationLook()
        {
            //aimTarget.parent.position = Vector3.zero;
            //aimTarget.localPosition = new Vector3(Cross.anchoredPosition.normalized.x, Cross.anchoredPosition.normalized.y, 2);
            //Player.rotation = Quaternion.RotateTowards(Player.rotation, Quaternion.LookRotation(aimTarget.localPosition), Mathf.Deg2Rad * speed * Time.deltaTime);

            aimTarget.position = Camera.main.ScreenToWorldPoint(new Vector3(CrossHair.position.x, CrossHair.position.y, 100));
            transform.rotation = Quaternion.LookRotation(aimTarget.position - transform.position);
        }

        public void OnGhostEffect()
        {
            PlayerGhost g = ObjectPoolSystem.ObjectPoolPlayerGhost.Instance.ActivateFromPool();
            g.Initialized(dollyCart.transform, transform, _modelBone);
            g.CopyObjectBones();
        }

        public void OnGhostTrail(bool value)
        {
            ////_ghostTrailGO.SetActive(value);
        }
        #endregion

        #region CrossHair

        public void MoveCrossHair()
        {

            if (_playerController.CurrentControlScheme == "Gamepad")
            {
                lerpCrossHair = gamePadlerpCrossHair;
                if (newInputRot == Vector2.zero)
                {
                    newInputRot = new Vector2(Screen.width / 2, Screen.height / 2);
                }
                else
                {
                    newInputRot += _playerController.InputLook * gamePadRightSensitivity;
                    screenPosition = new Vector2(Mathf.Clamp(newInputRot.x, Screen.width * 0.05f, Screen.width * 0.95f),
                            Mathf.Clamp(newInputRot.y, Screen.height * 0.05f, Screen.height * 0.95f));
                    newInputRot = screenPosition;
                }
            }
            else if (_playerController.CurrentControlScheme == "Keyboard&Mouse")
            {
                lerpCrossHair = keyboardlerpCrossHair;
                //if (playerController.InputRot == Vector2.zero)
                //{
                //    newInputRot = new Vector2(Screen.width / 2, Screen.height / 2);
                //}
                if (_playerController.InputLook != Vector2.zero)
                {
                    newInputRot = _playerController.InputLook;
                    screenPosition = new Vector2(Mathf.Clamp(newInputRot.x, Screen.width * 0.05f, Screen.width * 0.95f),
                            Mathf.Clamp(newInputRot.y, Screen.height * 0.05f, Screen.height * 0.95f));

                }
            }
            lerpCrossHair = PlayerManager.Instance.playerWeapons.handicapMoveCrosshair == 0 ? lerpCrossHair : lerpCrossHair * PlayerManager.Instance.playerWeapons.handicapMoveCrosshair;

            if (_playerController.MenuControlsActivated())
            {
                
                CrossHair.position = Vector2.Lerp(CrossHair.position, new Vector2(Screen.width / 2, Screen.height / 2), Time.deltaTime * lerpCrossHair);
            }
            else
            {
           

                CrossHair.position = Vector2.Lerp(CrossHair.position, screenPosition, Time.deltaTime * lerpCrossHair);
            }

            //Vector2 screenPosA;
            //Vector2 _resInputRot;
            //if (playerController.InputRot == Vector2.zero)
            //{
            //    _resInputRot = new Vector2(Screen.width / 2, Screen.height / 2);
            //}
            //else
            //{
            //    _resInputRot = playerController.InputRot;

            //}
            ////controlo que el crosshair no pase los limites de la pantalla
            //screenPosA = new Vector2(Mathf.Clamp(_resInputRot.x, Screen.width * 0.05f, Screen.width * 0.95f),
            //    Mathf.Clamp(_resInputRot.y, Screen.height * 0.05f, Screen.height * 0.95f));
            ////Muevo target area con un poco de lerp
            //CrossHair.position = Vector2.Lerp(CrossHair.position, screenPosA, Time.deltaTime * 5f);
        }

        #endregion

        #region ATTACK



        #endregion

        #region IPLAYER

        public bool CanTakeDamage => _canTakeDamage;
        public bool IsDeath => _isDeath;
        public void DamageReceived(int value)
        {

            if (!_isTakingDamage && !_isDeath)
            {
                CanvasManager.Instance.healthBar.SubtractValue(value);


                if (CanvasManager.Instance.healthBar.CurrentValue > 0)
                {
                    
                    _isTakingDamage = true;
                    //change shield color
                    if (CanvasManager.Instance.healthBar.CurrentValue == 1)
                        _shield.SetCrackColor();
                    else
                        _shield.SetNormalColor();

                    StartCoroutine(IenDamageCoolDown());
                }
                else
                {
                    Death();
                }
            }
        }

        public void Activate()
        {
            _canTakeDamage = true;
            _isDeath = false;
        }
        public void Deactivate()
        {
            //Player
            _canTakeDamage = false;
            //Path
            dollyCart.m_Activate = false;
            //Bars
            CanvasManager.Instance.energyBar?.StopCountDown();
            ShakeCamera(false);
            TargetManager.Instance.Deactivate();
            _ghostTrailGO.SetActive(false);
        }
        public void Death()
        {
            if(Time.timeScale != 1)
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = Time.timeScale;
            }
            
            CanvasManager.Instance.OnSaveScore?.Invoke();

            Deactivate();
            _isDeath = true;
            _playerAnima.DeathAnimation(true);
            StartCoroutine(IenWaitForReset());
        }
        public void ResetPlayer()
        {
            dollyCart.m_Position = 0;
            dollyCart.transform.position = Vector3.zero;
            dollyCart.transform.rotation = Quaternion.Euler(Vector3.zero);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            _model.rotation = Quaternion.Euler(0, 0, 0);
            _playerAnima.DeathAnimation(false);
            _playerAnima.ShootingAnimation(false);
            _playerAnima.MoveXAnimation(0f);
            _ghostTrailGO.SetActive(false);
        }
        #endregion

        #region Eventos
        //public void Break(bool state)
        //{
        //    float speed = state ? ForwardSpeed / 3 : ForwardSpeed;
        //    float zoom = state ? cameraZoom : 0;

        //    DOVirtual.Float(dolly.m_Speed, speed, .15f, SetSpeed);
        //    SetCameraZoom(zoom, .4f);
        //}
        public void Zoom(bool state)
        {
            //float speed = state ? forwardSpeed / 3 : forwardSpeed;
            float zoom = state ? _camZoom : 0;
            //DOVirtual.Float(dolly.m_Speed, forwardSpeed, .15f, SetSpeed);
            SetCameraZoom(zoom, .4f);
        }
        public void Boost(bool state)
        {

            if (state)
            {
                _cameraParent.GetComponentInChildren<CinemachineImpulseSource>().GenerateImpulse();

                foreach (var item in trail)
                {
                    item.Play();
                }
                //trail.Play();
                circle.Play();
            }
            else
            {
                foreach (var item in trail)
                {
                    item.Stop();
                }
                //trail.Stop();
                circle.Stop();
            }
            foreach (var item in trail)
            {
                item.GetComponent<TrailRenderer>().emitting = state;
            }
            //trail.GetComponent<TrailRenderer>().emitting = state;

            float origFov = state ? 40 : 55;
            float endFov = state ? 55 : 40;
            float origChrom = state ? 0 : 1;
            float endChrom = state ? 1 : 0;
            float origDistortion = state ? 0 : -0.5f;
            float endDistorton = state ? -0.5f : 0;
            float starsVel = state ? -80 : -40;
            float speed = state ? _boostSpeed * 2 : _forwardSpeed;
            float zoom = state ? _camBoost : 0;

            DOVirtual.Float(origChrom, endChrom, .5f, Chromatic);
            DOVirtual.Float(origFov, endFov, .5f, FieldOfView);
            DOVirtual.Float(origDistortion, endDistorton, .15f, DistortionAmount);
            var pvel = stars.velocityOverLifetime;
            pvel.z = starsVel;

            DOVirtual.Float(dollyCart.m_Speed, speed, .15f, SetSpeed);
            SetCameraZoom(zoom, .4f);
        }
        

        void SetSpeed(float x)
        {
            dollyCart.m_Speed = x;
        }
        void SetCameraZoom(float zoom, float duration)
        {
            _cameraParent.DOLocalMove(new Vector3(0, 0, zoom), duration);
        }
        void FieldOfView(float fov)
        {
            _cameraParent.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = fov;
        }
        void Chromatic(float x)
        {
            VolumeManager.Instancia.ChromaticAberration.intensity.value = x;
        }
        void DistortionAmount(float x)
        {
            //UnityEngine.Rendering.VolumeProfile volumeProfile = FindObjectOfType<UnityEngine.Rendering.Volume>().profile;
            //UnityEngine.Rendering.Universal.LensDistortion lens;
            //volumeProfile.TryGet(out lens);
            //lens.intensity.Override(x);
            //VolumeManager.Instancia.LensDistortion.intensity.min = -30;
            //VolumeManager.Instancia.LensDistortion.intensity.max = 30;
            VolumeManager.Instancia.LensDistortion.intensity.value = x;

        }
        #endregion

        IEnumerator IenWaitForReset()
        {
            //Effects.ExplosionBase explosion = ObjectPoolSystem.ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, transform.position, Quaternion.identity);
            //explosion.ReSize(5);
            //explosion.Activate(); 

            ExplosionBase explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Player, null, transform.position, Quaternion.identity);
            explosion.Activate();
            yield return new WaitForSeconds(2f);
            GameManager.Instance.Death();
            CanvasManager.Instance.uiExpressions.Stop();
        }

        private void HitSound()
        {
            FMODUnity.RuntimeManager.PlayOneShot(_hitSound, transform.position);
        }
    }
}

