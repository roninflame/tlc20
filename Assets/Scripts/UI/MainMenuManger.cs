using PolloScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PolloScripts.Enums;

public class MainMenuManger : MonoBehaviour
{
    public GameObject mainMenuGO;
    public GameObject levelMenuGO;
    public GameObject settingMenuGO;
    public GameObject settingMenuOptionsGO;
    public GameObject resolutionMenuGO;
    public GameObject languageMenuGO;
    public GameObject soundMenuGO;

    [Space]
    [Header("***** UI buttons *****")]
    [SerializeField] private GameObject _levelButtonGO;
    [SerializeField] private GameObject _levelBackGO;
    [SerializeField] private GameObject _settingBackGO;
    private GameObject _lastButtonGO;

    [Space]
    [Header("***** Quality *****")]
    [SerializeField] TMP_Dropdown _ddQuality;

    private bool buttonClicked;
    private bool levelClicked;
    List<Vector2> _resolutionScreen = new List<Vector2>();
    //public TMP_Dropdown resolutionDdn;

    Resolution[] _resolutions;
    private void OnEnable()
    {
        mainMenuGO.SetActive(true);
        levelMenuGO.SetActive(false);
        settingMenuGO.SetActive(false);
        resolutionMenuGO.SetActive(false);
        languageMenuGO.SetActive(false);
        settingMenuOptionsGO.SetActive(false);
        soundMenuGO.SetActive(false);

        levelClicked = false;
        buttonClicked = false;

        _ddQuality.value = QualitySettings.GetQualityLevel();
        //SetQuality(QualitySettings.GetQualityLevel());
       
    }
    private void Start()
    {
        _resolutionScreen.Add(new Vector2(1280, 720));
        _resolutionScreen.Add(new Vector2(1920, 1080));
        _resolutionScreen.Add(new Vector2(3840, 2160));

        _resolutions = Screen.resolutions;
        // resolutionDdn.ClearOptions();

        // List<string> options = new List<string>();

        // int currentResolutionIndex = 0;
        // for (int i = 0; i < _resolutions.Length; i++)
        // {

        //     string option = _resolutions[i].width + " x " + _resolutions[i].height;
        //     options.Add(option);

        //     if(_resolutions[i].width == Screen.currentResolution.width &&
        //         _resolutions[i].height == Screen.currentResolution.height)
        //     {
        //         currentResolutionIndex = i;
        //     }
        // }

        // resolutionDdn.AddOptions(options);
        // resolutionDdn.value = currentResolutionIndex;
        //// resolutionDdn.captionText.text = resolutionDdn.options[currentResolutionIndex].text;
        // resolutionDdn.RefreshShownValue();
    }
    public void SetFirstButtonMenu()
    {
        EventSystemManager.Instance.SetCurrentSelectedGameObject(_levelButtonGO);
        _lastButtonGO = _levelButtonGO;
    }
    public void OnControlsChanged()
    {
        if(GameManager.Instance.GetActualScene() == SceneIndexes.MAIN_MENU)
        {
            print("***** " + PlayerManager.Instance.playerController.CurrentControlScheme);
            if (PlayerManager.Instance.playerController.CurrentControlScheme == "Gamepad")
            {
                EventSystemManager.Instance.SetCurrentSelectedGameObject(_lastButtonGO == null?_levelButtonGO:_lastButtonGO);
                print("cambio");
            }
        }
 
    }
    public void ShowLevelMenu(GameObject menu)
    {
        if (!buttonClicked)
        {
            buttonClicked = true;
            levelMenuGO.SetActive(true);
            menu.SetActive(false);
            EventSystemManager.Instance.SetCurrentSelectedGameObject(_levelBackGO);
            _lastButtonGO = _levelBackGO;
        }
    }
    public void ShowSettings(GameObject menu)
    {
        if (!buttonClicked)
        {
            buttonClicked = true;
            settingMenuGO.SetActive(true);
            settingMenuOptionsGO.SetActive(true);
            menu.SetActive(false);
            EventSystemManager.Instance.SetCurrentSelectedGameObject(_settingBackGO);
            _lastButtonGO = _settingBackGO;
        }
    }

    public void ShowResolutions(GameObject menu)
    {
        resolutionMenuGO.SetActive(true);
        languageMenuGO.SetActive(false);
        soundMenuGO.SetActive(false);
    }
    public void ShowLanguages(GameObject menu)
    {
        languageMenuGO.SetActive(true);
        resolutionMenuGO.SetActive(false);
        soundMenuGO.SetActive(false);
    }
    public void ShowSound(GameObject menu)
    {
        soundMenuGO.SetActive(true);
        resolutionMenuGO.SetActive(false);
        languageMenuGO.SetActive(false);
    }
    public void SetResolution(int resolutionIndex)
    {
        for (int i = 0; i < _resolutions.Length; i++)
        {
            if (_resolutions[i].width == _resolutionScreen[resolutionIndex].x &&
                _resolutions[i].height == _resolutionScreen[resolutionIndex].y)
            {
                Screen.SetResolution((int)_resolutionScreen[resolutionIndex].x, (int)_resolutionScreen[resolutionIndex].y, Screen.fullScreen);
            }
        }
       
        //Resolution resolution = _resolutions[resolutionIndex];
        //Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void SetFullScreen(bool opcion)
    {
        Screen.fullScreen = opcion;
    }
    public void ShowMainMenu(GameObject menu)
    {

        buttonClicked = false;

        mainMenuGO.SetActive(true);
        menu.SetActive(false);
        EventSystemManager.Instance.SetCurrentSelectedGameObject(_levelButtonGO);
        _lastButtonGO = _levelButtonGO;
    }

 
    public void OnLevel1()
    {
        if (!levelClicked)
        {
            levelClicked = true;
            GameManager.Instance.GoToLevel01();
        }
      
    }
    public void OnLevel2()
    {
        if (!levelClicked)
        {
            levelClicked = true;
            GameManager.Instance.GoToLevel02();
        }

    }
    public void OnLevel3()
    {
        if (!levelClicked)
        {
            levelClicked = true;
            GameManager.Instance.GoToLevel03();
        }
       
    }
    public void NivelBoss_01()
    {

    }
    public void Salir()
    {
        Application.Quit();
    }
}
