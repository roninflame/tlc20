
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolloScripts.Enums;
using PolloScripts.Interfaces;

namespace PolloScripts
{
    [RequireComponent(typeof(UnityEngine.InputSystem.PlayerInput))]
    [RequireComponent(typeof(PlayerController))]
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance;

        [Header("Public Parameters")]
        public GameObject playerHolder;
        public Player player;
        public PlayerWeapons playerWeapons;
        public PlayerController playerController;
        public Transform enemyHolder;
        public float DollySpeed => _dollyCart.m_Speed;
        public bool DollyActivated => _dollyCart.m_Activate;
        //loca
        private CinemachineDollyCart _dollyCart;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _dollyCart = playerHolder.GetComponent<CinemachineDollyCart>();
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
        public void SetActivePlayerHolder(bool b)
        {
            playerHolder.SetActive(b);
        }
        public bool IsPlayerHolderActive()
        {
            return playerHolder.activeInHierarchy;
        }

        public void StartDollyCart()
        {
            _dollyCart.m_Activate = true;
        }

        public void StopDollyCart()
        {
            _dollyCart.m_Activate = false;
        }
        
        #region Iplayer
       
        #endregion

        #region Metodos PlayerController
        public void SetPath(GameObject path)
        {
            _dollyCart.m_Path = path.GetComponent<CinemachineSmoothPath>();
        }
        #endregion

        #region Metodos Player Weapons
        public void CreateWeapons()
        {
            //if(GameManager.Instance.CurrentScene == SceneIndexes.NIVEL_01)
            //{
            //    PlayerWeapons.Instance.CreateBasic();
            //}
        }
        
        #endregion

 
    }
}

