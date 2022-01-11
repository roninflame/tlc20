using FMODUnity;
using PolloScripts.Data;
using PolloScripts.Enums;
using PolloScripts.Interfaces;
using PolloScripts.ObjectPoolSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.WeaponSystem
{
    public class LaserSmoke : Laser
    {
        [Space]
        [Header("Laser")]
        [SerializeField] private GameObject _hitEffectGO;
        [SerializeField] private float _hitOffset = 0;
        [SerializeField] private bool _useLaserRotation = false;
        private LineRenderer _laser;

        [SerializeField] private float _mainTextureLength = 1f;
        [SerializeField] private float _noiseTextureLength = 1f;
        private Vector4 _length = new Vector4(1, 1, 1, 1);
        //private Vector4 LaserSpeed = new Vector4(0, 0, 0, 0); {DISABLED AFTER UPDATE}
        //private Vector4 LaserStartSpeed; {DISABLED AFTER UPDATE}
        //One activation per shoot
        private bool _laserSaver = false;
        private bool _updateSaver = false;

        private ParticleSystem[] _effectsPS;
        private ParticleSystem[] _hitPS;

        private void Awake()
        {
            _laser = GetComponent<LineRenderer>();
            _effectsPS = GetComponentsInChildren<ParticleSystem>();
            _hitPS = _hitEffectGO.GetComponentsInChildren<ParticleSystem>();

        }
        void Update()
        {
            //if (_pivotRefTF != null)
            //{
            //    transform.rotation = _pivotRefTF.rotation;
            //    transform.position = _pivotRefTF.position;
            //    //print(transform.rotation);
            //}

            //if (Laser.material.HasProperty("_SpeedMainTexUVNoiseZW")) Laser.material.SetVector("_SpeedMainTexUVNoiseZW", LaserSpeed);
            //SetVector("_TilingMainTexUVNoiseZW", Length); - old code, _TilingMainTexUVNoiseZW no more exist
            _laser.material.SetTextureScale("_MainTex", new Vector2(_length[0], _length[1]));
            _laser.material.SetTextureScale("_Noise", new Vector2(_length[2], _length[3]));
            //To set LineRender position
            if (_laser != null && _updateSaver == false)
            {
                _laser.SetPosition(0, transform.position);
                RaycastHit hit; //DELETE THIS IF YOU WANT USE LASERS IN 2D
                                //ADD THIS IF YOU WANNT TO USE LASERS IN 2D: RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward, MaxLength);       
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, _maxLength, _layerMask))//CHANGE THIS IF YOU WANT TO USE LASERRS IN 2D: if (hit.collider != null)
                {

                    //End laser position if collides with object
                    _laser.SetPosition(1, hit.point);

                    _hitEffectGO.transform.position = hit.point + hit.normal * _hitOffset;
                    if (_useLaserRotation)
                        _hitEffectGO.transform.rotation = transform.rotation;
                    else
                        _hitEffectGO.transform.LookAt(hit.point + hit.normal);

                    foreach (var AllPs in _effectsPS)
                    {
                        if (!AllPs.isPlaying) AllPs.Play();
                    }
                    //Texture tiling
                    _length[0] = _mainTextureLength * (Vector3.Distance(transform.position, hit.point));
                    _length[2] = _noiseTextureLength * (Vector3.Distance(transform.position, hit.point));
                    //Texture speed balancer {DISABLED AFTER UPDATE}
                    //LaserSpeed[0] = (LaserStartSpeed[0] * 4) / (Vector3.Distance(transform.position, hit.point));
                    //LaserSpeed[2] = (LaserStartSpeed[2] * 4) / (Vector3.Distance(transform.position, hit.point));

                    IPlayer player = hit.transform.GetComponentInParent<IPlayer>();
                    if (player != null)
                    {
                        player.DamageReceived(_damage);
                        //_updateSaver = true;
                    }

                }
                else
                {
                    //End laser position if doesn't collide with object
                    var EndPos = transform.position + transform.forward * _maxLength;
                    _laser.SetPosition(1, EndPos);
                    _hitEffectGO.transform.position = EndPos;
                    foreach (var AllPs in _hitPS)
                    {
                        if (AllPs.isPlaying) AllPs.Stop();
                    }
                    //Texture tiling
                    _length[0] = _mainTextureLength * (Vector3.Distance(transform.position, EndPos));
                    _length[2] = _noiseTextureLength * (Vector3.Distance(transform.position, EndPos));
                    //LaserSpeed[0] = (LaserStartSpeed[0] * 4) / (Vector3.Distance(transform.position, EndPos)); {DISABLED AFTER UPDATE}
                    //LaserSpeed[2] = (LaserStartSpeed[2] * 4) / (Vector3.Distance(transform.position, EndPos)); {DISABLED AFTER UPDATE}
                }
                //Insurance against the appearance of a laser in the center of coordinates!
                if (_laser.enabled == false && _laserSaver == false)
                {
                    _laserSaver = true;
                    _laser.enabled = true;
                }
            }
        }
        public void Init(LaserData data, TargetType targetType)
        {
            _targetType = targetType;
            _damage = data.damage;

            if(_weaponFMOD.Length > 0)
            {
                aSoundInstance.release();
                aSoundInstance = RuntimeManager.CreateInstance(_weaponFMOD);
            }
        }
        public override void Activate()
        {
            _updateSaver = true;
            gameObject.SetActive(true);
            if (_laser != null)
            {
                _laser.enabled = false;
            }
            _length = new Vector4(1, 1, 1, 1);
            _hitEffectGO.transform.position = Vector2.one;
            //_cargandoLaser = false;

        }
        public override void Deactivate()
        {
            DisablePrepare();
            if (gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
                ReturnToPool();
            }
 
        }
        public override void Shoot()
        {
            if (_laser != null)
            {
                _laser.enabled = true;
            }
            _updateSaver = false;

            PlayWeaponSound();
        }

        //public override void SetParent(Transform pivot)
        //{
        //    _pivotRefTF = pivot;
        //}

        public override void ReturnToPool()
        {
            ObjectPoolLaserSmoke.Instance.ReturnToPool(this);
        }

        public override void DisablePrepare()
        {
            if (_laser != null)
            {
                if (_laser)
                    StopWeaponSound();

                _laser.enabled = false;
            }
            _updateSaver = true;
            //Effects can = null in multiply shooting
            if (_effectsPS != null)
            {
                foreach (var AllPs in _effectsPS)
                {
                    if (AllPs.isPlaying) AllPs.Stop();
                }
            }
            _hitEffectGO.transform.position = Vector2.one;
        }

        protected override void PlayWeaponSound()
        {
            if (aSoundInstance.isValid())
                aSoundInstance.start();
                aSoundInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, GetComponent<Rigidbody>()));
        }

        protected override void StopWeaponSound()
        {
            if (aSoundInstance.isValid())
                aSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                aSoundInstance.release();
        }
    }
}

