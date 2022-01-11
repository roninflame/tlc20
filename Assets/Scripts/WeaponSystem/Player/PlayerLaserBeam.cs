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
    public class PlayerLaserBeam : Laser
    {
        //SHARE
        public bool CargandoLaser => _cargandoLaser;
        public int Damage => _damage;

        [Header("Laser")]
        [Space]

        [SerializeField] private Color32 _laserColor = new Color32(0, 0, 0, 0);
        [SerializeField] private GameObject _hitEffectGO;
        [SerializeField] private GameObject _flashEffectGO;

        [SerializeField] private float _laserScale = 6;
        [SerializeField] private float _hitOffset = 1f;
        [SerializeField] private bool _cargandoLaser = false;
        [SerializeField] private Vector3 _colliderScale = Vector3.one;

        //Handicap para mover el crosshair
        [Range(0f,1f)]
        public float handicapMoveCrosshair = 0.5f;


        //Particles System
        private ParticleSystem _laserPS;
        private ParticleSystem[] _flashPS;
        private ParticleSystem[] _hitPS;
        private ParticleSystem.Particle[] _particles;

        private Material _laserMat;
        private bool _updateSaver = false;

        private int _particleCount;

        private Vector3[] _particlesPositions;
        private bool _hitObject;
        private bool _startDissovle = false;
        private float _dissovleTimer = 0;

        [Header("**** Pruebas ****")]
        public bool activarLaser = false;
        private RaycastHit pruebahit;

        private void Awake()
        {

            _laserPS = GetComponent<ParticleSystem>();
            _laserMat = GetComponent<ParticleSystemRenderer>().material;
            _flashPS = _flashEffectGO.GetComponentsInChildren<ParticleSystem>();
            _hitPS = _hitEffectGO.GetComponentsInChildren<ParticleSystem>();
        }

        private void Start()
        {
            if (activarLaser)
            {
                Activate();
                Shoot();
            } 
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * pruebahit.distance);
            Gizmos.DrawWireCube(transform.position + transform.forward * pruebahit.distance, _colliderScale);
        }

        public void Init(LaserData data, TargetType targetType)
        {
            _targetType = targetType;
            _damage = data.damage;

            if (_weaponFMOD.Length > 0)
            {
                aSoundInstance.release();
                aSoundInstance = RuntimeManager.CreateInstance(_weaponFMOD);
            }
        }

        void Update()
        {
            foreach (var AllFlashes in _flashPS)
            {
                if (!AllFlashes.isPlaying) AllFlashes.Play();
            }

            if (CargandoLaser)
            {
                //foreach (var AllFlashes in _flashPS)
                //{
                //    if (!AllFlashes.isPlaying) AllFlashes.Play();
                //}
                return;
            }

        

            if (_laserPS != null && _updateSaver == false)
            {
                //Set start laser point
                _laserMat.SetVector("_StartPoint", transform.position);
                //Set end laser point

                RaycastHit hit;
                if (Physics.BoxCast(transform.position, _colliderScale, transform.forward, out hit, transform.rotation, _maxLength, _layerMask))
                {
                    pruebahit = hit;

                    _particleCount = Mathf.RoundToInt(hit.distance / (2 * _laserScale));
                    if (_particleCount < hit.distance / (2 * _laserScale))
                    {
                        _particleCount += 1;
                    }
                    _particlesPositions = new Vector3[_particleCount];
                    AddParticles();

                    _laserMat.SetFloat("_Distance", hit.distance);
                    _laserMat.SetVector("_EndPoint", hit.point);

                    if (_hitPS != null)
                    {
                        _hitEffectGO.transform.position = hit.point + hit.normal * _hitOffset;
                        _hitEffectGO.transform.LookAt(hit.point);
                        foreach (var AllHits in _hitPS)
                        {
                            if (!AllHits.isPlaying) AllHits.Play();
                        }
                        //foreach (var AllFlashes in _flashPS)
                        //{
                        //    if (!AllFlashes.isPlaying) AllFlashes.Play();
                        //}
                    }

                    if (DoDamage(hit.collider.gameObject))
                    {
                        //print("Player Laser: " + hit.collider.transform.parent.name);
                    }

                }
                else
                {
                    var EndPos = transform.position + transform.forward * _maxLength;
                    var distance = Vector3.Distance(EndPos, transform.position);
                    _particleCount = Mathf.RoundToInt(distance / (2 * _laserScale));
                    if (_particleCount < distance / (2 * _laserScale))
                    {
                        _particleCount += 1;
                    }
                    _particlesPositions = new Vector3[_particleCount];
                    AddParticles();

                    _laserMat.SetFloat("_Distance", distance);
                    _laserMat.SetVector("_EndPoint", EndPos);
                    if (_hitPS != null)
                    {
                        _hitEffectGO.transform.position = EndPos;
                        foreach (var AllPs in _hitPS)
                        {
                            if (AllPs.isPlaying) AllPs.Stop();
                        }
                    }
                }



            }
            if (_startDissovle)
            {
                _dissovleTimer += Time.deltaTime;
                _laserMat.SetFloat("_Dissolve", _dissovleTimer * 5);
            }
        }


        public override void Activate()
        {
            gameObject.SetActive(true);
            _laserMat.SetFloat("_Scale", this._laserScale);
            //_cargandoLaser = false;
            _updateSaver = true;
            _startDissovle = false;
            _hitObject = false;
            //_dirHitPlayer = transform.forward;
            _cargandoLaser = true;
            _laserMat.SetFloat("_Dissolve", 0f);
        }
        public override void Deactivate()
        {
            DisablePrepare();
            //gameObject.SetActive(false);
            if (gameObject.activeSelf)
                ReturnToPool();
        }
        public override void DisablePrepare()
        {
            StopWeaponSound();
            _dissovleTimer = 0;
            _startDissovle = true;
            _updateSaver = true;
            if (_flashPS != null && _hitPS != null)
            {
                foreach (var AllHits in _hitPS)
                {
                    if (AllHits.isPlaying) AllHits.Stop();
                }
                foreach (var AllFlashes in _flashPS)
                {
                    if (AllFlashes.isPlaying) AllFlashes.Stop();
                }
            }
            _laserMat.SetFloat("_Dissolve", 0f);
        }
        public override void Shoot()
        {
            PlayWeaponSound();
            _laserMat.SetFloat("_Scale", this._laserScale);
            _cargandoLaser = false;
            _updateSaver = false;
            _startDissovle = false;
            _hitObject = false;
        }

        private void AddParticles()
        {
            _particles = new ParticleSystem.Particle[_particleCount];

            for (int i = 0; i < _particleCount; i++)
            {
                _particlesPositions[i] = new Vector3(0f, 0f, 0f) + new Vector3(0f, 0f, i * 2 * _laserScale);
                _particles[i].position = _particlesPositions[i];
                _particles[i].startSize3D = new Vector3(0.001f, 0.001f, 2 * _laserScale);
                _particles[i].startColor = _laserColor;
            }
            _laserPS.SetParticles(_particles, _particles.Length);
        }

        public override void ReturnToPool()
        {
            ObjectPoolPlayerLaserBeam.Instance.ReturnToPool(this);
        }

        protected override void PlayWeaponSound()
        {
            if (aSoundInstance.isValid())
                aSoundInstance.start();
        }

        protected override void StopWeaponSound()
        {
            if (aSoundInstance.isValid())
                aSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }
}

