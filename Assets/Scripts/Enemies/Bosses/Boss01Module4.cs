using PolloScripts.Data;
using PolloScripts.Effects;
using PolloScripts.ObjectPoolSystem;
using PolloScripts.UI;
using PolloScripts.WeaponSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PolloScripts.Enemies.Bosses
{
    public class Boss01Module4 : BossBaseModule
    {
        public BossHealth bossHealth;
        public UnityEvent OnChangeLevel;
      
        public Transform bossTra;

        [Space]
        [Header("Weapon 5")]
        public BossWeapon05 weapon5;
        public List<BossWeaponData5> weapon05DataList;

        [Space]
        [Header("Weapon 6")]
        public List<BossWeapon06> weapon6List;
        public List<BossWeaponData5> weapon06DataList;

        [Space]
        [Header("Weapon 7")]
        public BossWeapon07 weapon7;
        public List<BossWeaponData2> weapon07DataList;

        [Space]
        [Header("Weapon 8")]
        public BossWeapon08 weapon8;
        public List<BossWeaponData2> weapon08DataList;

        public Animator animator;
        private int _rocketOnHash = Animator.StringToHash("RocketON");
        private int _rocketOffHash = Animator.StringToHash("RocketOFF");
        private int _fireLaserOnHash = Animator.StringToHash("LaserON");
        private int _fireLaserOffHash = Animator.StringToHash("LaserOFF");
        private int _damageHash = Animator.StringToHash("Damage");
        private int _stopHash = Animator.StringToHash("Stop");

        //para saber cuando usar patrones combinados
        private bool _activated;
        private bool _levelUP;
        private Coroutine _patter1Cor;
        //private void Update()
        //{
        //    if ((bossHealth.HealthPoints[0] / 2 > bossHealth.Health) && !_levelUP && bossHealth.CanTakeDamage)
        //    {
                
        //    }
        //}
        public void LevelUP()
        {
            _levelUP = true;
            StartCoroutine(IenRestartModule());
        }
        public override void OnDeath()
        {
            print("**** DEATH ****");
            AddScore();

            CanvasManager.Instance.OnSaveScore.Invoke();

            FireLaserOFF();
            RocketOFF();

            StopWeapons();

            StopCoroutine(_patter1Cor);

            animator.SetTrigger(_damageHash);

            bossHealth.Deactivate();

            OnChangeLevel.Invoke();
            //StartCoroutine(IenExplosions());

            //ObjectPoolManager.Instance.DeactivateBossProjectile(Enums.BossProjectileType.Rocket);
            //ObjectPoolManager.Instance.DeactivateBossProjectile(Enums.BossProjectileType.CrossLaser);
        }

        public override void OnLostHealth()
        {
            print("**** LOST HEALTH ****");
            _currentLevel++;
            StopWeapons();
            bossHealth.Stop();
 
        }

        public override void StartModule()
        {
            StartCoroutine(IenStartModule());
        }

        protected override void ActivateWeapons()
        {
            
        }

        protected override IEnumerator IenExplosions()
        {
            print("//// Explosions ////");
            foreach (var item in _explosionPointsList)
            {
                ExplosionBase explosion = ObjectPoolManager.Instance.ActivateExplosion(Enums.ExplosionType.Fire, null, item, Quaternion.identity);
                explosion.ReSize(20);
                explosion.Activate();
                yield return new WaitForSeconds(0.2f);
            }


            //OnChangeLevel.Invoke();
        }

        protected override void SetWeaponData(int level)
        {
            weapon5.Init(weapon05DataList[level]);

            foreach (var item in weapon6List)
            {
                item.Init(weapon06DataList[level]);
            }
            weapon7.Init(weapon07DataList[level]);

            //weapon8.Init(weapon08DataList[level]);
        }

        protected override void StopWeapons()
        {
            weapon5.Stop();
            foreach (var item in weapon6List)
            {
                item.Stop();
            }
            weapon7.Stop();
            //weapon8.Stop();
        }

        IEnumerator IenStartModule()
        {
            print("//// Start ////");
            _currentLevel = 0;
            
            weapon5.bossParent = bossTra.parent;
            foreach (var item in weapon6List)
            {
                item.bossParent = bossTra.parent;
            }
            
            yield return new WaitForSeconds(1f);
            bossHealth.Activate();
            SetWeaponData(_currentLevel);
            //ActivateWeapons();

            _patter1Cor = StartCoroutine(IenPatter1());

        }
        IEnumerator IenRestartModule()
        {
            print("//// ReStart ////");

            if(weapon7.IsFiring)
               FireLaserOFF();

            if (weapon5.IsFireing)
            {
                //weapon5.Pause();
                animator.SetTrigger(_stopHash);
                //RocketOFF();
            }
  
            bossHealth.Stop();

            StopWeapons();
 
            StopCoroutine(_patter1Cor);
            animator.SetTrigger(_damageHash);


            for (int i = 0; i < 3; i++)
            {
                ExplosionBase explosion = ObjectPoolManager.Instance.ActivateExplosion(Enums.ExplosionType.Fire, bossTra.parent, _explosionPointsList[i], Quaternion.identity);
                explosion.ReSize(20);
                explosion.Activate();
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(3f);

            bossHealth.ReStart();
            _currentLevel = 2;
            SetWeaponData(_currentLevel);
            _patter1Cor = StartCoroutine(IenPatter2());
        }

        IEnumerator IenPatter1()
        {
            ////PARTE 1
            RocketON();
            yield return new WaitForSeconds(8);
            RocketOFF();

            foreach (var item in weapon6List)
            {
                item.Activate();
            }
            yield return new WaitForSeconds(3);

            FireLaserON();
            yield return new WaitForSeconds(2);

            RocketON();
            yield return new WaitForSeconds(5);
            RocketOFF();

            yield return new WaitForSeconds(4);

            FireLaserOFF();

            _currentLevel = 1;
            SetWeaponData(_currentLevel);
            print("==== pattern 1 B =====");
            while (true)
            {
                RocketON();
                yield return new WaitForSeconds(8);
                RocketOFF();

                foreach (var item in weapon6List)
                {
                    item.Activate();
                }
                yield return new WaitForSeconds(4);

                FireLaserON();
                yield return new WaitForSeconds(2);

                RocketON();
                yield return new WaitForSeconds(6);
                RocketOFF();

                yield return new WaitForSeconds(4);

                FireLaserOFF();

                foreach (var item in weapon6List)
                {
                    item.Activate();
                }
                yield return new WaitForSeconds(2);
            }
        }

        IEnumerator IenPatter2()
        {
            print("==== pattern 2 =====");
            while (true)
            {
                RocketON();
                yield return new WaitForSeconds(8);
                RocketOFF();

                yield return new WaitForSeconds(2);

                foreach (var item in weapon6List)
                {
                    item.Activate();
                }

                yield return new WaitForSeconds(8);

                RocketON();
                yield return new WaitForSeconds(6);
               
                FireLaserON();
                yield return new WaitForSeconds(4);

                RocketOFF();
                yield return new WaitForSeconds(4);
            
                FireLaserOFF();

                yield return new WaitForSeconds(1);

                foreach (var item in weapon6List)
                {
                    item.Activate();
                }

                yield return new WaitForSeconds(6);
            }
        }

        private void RocketON()
        {
            animator.SetTrigger(_rocketOnHash);
        }
        private void RocketOFF()
        {
            animator.SetTrigger(_rocketOffHash);
            weapon5.Pause();
        }
        public void FireRocket()
        {
            if(!weapon5.IsFireing)
                weapon5.Fire();
        }

        public void StopRocket()
        {
            weapon5.StopFiring();
        }

        private void FireLaserON()
        {
            animator.SetTrigger(_fireLaserOnHash);
        }
        private void FireLaserOFF()
        {
            animator.SetTrigger(_fireLaserOffHash);
            weapon7.Stop();
        }

        public void FireLaser()
        {
            weapon7.Fire();
        }

        public void LoadLaser()
        {
            weapon7.Activate();
        }

    }
}

