using DG.Tweening;
using PolloScripts.Data;
using PolloScripts.DialogueSystem;
using PolloScripts.Effects;
using PolloScripts.Enums;
using PolloScripts.Interfaces;
using PolloScripts.ObjectPoolSystem;
using PolloScripts.UI;
using PolloScripts.WeaponSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Enemies
{
    public class MiniBoss01 : SpaceShip
    {
        [Header("MiniBoss")]
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _timeToChangePos = 5f;
        [SerializeField] List<Vector3> _positionList;

        [Space]
        [Header("Weapon")]
        [SerializeField] private Transform _weaponSpot;
        [SerializeField] private Transform _weaponSpotL;
        [SerializeField] private Transform _weaponSpotR;
        [SerializeField] private Transform _weaponSpotC;


        [Space]
        [Header("Energy bullet")]
        [SerializeField] private int _ammoAmount;
        [SerializeField] private float _fireRate = 0.5f;
        [SerializeField] private float _timeToShooting_1 = 6f;

        [Space]
        [Header("Energy ball")]
        [SerializeField] private int _energyBallAmount;
        [SerializeField] private float _energyBallFireRate = 2f;
        [SerializeField] private float _timeToShooting_2 = 10f;

        [Space]
        [Header("WeaponData")]
        [SerializeField] private ProjectileData energyBulletData;
        [SerializeField] private ProjectileData energyBallData;

        [Space]
        [Header("End Level")]
        public ConversationIndex index;
        public GameObject tunnel;
        [SerializeField] private float _waitToEnd = 2;


        [Space]
        [Header("GameObject Reference")]
        public Transform ref1;
        public Transform ref2;

        //Local
        private bool _showModel;
        private float _distance;
        private bool _isShooting;
        private bool _canMove;

        private Vector3 _moveToPosition = new Vector3(10, 0, 200);
        public override SpaceCraftType SpaceCraftType => SpaceCraftType.MiniBoss1;

        private Vector3 _predictProjectilePosition;

        Sequence disparoIzq;
        Sequence disparoDer;
        Sequence seq;

        Coroutine corShoot;
        Coroutine corShoot2;
        //private void Start()
        //{
        //    Activate();

        //}

        protected void Update()
        {
            if (_isDead)
                return;

            if (_canTakeDamage && _target.TargetEnable)
            {
                _target.UpdateTarget(transform.position, gameObject);
            }

            //if (_canMove)
            //{
            //    _holderGO.transform.LookAt(PlayerManager.Instance.player.hitPosition);
            //    transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(
            //        _moveToPosition.x
            //        , _moveToPosition.y
            //        , transform.localPosition.z)
            //        , Time.deltaTime * _speed
            //        );
            //}

        }
        public void Init(EnemySpaceCraftData data, Transform parent, Vector3 position, Quaternion rotation)
        {
            if (parent == null)
            {
                transform.position = position;
                transform.rotation = rotation;
            }
            else
            {
                transform.parent = parent;
                transform.localPosition = position;
                transform.localRotation = rotation;
            }


            _health = data.health;
            _scoreValue = data.scoreValue;
            _damage = data.damage;
            //weapon
            _ammoAmount = data.ammoAmount;
            //_timeToShoot = data.timeToShoot;
            _fireRate = data.fireRate;

            _windSpeed = data.windSpeed;
            _windStrength = data.windStrength;
        }
        public override void Activate()
        {
            base.Activate();
            ShowModel(true);
            Move(null);
        }
        public override void Deactivate()
        {
            base.Deactivate();
            _showModel = false;
            DetenerShoot();
            seq.Kill();
            //ReturnToPool();
        }
        public override void Death()
        {
            base.Death();
            CanvasManager.Instance.OnSaveScore.Invoke();
            Deactivate();
            //Explosion
            StartCoroutine(IenDeath());
        }
        public override void ShowModel(bool show)
        {
            base.ShowModel(show);
            if (show)
            {
                _showModel = true;

            }
            _canTakeDamage = show;
        }
        public override void Move(IPatternMove pattern)
        {
            seq = DOTween.Sequence();

            Vector3 pos1 = new Vector3(-60, 0, 170);
            Vector3 pos2 = new Vector3(60, 0, -150);
            Vector3 pos3 = new Vector3(0, 80, -100);

            Vector3 rot1 = new Vector3(-10, 0, 0);
            Vector3 rot2 = new Vector3(10, 0, 0);

            float time1 = 5f;
            float time2 = 2f;
   

            float pathTime1 = 3f;
            float pathTime2 = 5f;
            float pathTime3 = 8f;
            float pathTime4 = 1f;

            Vector3[] localPath = null;

            localPath = new Vector3[3];
            localPath[0] = new Vector3(-25, -10, 150);
            localPath[1] = new Vector3(25, -10, 150);
            localPath[2] = new Vector3(60, 0, 150);

            Vector3[] localPath2 = null;

            localPath2 = new Vector3[6];
            localPath2[0] = new Vector3(-310, 0, 380);
            localPath2[1] = new Vector3(-230, 0, 454);
            localPath2[2] = new Vector3(-122, 0, 457);
            localPath2[3] = new Vector3(-37, 0, 406);
            localPath2[4] = new Vector3(-2, 0, 306);
            localPath2[5] = new Vector3(0, 0, 150);

            Vector3[] localPath3 = null;

            localPath3 = new Vector3[7];
            localPath3[0] = new Vector3(-30, -15, 150);
            localPath3[1] = new Vector3(-50, 0, 150);
            localPath3[2] = new Vector3(-30, 15, 150);
            localPath3[3] = new Vector3(30, -15, 150);
            localPath3[4] = new Vector3(50, 0, 150);
            localPath3[5] = new Vector3(30, 15, 150);
            localPath3[6] = new Vector3(0, 0, 150);

            Vector3[] localPath4 = null;

            localPath4 = new Vector3[2];
            localPath4[0] = new Vector3(-12, 40, 96);
            localPath4[1] = new Vector3(97, 150, 30);

            transform.localPosition = new Vector3(-150, 0, 0);

            seq.Append(transform.DOLocalRotate(new Vector3(0, 0, 0), 0));
            seq.Append(_modelGO.transform.DOLocalRotate(rot1, 0));
            seq.Append(transform.DOLocalMove(pos1, time1).SetEase(Ease.OutSine));
            seq.Append(transform.DOLocalMoveZ(150, 2).SetEase(Ease.InSine));

            seq.AppendCallback(Shoot);
            seq.Append(transform.DOLocalPath(localPath, pathTime1, PathType.CatmullRom, PathMode.Full3D).SetEase(Ease.InOutQuart).SetLoops(3, LoopType.Yoyo));
            seq.AppendCallback(DetenerShoot);

            seq.Append(transform.DOLocalMove(pos2, time2).SetEase(Ease.InCubic));
            seq.Append(transform.DOLocalMove(new Vector3(-330,0,350),0));
            seq.Append(_modelGO.transform.DOLocalRotate(rot2, 0));
            
            seq.Append(transform.DOLocalPath(localPath2, pathTime2, PathType.CatmullRom, PathMode.Full3D).SetLookAt(0.01f).SetEase(Ease.OutQuart).SetDelay(2));

            seq.AppendCallback(Shoot2);
            seq.Append(transform.DOLocalPath(localPath3, pathTime3, PathType.CatmullRom, PathMode.Full3D).SetEase(Ease.Linear).SetLoops(3, LoopType.Restart));
            seq.AppendCallback(DetenerShoot);

            seq.Append(transform.DOLocalPath(localPath4, pathTime4, PathType.CatmullRom, PathMode.Full3D).SetEase(Ease.InCubic).SetDelay(1));

            seq.SetLoops(-1, LoopType.Restart);

        }
        public override void Shoot()
        {
            _isShooting = true;
            if (gameObject.activeInHierarchy)
            {
                if(corShoot != null)
                {
                    StopCoroutine(corShoot);
                }

                corShoot = StartCoroutine(IenFire());
            }
        }

        public void Shoot2()
        {
            _isShooting = true;
            if (gameObject.activeInHierarchy)
            {
                if (corShoot2 != null)
                {
                    StopCoroutine(corShoot2);
                }

                corShoot2 = StartCoroutine(IenFire2());
            }
        }
        public void DetenerShoot()
        {
            _isShooting = false;
            if (corShoot != null)
            {
                StopCoroutine(corShoot);
            }
            DetenerDisparosLaterales();

            if (corShoot2 != null)
            {
                StopCoroutine(corShoot2);
            }
        }
        protected override void ReturnToPool()
        {
            //ObjectPoolSpaceCraft4.Instance.ReturnToPool(this);
        }

        IEnumerator IenFire()
        {
            IniciarDisparosLaterales();
            while (true)
            {
                yield return new WaitForSeconds(_timeToShooting_1);
              
                for (int i = 0; i < _ammoAmount; i++)
                {
                    Projectile weaponL;

                    //_weaponSpotL.LookAt(PlayerManager.Instance.player.hitPosition + (PlayerManager.Instance.player.transform.right * 3f));
                    _weaponSpotL.LookAt(ref2);
                    weaponL = ObjectPoolManager.Instance.ActivateMiniBossProjectile(ProjectileType.EnergyBullet, TargetType.Player, energyBulletData);
                    weaponL.SetTransform(null, _weaponSpotL.position, _weaponSpotL.rotation.eulerAngles);
                    weaponL.Activate();
                    weaponL.Shoot();

                    Projectile weaponR;
                    //_weaponSpotR.LookAt(PlayerManager.Instance.player.hitPosition + (-PlayerManager.Instance.player.transform.right * 3f));
                    _weaponSpotR.LookAt(ref1);
                    weaponR = ObjectPoolManager.Instance.ActivateMiniBossProjectile(ProjectileType.EnergyBullet, TargetType.Player, energyBulletData);
                    weaponR.SetTransform(null, _weaponSpotR.position, _weaponSpotR.rotation.eulerAngles);
                    weaponR.Activate();
                    weaponR.Shoot();

                    Projectile weaponC;
                    _weaponSpotC.LookAt(PlayerManager.Instance.player.hitPosition);
                    weaponC = ObjectPoolManager.Instance.ActivateMiniBossProjectile(ProjectileType.EnergyBullet, TargetType.Player, energyBulletData);
                    weaponC.SetTransform(null, _weaponSpotC.position, _weaponSpotC.rotation.eulerAngles);
                    weaponC.Activate();
                    weaponC.Shoot();

                    yield return new WaitForSeconds(_fireRate);
                }

          

                //yield return new WaitForSeconds(_timeToShooting_2);

                //for (int i = 0; i < _energyBallAmount; i++)
                //{
                //    Projectile weapon;
                //    Vector3 targetRot = _weaponSpot.rotation.eulerAngles;

                //    _weaponSpot.LookAt(PlayerManager.Instance.player.hitPosition);
                //    weapon = ObjectPoolManager.Instance.ActivateMiniBossProjectile(ProjectileType.EnergyBall, TargetType.Player, energyBallData);
                //    weapon.SetTransform(null, _weaponSpot.position, Quaternion.Euler(targetRot).eulerAngles);
                //    weapon.Activate();
                //    weapon.Shoot();

                //    for (int j = 0; j < 5; j++)
                //    {
                //        targetRot.y +=2;
                //        weapon = ObjectPoolManager.Instance.ActivateMiniBossProjectile(ProjectileType.EnergyBall, TargetType.Player, energyBallData);
                //        weapon.SetTransform(null, _weaponSpot.position, Quaternion.Euler(targetRot).eulerAngles);
                //        weapon.Activate();
                //        weapon.Shoot();
                //    }

                //    targetRot = _weaponSpot.rotation.eulerAngles;
                //    for (int j = 0; j < 5; j++)
                //    {
                //        targetRot.y -= 2;
                //        weapon = ObjectPoolManager.Instance.ActivateMiniBossProjectile(ProjectileType.EnergyBall, TargetType.Player, energyBallData);
                //        weapon.SetTransform(null, _weaponSpot.position, Quaternion.Euler(targetRot).eulerAngles);
                //        weapon.Activate();
                //        weapon.Shoot();
                //    }

                //    targetRot = _weaponSpot.rotation.eulerAngles;
                //    for (int j = 0; j < 5; j++)
                //    {
                //        targetRot.x += 2;
                //        weapon = ObjectPoolManager.Instance.ActivateMiniBossProjectile(ProjectileType.EnergyBall, TargetType.Player, energyBallData);
                //        weapon.SetTransform(null, _weaponSpot.position, Quaternion.Euler(targetRot).eulerAngles);
                //        weapon.Activate();
                //        weapon.Shoot();
                //    }

                //    targetRot = _weaponSpot.rotation.eulerAngles;
                //    for (int j = 0; j < 5; j++)
                //    {
                //        targetRot.x -= 2;
                //        weapon = ObjectPoolManager.Instance.ActivateMiniBossProjectile(ProjectileType.EnergyBall, TargetType.Player, energyBallData);
                //        weapon.SetTransform(null, _weaponSpot.position, Quaternion.Euler(targetRot).eulerAngles);
                //        weapon.Activate();
                //        weapon.Shoot();
                //    }

                //    yield return new WaitForSeconds(_energyBallFireRate);

                //}

            }
      
            
        }
        IEnumerator IenFire2()
        {
            while (true)
            {
                yield return new WaitForSeconds(_timeToShooting_2);

                for (int i = 0; i < _energyBallAmount; i++)
                {
                    Projectile weapon;
                    Vector3 targetRot;

                    _weaponSpot.LookAt(PlayerManager.Instance.player.hitPosition);
                    targetRot = _weaponSpot.rotation.eulerAngles;
                    weapon = ObjectPoolManager.Instance.ActivateMiniBossProjectile(ProjectileType.EnergyBall, TargetType.Player, energyBallData);
                    weapon.SetTransform(null, _weaponSpot.position,targetRot);
                    weapon.Activate();
                    weapon.Shoot();

                    targetRot = _weaponSpot.rotation.eulerAngles;
                    for (int j = 0; j < 5; j++)
                    {
                        targetRot.y += 2;
                        weapon = ObjectPoolManager.Instance.ActivateMiniBossProjectile(ProjectileType.EnergyBall, TargetType.Player, energyBallData);
                        weapon.SetTransform(null, _weaponSpot.position, targetRot);
                        weapon.Activate();
                        weapon.Shoot();
                    }

                    targetRot = _weaponSpot.rotation.eulerAngles;
                    for (int j = 0; j < 5; j++)
                    {
                        targetRot.y -= 2;
                        weapon = ObjectPoolManager.Instance.ActivateMiniBossProjectile(ProjectileType.EnergyBall, TargetType.Player, energyBallData);
                        weapon.SetTransform(null, _weaponSpot.position, targetRot);
                        weapon.Activate();
                        weapon.Shoot();
                    }

                    targetRot = _weaponSpot.rotation.eulerAngles;
                    for (int j = 0; j < 5; j++)
                    {
                        targetRot.x += 2;
                        weapon = ObjectPoolManager.Instance.ActivateMiniBossProjectile(ProjectileType.EnergyBall, TargetType.Player, energyBallData);
                        weapon.SetTransform(null, _weaponSpot.position, targetRot);
                        weapon.Activate();
                        weapon.Shoot();
                    }

                    targetRot = _weaponSpot.rotation.eulerAngles;
                    for (int j = 0; j < 5; j++)
                    {
                        targetRot.x -= 2;
                        weapon = ObjectPoolManager.Instance.ActivateMiniBossProjectile(ProjectileType.EnergyBall, TargetType.Player, energyBallData);
                        weapon.SetTransform(null, _weaponSpot.position, targetRot);
                        weapon.Activate();
                        weapon.Shoot();
                    }

                    yield return new WaitForSeconds(_energyBallFireRate);

                }

            }


        }
        IEnumerator IenCambiarPosicion()
        {
            while (true)
            {
                foreach (var item in _positionList)
                {
                    _moveToPosition = item;
                    yield return new WaitForSeconds(_timeToChangePos);
                }
            }
        }

        IEnumerator IenDeath()
        {
            ExplosionBase explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, transform.position, Quaternion.identity);
            explosion.ReSize(50);
            explosion.Activate();

            yield return new WaitForSeconds(0.2f);

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, transform.position + transform.right * 40, Quaternion.identity);
            explosion.ReSize(30);
            explosion.Activate();

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, transform.position + (-transform.right * 40), Quaternion.identity);
            explosion.ReSize(30);
            explosion.Activate();

            yield return new WaitForSeconds(0.2f);

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, transform.position + (transform.up * 30), Quaternion.identity);
            explosion.ReSize(30);
            explosion.Activate();

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, transform.position + (-transform.up * 30), Quaternion.identity);
            explosion.ReSize(30);
            explosion.Activate();

            //Segunda ronda

            yield return new WaitForSeconds(0.5f);

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, transform.position + transform.forward * 70, Quaternion.identity);
            explosion.ReSize(30);
            explosion.Activate();

            yield return new WaitForSeconds(0.2f);

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, transform.position + transform.forward * 70 + transform.right * 40, Quaternion.identity);
            explosion.ReSize(30);
            explosion.Activate();

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, transform.position + transform.forward * 70 + (-transform.right * 40), Quaternion.identity);
            explosion.ReSize(30);
            explosion.Activate();

            yield return new WaitForSeconds(0.2f);

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, transform.position + transform.forward * 70 + (transform.up * 30), Quaternion.identity);
            explosion.ReSize(30);
            explosion.Activate();

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, transform.position + transform.forward * 70 + (-transform.up * 30), Quaternion.identity);
            explosion.ReSize(30);
            explosion.Activate();

            //tercerta
            yield return new WaitForSeconds(0.2f);

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, transform.position + transform.forward * 80, Quaternion.identity);
            explosion.ReSize(30);
            explosion.Activate();

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, transform.position + transform.forward * 80 + (transform.right * 30), Quaternion.identity);
            explosion.ReSize(30);
            explosion.Activate();

            explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, transform.position + transform.forward * 80 + (-transform.right * 30), Quaternion.identity);
            explosion.ReSize(30);
            explosion.Activate();

            yield return new WaitForSeconds(0.1f);

            ShowModel(false);

            yield return new WaitForSeconds(2f);

            PlayerManager.Instance.player.Deactivate();

            StartCoroutine(IenEndLevel());
        }

        IEnumerator IenEndLevel()
        {
            DialogueManager dia = DialogueManager.instance;

            dia.StartDialogue(index);

            while (dia.InDialogue) { yield return null; }

            tunnel.SetActive(true);
            yield return new WaitForSeconds(_waitToEnd);
            
            GameManager.Instance.Death();

            //gameObject.SetActive(false);
        }

        private void IniciarDisparosLaterales()
        {
            disparoDer = DOTween.Sequence();
            disparoIzq = DOTween.Sequence();

            ref1.parent = transform.parent;
            ref2.parent = transform.parent;

            ref1.transform.localPosition = new Vector3(-15, 8, 0);
            ref2.transform.localPosition = new Vector3(15, 8, 0);

            float timeMov = 15;
            Vector3[] localPath = null;

            localPath = new Vector3[6];
            localPath[0] = new Vector3(-15, 8, 0);
            localPath[1] = new Vector3(-5, 8, 0);
            localPath[2] = new Vector3(-10, 5, 0);
            localPath[3] = new Vector3(-10, -5, 0);
            localPath[4] = new Vector3(-5, -8, 0);
            localPath[5] = new Vector3(-15, -8, 0);


            disparoDer.Append(ref1.DOLocalPath(localPath, timeMov, PathType.CatmullRom, PathMode.Full3D).SetEase(Ease.InSine).SetLoops(-1, LoopType.Yoyo));

            Vector3[] localPath2 = null;

            localPath2 = new Vector3[6];
            localPath2[0] = new Vector3(15, 8, 0);
            localPath2[1] = new Vector3(5, 8, 0);
            localPath2[2] = new Vector3(10, 5, 0);
            localPath2[3] = new Vector3(10, -5, 0);
            localPath2[4] = new Vector3(5, -8, 0);
            localPath2[5] = new Vector3(15, -8, 0);


            disparoIzq.Append(ref2.DOLocalPath(localPath2, timeMov, PathType.CatmullRom, PathMode.Full3D).SetEase(Ease.InSine).SetLoops(-1, LoopType.Yoyo));

        }

        private void DetenerDisparosLaterales()
        {
            disparoDer.Kill();
            disparoIzq.Kill();
        }

        protected override void HitSound()
        {
            //FMODUnity.RuntimeManager.PlayOneShot(_hitSound, transform.position);
        }

        public override void Movement(IPatternMovement pattern)
        {
            throw new System.NotImplementedException();
        }
    }
}

