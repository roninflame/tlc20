
using UnityEngine;
using PolloScripts.Enums;
using PolloScripts.Interfaces;
using PolloScripts.ObjectPoolSystem;


namespace PolloScripts.Enemies
{
    public abstract class AsteroidBase : EnemyBase, IVisible
    {


        [Space]
        [Header("****** Rotation ******")]
 
        [SerializeField] private bool _randomRotation;
        [SerializeField] private DirectionType _directionType;
        [SerializeField] private float _rotationTime;

        [Space]
        [Header("****** Movement ******")]
        [SerializeField] private float _speed;

        //SHARE
        public EnemyType EnemyType => EnemyType.Asteroid;
        public abstract AsteroidType TypeID { get; }

        private Vector3 _direction;
        protected void Update()
        {
            if (_isDead)
                return;

            if (_canTakeDamage && _target.TargetEnable)
            {
                _target.UpdateTarget(transform.position, gameObject);
            }

            if(_directionType != DirectionType.None)
            {
                _holderGO.transform.rotation *= Quaternion.AngleAxis(_rotationTime * Time.deltaTime, _direction);
            }

            transform.position += transform.position * Time.deltaTime * _speed;
        }

        public void Init(Data.EnemyAsteroidData data, Transform parent, Vector3 position, Quaternion rotation)
        {

            _health = data.health;
            _scoreValue = data.scoreValue;
            _damage = data.damage;
            _windSpeed = data.windSpeed;
            _windStrength = data.windStrength;

            if(parent != null)
            {
                transform.parent = parent;
                transform.localPosition = position;
                transform.localRotation = rotation;
            }
            else
            {
                transform.position = position;
                transform.rotation = rotation;
            }
            SetDirection();
        }

      

        private void SetDirection()
        {
            if (_randomRotation)
            {
                int namesCount = System.Enum.GetNames(typeof(DirectionType)).Length;
                _directionType = (DirectionType)Random.Range(1, namesCount);
            }

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
        private void OnTriggerEnter(Collider other)
        {
            ////Debug.Log("ast - " + other.name);
            IPlayer player = other.GetComponentInParent<IPlayer>();
            if (player != null)
            {
                player.DamageReceived(_damage);
                Death();
               
            }
        }

        #region EnemyBase

        public override void Activate()
        {
            base.Activate();
            ShowModel(false);
            _canTakeDamage = false;
            _isDead = false;
        }
        public override void Deactivate()
        {
            base.Deactivate();
            ResetMaterialHit();
            ReturnToPool();
        }
        
        public override void ShowModel(bool option)
        {
            base.ShowModel(option);
            _canTakeDamage = true;
        }
        public override void DamageReceived(int value)
        {
            if (!_isTakingDamage && _canTakeDamage)
            {
                int sum = _health - value;
                if (sum > 0)
                {
                    _health = sum;
                    StartCoroutine(IenDamageCoolDown());

                    if (_colorMaterialHitCor == null)
                    {
                        _colorMaterialHitCor = StartCoroutine(IenColorMaterialHit());
                    }
                    else
                    {
                        StopCoroutine(_colorMaterialHitCor);
                        _colorMaterialHitCor = StartCoroutine(IenColorMaterialHit());
                    }
                }
                else
                {
                    AddScore();
                    Death();
                }
            }
        }
        protected abstract void ReturnToPool();

        #endregion
    }
}

