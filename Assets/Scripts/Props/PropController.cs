using PolloScripts.Effects;
using PolloScripts.Enums;
using PolloScripts.Interfaces;
using PolloScripts.ObjectPoolSystem;
using PolloScripts.WeaponSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Props
{
    public class PropController : MonoBehaviour, IProp
    {

        [Space]
        [Header("****** Reference ******")]
        [Space]
        [SerializeField] protected GameObject _holderGO;
        [SerializeField] protected GameObject _modelGO;

        [Space]
        [Header("Materials")]
        [Space]
        //[SerializeField] protected Color32 _colorHit = Color.red;
        [ColorUsage(true, true)]
        public Color _colorHit = Color.red;
        [SerializeField] protected float _windSpeed;
        [SerializeField] protected float _windStrength;
        [Range(-2, 2)]
        //[SerializeField] private float  _albedoHSVvalue;
        [SerializeField] protected Renderer[] _modelRenderer;

        [Space]
        [Header("****** Rotation ******")]
        [Space]
        [SerializeField] private bool _randomRotation;
        [SerializeField] private DirectionType _directionRotation;
        [SerializeField] private float _rotationSpeed = 10;

        [Space]
        [Header("****** Movement ******")]
        [Space]
        [SerializeField] private float _forwardSpeed = 0;

        [Space]
        [Header("****** STAtS ******")]
        [Space]
        [SerializeField] private int _health;
        [SerializeField] private int _damage;
        [SerializeField] protected bool _canTakeDamage;
        [SerializeField] protected float _damageCoolDown = 0.01f;
        [SerializeField] protected float _colorHitCoolDown = 0.3f;

        private Vector3 _direction;
        private bool _isDeactivated;

     
        protected Material[] _modelMaterial;

        [ColorUsage(true, true)]
        private Color _colorBase = new Color(0, 0, 0, 0);
        protected bool _isTakingDamage;
        private Vector3 _explosionPos;
        private Coroutine _corHitColor;
        //private float _hitingTime = 0;
        //private bool hiting;
        //private float _cant;

        private void OnEnable()
        {

            SetDirection();
        }
        protected virtual void Awake()
        {
            //_canTakeDamage = false;
            if (_modelMaterial == null)
            {
                _modelMaterial = new Material[_modelRenderer.Length];
                for (int i = 0; i < _modelRenderer.Length; i++)
                {
                    _modelMaterial[i] = _modelRenderer[i].material;
                }
            }
            _colorBase = (Color)_modelMaterial[0].GetVector("_BaseColor");
            ResetMaterialHit();

        }   
        void Update()
        {
            if (_directionRotation != DirectionType.None)
            {
                _holderGO.transform.rotation *= Quaternion.AngleAxis(_rotationSpeed * Time.deltaTime, _direction);
            }

            transform.position += transform.position * Time.deltaTime * _forwardSpeed;

            //if (hiting)
            //{
            //    if (_cant < _hitingTime)
            //    {
            //        MaterialHit(true);
            //        _cant += Time.deltaTime;
            //    }
            //    else
            //    {
            //        _cant = 0;
            //        MaterialHit(false);
            //        hiting = false;
            //    }

            //}
        }

        private void SetDirection()
        {
            if (_randomRotation)
            {
                int namesCount = System.Enum.GetNames(typeof(DirectionType)).Length;
                _directionRotation = (DirectionType)Random.Range(1, namesCount);
            }

            switch (_directionRotation)
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

        public void Deactivate()
        {
            StopAllCoroutines();
            MaterialHit(false);
            _corHitColor = null;

            if (!_isDeactivated)
            {
                _isDeactivated = true;
                //_hitingTime = 0;
                //_cant = 0;
                MaterialHit(false);
                //hiting = false;
                _canTakeDamage = false;
                gameObject.SetActive(false);
                //StartCoroutine(IenDeactivate());
            }
        }
        public void Death()
        {

            Deactivate();
            //Explosion
            ExplosionBase explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, _explosionPos, Quaternion.identity);
            explosion.ReSize(20);
            explosion.Activate();
        }
        public void DamageReceived(int value)
        {
            if (!_isTakingDamage && _canTakeDamage)
            {
                int sum = _health - value;
                if (sum > 0)
                {
                    _health = sum;
                    if (_corHitColor == null)
                    {
                        _corHitColor = StartCoroutine(IenHitColor());
                    }
                    else
                    {
                        StopCoroutine(_corHitColor);
                        _corHitColor = StartCoroutine(IenHitColor());
                    }

                    StartCoroutine(IenDamageCoolDown());
                }
                else
                {
                    Death();
                }
            }
        }
        protected virtual void ResetMaterialHit()
        {
            for (int i = 0; i < _modelMaterial.Length; i++)
            {
                _modelMaterial[i].SetFloat("_UseWind", 1f);
                _modelMaterial[i].SetFloat("_WindSpeed", 0f);
                _modelMaterial[i].SetFloat("_WindStrength", 0f);
                _modelMaterial[i].SetFloat("_Alpha", 1f);
                //_modelMaterial[i].SetFloat("_albedo_val",0f);
                _modelMaterial[i].SetColor("_BaseColor", (Vector4)_colorBase);
            }
        }
        protected virtual void MaterialHit(bool hit)
        {
            if (hit)
            {
                for (int i = 0; i < _modelMaterial.Length; i++)
                {
                    _modelMaterial[i].SetColor("_BaseColor", (Vector4)_colorHit);
                    //_modelMaterial[i].SetFloat("_albedo_val", _albedoHSVvalue);
                    _modelMaterial[i].SetFloat("_WindSpeed", _windSpeed);
                    _modelMaterial[i].SetFloat("_WindStrength", _windStrength);
                }
            }
            else
            {
                for (int i = 0; i < _modelMaterial.Length; i++)
                {
                    _modelMaterial[i].SetColor("_BaseColor", (Vector4)_colorBase);
                    //_modelMaterial[i].SetFloat("_albedo_val", 0f);
                    _modelMaterial[i].SetFloat("_WindSpeed", 0);
                    _modelMaterial[i].SetFloat("_WindStrength", 0);
                }
            }
        }
        //IEnumerator IenDeactivate()
        //{
        //    yield return new WaitForSeconds(2f);
        //    gameObject.SetActive(false);
        //}

        protected IEnumerator IenDamageCoolDown()
        {
            //_hitingTime += + 0.05f;
            //hiting = true;
            
            _isTakingDamage = true;
            yield return new WaitForSeconds(_damageCoolDown);
            _isTakingDamage = false;
        }

        private IEnumerator IenHitColor()
        {
            MaterialHit(true);
            yield return new WaitForSeconds(_damageCoolDown + _colorHitCoolDown);
            MaterialHit(false);
        }
        private void OnCollisionEnter(Collision collision)
        {
            IPlayer player = collision.gameObject.GetComponentInParent<IPlayer>();
            if (player != null)
            {
                if (player.CanTakeDamage)
                {
                    player.DamageReceived(_damage);
                }
                _explosionPos = PlayerManager.Instance.player.hitPosition;
                Death();
            }
            else
            {
                WeaponBase weapon = collision.gameObject.GetComponentInParent<WeaponBase>();
                if(weapon != null)
                {
                    if(weapon.WeaponType == WeaponType.Projectile)
                    {
                        _explosionPos = collision.transform.position;
                        weapon.Deactivate();
                        DamageReceived(weapon.Damage);
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            IPlayer player = other.GetComponentInParent<IPlayer>();
            if (player != null)
            {
                if (player.CanTakeDamage)
                {
                    player.DamageReceived(_damage);
                }
                _explosionPos = PlayerManager.Instance.player.hitPosition + (PlayerManager.Instance.playerHolder.transform.forward * 20);
                Death();
            }
            else
            {
                WeaponBase weapon = other.GetComponentInParent<WeaponBase>();
                if (weapon != null)
                {
                    if (weapon.WeaponType == WeaponType.Projectile && weapon.TargetType == TargetType.Enemy)
                    {
                        _explosionPos = other.transform.position + (other.transform.forward * 10);
                        weapon.Deactivate();
                        DamageReceived(weapon.Damage);
                    }
                }
            }
        }
    }

}
