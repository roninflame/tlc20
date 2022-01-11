
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using System.Collections.Generic;

namespace PolloScripts
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Controller")]
        private PlayerController playerController;

        [Space]

        [Header("Public References")]
        public CinemachineDollyCart dollyCart;
        public Transform cameraParent;

        //public RectTransform Canvas;
      
       

        [Space]
        [Header("Transforms")]
        public Transform Player;
        public Transform PlayerModel;
        public Transform aimTarget;
        [SerializeField] private Transform CrossHair;
        [Space]
        [Header("===== PARAMETERS ====")]
        [Space]
        [SerializeField] private Vector2 limiteX = new Vector2(-12, 12);
        [SerializeField] private Vector2 limiteY = new Vector2(-8, 8);
        [SerializeField] private float horizontalLerpTime = 0.1f;
        [SerializeField] private float horizontalLeanLimitX = 40;
        [Space]
        [Header("===== CrossHair ====")]
        [Space]
        [SerializeField] private float gamePadRightSensitivity = 15f;
        [SerializeField] private float gamePadlerpCrossHair = 1f;
        [SerializeField] private float keyboardlerpCrossHair = 1f;
        private float lerpCrossHair;
        private Vector2 newInputRot = Vector2.zero;
        private Vector2 screenPosition = new Vector2();

        [Header("Boost")]
        [SerializeField] private float boostSpeed = 80;
        [SerializeField] private float forwardSpeed = 50;
        [SerializeField] private float xySpeed = 12;
        [SerializeField] private float camBoost = -1;
        [Header("Zoom")]
        [SerializeField] private float camZoom = 5;
        //public float lookSpeed = 340;


        [Space]

        [Header("Particles")]
        [SerializeField] private List<ParticleSystem> trail;
        [SerializeField] private ParticleSystem circle;
        //[SerializeField] private ParticleSystem barrel;
        [SerializeField] private ParticleSystem stars;

        public float ForwardSpeed { get => forwardSpeed; set => forwardSpeed = value; }

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(aimTarget.position, .5f);
            Gizmos.DrawSphere(aimTarget.position, .15f);

        }
        public void LocalMove()
        {
            Player.localPosition += new Vector3(playerController.InputXY.x, playerController.InputXY.y) * xySpeed * Time.deltaTime;
            ClampPosition();
        }
        private void ClampPosition()
        {
            Player.localPosition = new Vector3(Mathf.Clamp(Player.localPosition.x, limiteX.x, limiteX.y)
                                , Mathf.Clamp(Player.localPosition.y, limiteY.x, limiteY.y)
                                , 0f);
        }
        public void HorizontalLean()
        {
            Vector3 targetEulerAngels = PlayerModel.localEulerAngles;
            float axis = playerController.InputXY.x;
            PlayerModel.localEulerAngles = new Vector3(targetEulerAngels.x, targetEulerAngels.y, Mathf.LerpAngle(targetEulerAngels.z, -axis * horizontalLeanLimitX, horizontalLerpTime));
        }
        public void RotationLook()
        {
            //aimTarget.parent.position = Vector3.zero;
            //aimTarget.localPosition = new Vector3(Cross.anchoredPosition.normalized.x, Cross.anchoredPosition.normalized.y, 2);
            //Player.rotation = Quaternion.RotateTowards(Player.rotation, Quaternion.LookRotation(aimTarget.localPosition), Mathf.Deg2Rad * speed * Time.deltaTime);
            
            aimTarget.localPosition = Camera.main.ScreenToWorldPoint(new Vector3(CrossHair.position.x, CrossHair.position.y, 100));
            Player.rotation = Quaternion.LookRotation(aimTarget.localPosition - Player.position);
        }

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
            float zoom = state ? camZoom : 0;
            //DOVirtual.Float(dolly.m_Speed, forwardSpeed, .15f, SetSpeed);
            SetCameraZoom(zoom, .4f);
        }
        public void Boost(bool state)
        {

            if (state)
            {
                cameraParent.GetComponentInChildren<CinemachineImpulseSource>().GenerateImpulse();
      
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
            float speed = state ? boostSpeed * 2 : ForwardSpeed;
            float zoom = state ? camBoost : 0;

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
            cameraParent.DOLocalMove(new Vector3(0, 0, zoom), duration);
        }
        void FieldOfView(float fov)
        {
            cameraParent.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = fov;
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

        public void MoveCrossHair()
        {
            
            if (playerController.CurrentControlScheme == "Gamepad")
            {
                lerpCrossHair = gamePadlerpCrossHair;
                if (newInputRot == Vector2.zero)
                {
                    newInputRot = new Vector2(Screen.width / 2, Screen.height / 2);
                }
                else
                {
                    newInputRot += playerController.InputLook * gamePadRightSensitivity;
                    screenPosition =new Vector2(Mathf.Clamp(newInputRot.x, Screen.width * 0.05f, Screen.width * 0.95f),
                            Mathf.Clamp(newInputRot.y, Screen.height * 0.05f, Screen.height * 0.95f));
                    newInputRot = screenPosition;
                }
            }
            else if (playerController.CurrentControlScheme == "Keyboard&Mouse")
            {
                lerpCrossHair = keyboardlerpCrossHair;
                //if (playerController.InputRot == Vector2.zero)
                //{
                //    newInputRot = new Vector2(Screen.width / 2, Screen.height / 2);
                //}
                if (playerController.InputLook != Vector2.zero)
                {
                    newInputRot = playerController.InputLook;
                    screenPosition = new Vector2(Mathf.Clamp(newInputRot.x, Screen.width * 0.05f, Screen.width * 0.95f),
                            Mathf.Clamp(newInputRot.y, Screen.height * 0.05f, Screen.height * 0.95f));

                }
            }

            CrossHair.position = Vector2.Lerp(CrossHair.position, screenPosition, Time.deltaTime * lerpCrossHair);

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
    }

}
