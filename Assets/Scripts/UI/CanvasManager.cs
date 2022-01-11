using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PolloScripts.UI
{
    [Serializable] public class UnityEventInt : UnityEvent<int> { }
    public class CanvasManager : MonoBehaviour
    {
        public static CanvasManager Instance { get; private set; }
        [Header("===== UI CANVAS ====")]
        public Canvas uiCanvas;
        public EnergyBar energyBar;
        public HealthBar healthBar;
        public SlowMotionBar slowMotionBar;
        public UIWeaponSlots uiWeaponSlots;
        public UIExpressions uiExpressions;

        public ScoreManager scoreManager;

        [Space]
        [Header("===== Main Menu ====")]
        public Canvas mainMenuCanvas;

        [Space]
        [Header("===== Score  ====")]
        public UnityEventInt OnAddScore;
        public UnityEvent OnLoadScore;
        public UnityEvent OnSaveScore;
        public UnityEvent OnResetScoreValue;
        public UnityEvent OnResetScoreData;
        public int GetCurrentScore => scoreManager.GetCurrentScore();

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
  
            }
        }

        public void SetActiveUI(bool value)
        {
            uiCanvas.gameObject.SetActive(value);

            if (value)
            {
                uiExpressions.ShowBanner(false);
                uiExpressions.ShowBase(true);
            }
        }

        public void SetActiveMainMenu(bool value)
        {
            mainMenuCanvas.gameObject.SetActive(value);
        }
        public void SetActiveScoreBoard(bool value){
            if(value)
                scoreManager.ShowScoreBoard();
            else
                scoreManager.HideScoreBoard();
        }
        public void SelectFirstButtonMenu()
        {
            mainMenuCanvas.GetComponent<MainMenuManger>().SetFirstButtonMenu();
        }
        public void SelectNextButton()
        {
            EventSystemManager.Instance.SetCurrentSelectedGameObject(scoreManager.nextButtonGO);
        }
    }

}
