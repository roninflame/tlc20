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
    public class LaserBeam : Laser
    {
        //SHARE
        public bool CargandoLaser => _cargandoLaser;
        public int Damage => _damage;

        [Header("Laser")]
        [Space]
      
        [SerializeField] private Color _laserColor = new Vector4(1, 1, 1, 1);
        [SerializeField] private GameObject _hitEffectGO;
        [SerializeField] private GameObject _flashEffectGO;
     
        [SerializeField] private float _laserScale = 6;
        [SerializeField] private float _hitOffset = 1f;
        [SerializeField] private bool _cargandoLaser = false;
        [SerializeField] private Vector3 _colliderScale = Vector3.one;



 
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

        private RaycastHit pruebahit;

        private void Awake()
        {

            _laserPS = GetComponent<ParticleSystem>();
            _laserMat = GetComponent<ParticleSystemRenderer>().material;
            _flashPS = _flashEffectGO.GetComponentsInChildren<ParticleSystem>();
            _hitPS = _hitEffectGO.GetComponentsInChildren<ParticleSystem>();
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

            aSoundInstance.release();
            aSoundInstance = RuntimeManager.CreateInstance(_weaponFMOD);
        }

        void Update()
        {
            //if(_pivotRefTF != null)
            //{
            //    transform.rotation = _pivotRefTF.rotation;
            //    transform.position = _pivotRefTF.position;
            //    //print(transform.rotation);
            //}

            if (CargandoLaser)
            {
                foreach (var AllFlashes in _flashPS)
                {
                    if (!AllFlashes.isPlaying) AllFlashes.Play();
                }
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

                    IPlayer player = hit.transform.GetComponentInParent<IPlayer>();

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
                        foreach (var AllFlashes in _flashPS)
                        {
                            if (!AllFlashes.isPlaying) AllFlashes.Play();
                        }
                    }

                    if (player.CanTakeDamage)
                    {
                        player.DamageReceived(_damage);
                        //OnContact();
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
            if(gameObject.activeSelf)
                ReturnToPool();
        }
        public override void DisablePrepare()
        {
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
            _laserMat.SetFloat("_Scale", this._laserScale);
            _cargandoLaser = false;
            _updateSaver = false;
            _startDissovle = false;
            _hitObject = false;
            //_dirHitPlayer = transform.forward;
        }

        //public override void SetParent(Transform pivot)
        //{
        //    _pivotRefTF = pivot;
        //}
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
            ObjectPoolLaserBeam.Instance.ReturnToPool(this);
        }

        protected override void PlayWeaponSound()
        {
            aSoundInstance.start();
            aSoundInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, GetComponent<Rigidbody>()));
        }

        protected override void StopWeaponSound()
        {
            aSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            aSoundInstance.release();
        }
    }
}

