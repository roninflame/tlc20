using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolloScripts.Enums;
using PolloScripts.WeaponSystem;
using PolloScripts.TargetSystem;
using PolloScripts.ObjectPoolSystem;
using System;
using PolloScripts.UI;
using UnityEngine.Events;
using System.Linq;
using PolloScripts.Enemies;

namespace PolloScripts
{
    [Serializable]
    public class WeaponSlot
    {
        //public int index;
        public bool isArmed;
        public WeaponType weaponType = WeaponType.None;
        public int weaponID = -1;
        public int arms = 0;

        public float duration;
        public float coolDown;
        public bool showDuration;
        public float fireRate;
        public Sprite iconSprite;
    }
    public class PlayerWeapons : MonoBehaviour
    {

        [SerializeField] Transform _shotSpot;
        [SerializeField] private float _fireRate;


        public float nextFire;
        [SerializeField] public List<WeaponSlot> weaponSlots = new List<WeaponSlot>();
        public WeaponSlot selectedWeapon;

        public bool specialWeaponActivated;

        private int selectedIndex;
        private int currenWeaponIndex = -1;
        public float handicapMoveCrosshair;
        public UnityEvent OnBlaster;

        [Header(" ***** Pruebas  *****")]
        public GameObject EnemyRadar;

        public void ActivateWeapon()
        {
            CanvasManager.Instance.uiWeaponSlots.SelectSlot(selectedIndex);
        }

        //se añaden las armas obtenidas desde item weapons
        public void AddWeapon(int weaponType, int weaponID, Sprite iconSprite, float duration, float coolDown, float wfireRate)
        {
            bool showDura = false;
            for (int i = 1; i < weaponSlots.Count; i++)
            {
                WeaponSlot slot = weaponSlots[i];

                showDura = ((WeaponType)weaponType == WeaponType.Projectile) ? true : false;
          

                if (slot.weaponType == (WeaponType)weaponType && slot.weaponID == weaponID)
                {
                    //actualizo armns
                    slot.arms++;
                    if (showDura)
                        slot.duration += duration;
                    else
                        slot.duration = duration;

                    CanvasManager.Instance.uiWeaponSlots.SetSlot(i, slot.iconSprite, slot.arms, slot.duration, coolDown, showDura);

                    //weaponSlots[i] = slot;
                    break;
                }
                else if(!slot.isArmed)
                {
                    //ACtivar el slot y actualizo arms
                    slot.isArmed = true;
                    slot.weaponType = (WeaponType)weaponType;
                    slot.weaponID = weaponID;
                    slot.arms++;

                    slot.showDuration = showDura;
                    if (showDura)
                        slot.duration += duration;
                    else
                        slot.duration = duration;

                    slot.fireRate = wfireRate;
                    slot.coolDown = coolDown;
                    slot.iconSprite = iconSprite;
                    CanvasManager.Instance.uiWeaponSlots.SetSlot(i, slot.iconSprite, slot.arms, duration, coolDown, showDura);

                    //weaponSlots[i] = slot;
                    break;
                }
            }
        }

        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawSphere(transform.position + transform.forward * 300, 150);
        //}

        public void Fire(float time)
        {
            switch (selectedWeapon.weaponType)
            {
                case WeaponType.None:
                    break;
                    
                case WeaponType.Projectile:
                    _shotSpot.LookAt(TargetManager.Instance.GetClosetTarget());

                    if (selectedWeapon.weaponID == 1)
                    {
                        //Basic
                       
                        if (time > nextFire)
                        {
                            _shotSpot.LookAt(TargetManager.Instance.GetClosetTarget());
                            Projectile currentWeapon = ObjectPoolManager.Instance.ActivatePlayerProjectile(PlayerProjectileType.Basic);
                            currentWeapon.SetTransform(null, _shotSpot.position, _shotSpot.rotation.eulerAngles);
                            currentWeapon.Activate();
                            currentWeapon.Shoot();
                            nextFire = time + _fireRate;
                        }
                    }
                    else if(selectedWeapon.weaponID == 2)
                    {
                        if (time > nextFire)
                        {
                            if (selectedWeapon.duration > 0)
                            {
                                if (!specialWeaponActivated)
                                    OnBlaster.Invoke();

                                EnemyRadar.SetActive(true);
                          
                                specialWeaponActivated = true;
                            }
                            else
                            {
                                EnemyRadar.SetActive(false);
                                RemoveWeapon();
                            }
                            nextFire = time + selectedWeapon.fireRate;
                        }
                        else
                        {
                            specialWeaponActivated = false;
                            EnemyRadar.SetActive(false);
                        }
                    }

                    break;
                case WeaponType.Laser:
                    //Laser
                    _shotSpot.LookAt(TargetManager.Instance.GetCenterTarget());
                    if (!specialWeaponActivated)
                    {
                        OnBlaster.Invoke();

                        PlayerLaserBeam laser = (PlayerLaserBeam)ObjectPoolManager.Instance.ActivatePlayerLaser((PlayerLaserType)selectedWeapon.weaponID, null, _shotSpot.position, _shotSpot.rotation);
                        laser.SetLaser(TargetType.Enemy, _shotSpot, Vector3.zero, Vector3.zero);
                        laser.Activate();
                        laser.Shoot();
                        handicapMoveCrosshair = laser.handicapMoveCrosshair;
                        ActivateWeapon();
                        selectedWeapon.arms--;
                        StartCoroutine(IenLaserInUse(laser));
                        specialWeaponActivated = true;
                       
                    }
                    break;
                case WeaponType.Missile:
      
                    break;
            }

        }

        public void OnEnemyScan(GameObject go)
        {
            PlayerEnemyChaser currentWeapon = (PlayerEnemyChaser)ObjectPoolManager.Instance.ActivatePlayerProjectile(PlayerProjectileType.EnemyChaser);
            currentWeapon.SetTransform(null, _shotSpot.position, _shotSpot.rotation.eulerAngles);
            currentWeapon.Activate();
            currentWeapon.FollowPlayer(go.transform);
            currentWeapon.Shoot();

            if(selectedWeapon != null)
                RemoveDuration();
        }

        #region Slots
        //Se resetean los slots pero la arma basic siempre activa
        public void ResetSlots()
        {
            for (int i = 0; i < weaponSlots.Count; i++)
            {
                if (i == 0)
                {
                    weaponSlots[i].arms = 0;
                    weaponSlots[i].isArmed = true;
                    weaponSlots[i].weaponID = 1;
                    weaponSlots[i].weaponType = WeaponType.Projectile;
                }
                else
                {
                    weaponSlots[i].arms = 0;
                    weaponSlots[i].isArmed = false;
                    weaponSlots[i].weaponID = -1;
                    weaponSlots[i].weaponType = WeaponType.None;
                    weaponSlots[i].iconSprite = null;
                }

            }
            //Se selecciona por default la arma basica
            selectedWeapon = weaponSlots[0];
            selectedIndex = 0;
            handicapMoveCrosshair = 0;
        }
        private void ResetSlot()
        {
            selectedWeapon.isArmed = false;
            selectedWeapon.weaponType = WeaponType.None;
            selectedWeapon.weaponID = -1;
            selectedWeapon.arms = 0;
            selectedWeapon.duration = 0;
            selectedWeapon.coolDown = 0;
            selectedWeapon.iconSprite = null;
            selectedWeapon.showDuration = false;
            handicapMoveCrosshair = 0;
        }

        private void ReorderSlots()
        {

            List<WeaponSlot> weaponActiveList = new List<WeaponSlot>();

            foreach (var item in weaponSlots)
            {
                if (item.isArmed)
                {
                    WeaponSlot slot = new WeaponSlot();
                    //ACtivar el slot y actualizo arms
                    slot.isArmed = true;
                    slot.weaponType = item.weaponType;
                    slot.weaponID = item.weaponID;
                    slot.arms = item.arms;
                    slot.duration = item.duration;
                    slot.coolDown = item.coolDown;
                    slot.iconSprite = item.iconSprite;
                    slot.showDuration = item.showDuration;
                    slot.fireRate = item.fireRate;
                    weaponActiveList.Add(slot);
                }
            }

            ResetSlots();
            CanvasManager.Instance.uiWeaponSlots.ResetSlots();

            for (int i = 1; i < weaponActiveList.Count; i++)
            {
                WeaponSlot slot = weaponActiveList[i];
                weaponSlots[i] = slot;
                CanvasManager.Instance.uiWeaponSlots.SetSlot(i, slot.iconSprite, slot.arms, slot.duration, slot.coolDown, slot.showDuration);
            }
        }
        public void RemoveWeapon()
        {
            if (selectedWeapon.arms == 0 || selectedWeapon.showDuration)
            {
                CanvasManager.Instance.uiWeaponSlots.ResetSlot(selectedIndex);
                ResetSlot();
                ReorderSlots();
            }
            else
            {
                CanvasManager.Instance.uiWeaponSlots.SetArms(selectedIndex, selectedWeapon.arms);
                CanvasManager.Instance.uiWeaponSlots.DeselectSlot();
            }

            //Siempre regresa a la arma basica
            specialWeaponActivated = false;
            SelectWeapon(0);
        }

        public void RemoveDuration()
        {
            selectedWeapon.duration--;
            CanvasManager.Instance.uiWeaponSlots.SetDuration(selectedIndex, (int)selectedWeapon.duration);
            
        }

        #endregion

        #region Controller Events
        public void SelectWeapon(int index)
        {
            if (specialWeaponActivated)
                return;

            for (int i = 0; i < weaponSlots.Count; i++)
            {
                WeaponSlot wSlot = weaponSlots[index];
                if (wSlot.isArmed)
                {
                    selectedIndex = index;
                    CanvasManager.Instance.uiWeaponSlots.HighlightSlot(index);
                    selectedWeapon = wSlot;
                }
                else if(selectedIndex != 0)
                {
                    CanvasManager.Instance.uiWeaponSlots.DeselectSlot();
                }
            }
        }
        #endregion

        #region Lasers
        private IEnumerator IenLaserInUse(Laser laser)
        {
            yield return new WaitForSeconds(selectedWeapon.duration);
            laser.Deactivate();
            handicapMoveCrosshair = 0;
            yield return null;
            RemoveWeapon();
        }
        #endregion

    }
}

