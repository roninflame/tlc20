using PolloScripts.Enums;
using PolloScripts.TargetSystem;
using PolloScripts.UI;
using System.Collections;
using UnityEngine;

namespace PolloScripts.Enemies
{
    public abstract class EnemyBase : MonoBehaviour
    {
        [Space]
        [Header("****** EnemyBase ******")]
        [SerializeField] protected GameObject _holderGO;
        [SerializeField] protected GameObject _modelGO;
        [SerializeField] protected int _scoreValue;

        
        [Space]
        [Header("Materials")]
        [ColorUsage(true, true)]
        [SerializeField] protected Color _colorHit = Color.red;
        [SerializeField] protected float _windSpeed;
        [SerializeField] protected float _windStrength;
        [SerializeField] protected Renderer[] _modelRenderer;

        [Space]
        [Header("Parameters")]
        [SerializeField] protected int _health;
        [SerializeField] protected int _damage;
        [SerializeField] protected float _damageCoolDown = 0.01f;
        [SerializeField] protected float _colorHitCoolDown = 0.3f;

        [Space]
        [Header("FMOD")]
        [FMODUnity.EventRef]
        [SerializeField] protected string _hitSound;


        protected Target _target;

        protected bool _isDead;
        protected bool _isTakingDamage;
        protected bool _canTakeDamage;

        protected Material[] _modelMaterial;
        [ColorUsage(true, true)]
        protected Color _colorBase = new Color(0, 0, 0, 0);

        //SHARE
        public AssetType AssetType => AssetType.Enemy;
        public bool CanTakeDamage => _canTakeDamage;
        public bool IsDead => _isDead;
        public int Health => _health;

        protected Coroutine _colorMaterialHitCor;
        protected virtual void Awake()
        {
            _canTakeDamage = false;
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

        public virtual void Activate()
        {
            gameObject.SetActive(true);

            if (_target == null)
                _target = new Target();
            
            _target?.ActivateTarget();
        }
        public virtual void Deactivate()
        {
            StopAllCoroutines();
            _colorMaterialHitCor = null;
            MaterialHit(false);
            _target?.DeactivateTarget();
        }
        public virtual void ShowModel(bool show)
        {
            _modelGO.SetActive(show);
        }
        public abstract void DamageReceived(int value);

        public virtual void Death()
        {
            _isDead = true;
        }

        protected void AddScore(){
            CanvasManager.Instance.OnAddScore?.Invoke(_scoreValue);
        }

        protected virtual void ResetMaterialHit()
        {
            for (int i = 0; i < _modelMaterial.Length; i++)
            {
                _modelMaterial[i].SetFloat("_UseWind", 1f);
                _modelMaterial[i].SetFloat("_WindSpeed", 0f);
                _modelMaterial[i].SetFloat("_WindStrength", 0f);
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
                    _modelMaterial[i].SetFloat("_WindSpeed", _windSpeed);
                    _modelMaterial[i].SetFloat("_WindStrength", _windStrength);
                }
            }
            else
            {
                for (int i = 0; i < _modelMaterial.Length; i++)
                {
                    _modelMaterial[i].SetColor("_BaseColor", (Vector4)_colorBase);
                    _modelMaterial[i].SetFloat("_WindSpeed", 0);
                    _modelMaterial[i].SetFloat("_WindStrength", 0);
                }
            }
        }
        protected abstract void HitSound();
        protected virtual IEnumerator IenDamageCoolDown()
        {
            _isTakingDamage = true;
            //MaterialHit(true);
            yield return new WaitForSeconds(_damageCoolDown);
            _isTakingDamage = false;
            //MaterialHit(false);
        }

        protected IEnumerator IenColorMaterialHit()
        {
            MaterialHit(true);
            yield return new WaitForSeconds(_damageCoolDown + _colorHitCoolDown);
            MaterialHit(false);
        }
    }

}
