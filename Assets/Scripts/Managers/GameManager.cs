using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PolloScripts.Enums;
using PolloScripts.TargetSystem;
using PolloScripts.UI;
using UnityEngine.Rendering;

namespace PolloScripts
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager Instance => _instance;
        public RenderPipelineAsset mainMenuRP;
        public RenderPipelineAsset level01RP;
        public RenderPipelineAsset level02RP;
        public RenderPipelineAsset level03RP;
        public RenderPipelineAsset scoreBoardRP;

        [HideInInspector]
        public bool boostActivated;
        
        [Space]
        [Header("****** Testing *****")]
        [Space]
        public bool soundPerLetter;
        [Space]
        public bool skipDialogue;
        //SHARE
        public SceneIndexes CurrentScene => _currentScene;

 

        private static GameManager _instance;
        private List<AsyncOperation> _sceneLoading = new List<AsyncOperation>();
        private float _totalSceneProgress;

        private SceneIndexes _currentScene;
        private SceneIndexes _lastScene;
        private SceneIndexes _nextScene;

        void Start()
        {

            if (Instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            boostActivated = false;
            SetGameManager();
            GoToMainMenu();
        }

        void SetGameManager()
        {
            _currentScene = SceneIndexes.MANAGER;

            CameraController.Instance.ActivateLoadingCamera();
            CanvasManager.Instance.SetActiveUI(false);
            CanvasManager.Instance.SetActiveMainMenu(false);
            CanvasManager.Instance.SetActiveScoreBoard(false);
            LoadingCanvas.Instance.SetActive(false);
            PlayerManager.Instance.SetActivePlayerHolder(false);
        }

        public void Death()
        {
            //DETEBER EXPRESIONES
            //GoToMainMenu();
            GoToScoreBoard();
        }

        #region Loading Screen Events
        public void OnFadeInComplete(int fase)
        {
            if (fase == 1)
            {
                StartFadeOut();
                
            }
            else if(fase == 2)
            {
                StartCoroutine(IenSetNewScene());
            }
        }
        public void OnFadeOutComplete(int fase)
        {
            if (fase == 1)
            {
                StartingLoad(); 
            }
            else if (fase == 2)
            {
                //canvasManager.ActiveLoadingCanvas(false);
            }
        }

        #endregion

        #region Metodos para usar las escenas
        public void GoToScoreBoard(){
            StartFadeIn(SceneIndexes.GAME_OVER);
        }
        public void GoToMainMenu()
        {
            StartFadeIn(SceneIndexes.MAIN_MENU);
        }
      
        public void GoToLevel01()
        {
            StartFadeIn(SceneIndexes.LEVEL_01);
        }
        public void GoToLevel02()
        {
            StartFadeIn(SceneIndexes.LEVEL_02);
        }
        public void GoToLevel03()
        {
            StartFadeIn(SceneIndexes.LEVEL_03);
        }
        public void Salir()
        {
            Application.Quit();
        }
        public SceneIndexes GetActualScene()
        {
            return _currentScene;
        }
             
        #endregion

        #region Metodos para iniciar la carga de escenas

        //Primer facde In para iniciar la barra de carga
        private void StartFadeIn(SceneIndexes scene)
        {
            _nextScene = scene;
            LoadingCanvas.Instance.SetActive(true);
            LoadingCanvas.Instance.StartFadeIn(1);
        }
        //Primer Fade Out antes de cargar las escenas
        private void StartFadeOut()
        {
            CameraController.Instance.ActivateLoadingCamera();
            LoadingCanvas.Instance.StartFadeOut(1);
            CanvasManager.Instance.SetActiveUI(false);
            CanvasManager.Instance.SetActiveMainMenu(false);
            CanvasManager.Instance.SetActiveScoreBoard(false);
            PlayerManager.Instance.SetActivePlayerHolder(false);
        }
        //Segundo fade IN al terminar de cargar la escena
        public void StartFadeIn2()
        {
            LoadingCanvas.Instance.StartFadeIn(2);
        }
        //Segundo fade our luego de terminar de cargar la escena
        public void StartFadeOut2()
        {
            LoadingCanvas.Instance.StartFadeOut(2);
        }
       
        private void StartingLoad()
        {
            //Time.timeScale = 1f;
           
            if (_currentScene != SceneIndexes.MANAGER)
            {
                //Descargando la escena actual
                _sceneLoading.Add(SceneManager.UnloadSceneAsync((int)_currentScene));
            }
            //Cargando la siguiente escena
            _sceneLoading.Add(SceneManager.LoadSceneAsync((int)_nextScene, LoadSceneMode.Additive));
            //reemplazi la escena actual con la siguiente
            _currentScene = _nextScene;
            //Cortina para mostrar la carga de la escena
            StartCoroutine(GetSceneLoadProgress());
        }
        IEnumerator GetSceneLoadProgress()
        {
            LoadingCanvas.Instance.SetActive(true);

            for (int i = 0; i < _sceneLoading.Count; i++)
            {
                while (!_sceneLoading[i].isDone)
                {
                    _totalSceneProgress = 0;
                    foreach (AsyncOperation operation in _sceneLoading)
                    {
                        _totalSceneProgress += operation.progress;
                    }
                    _totalSceneProgress = (_totalSceneProgress / _sceneLoading.Count) * 100f;
                    //cargando la barra de carga
                    LoadingCanvas.Instance.SetLoadingBarValue(_totalSceneProgress);
                    yield return null;
                }
            }
            //Carga de escena
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)_currentScene));
            yield return new WaitForSeconds(0.1f);
            //Iniciar el fade IN de transicion
            StartFadeIn2();
        }

        #endregion

        #region Metodos para la nueva escena

        private IEnumerator IenSetNewScene()
        {
            print("*** Configurando < " + _currentScene.ToString() + " > ****");
            //Activo camara controller porque en MAIN_MENU se lo desactiva
            if (_currentScene != SceneIndexes.MAIN_MENU && _currentScene != SceneIndexes.GAME_OVER)
            {
                CameraController.Instance.SetActive(true);
            }


            if (_currentScene == SceneIndexes.MAIN_MENU)
            {
                GraphicsSettings.renderPipelineAsset = mainMenuRP;

                PlayerManager.Instance.playerController.EnableMenuControls();

                CameraController.Instance.CamerasOFF();
                //CameraController.Instance.SetActive(false);
                CanvasManager.Instance.SetActiveUI(false);
                CanvasManager.Instance.SetActiveMainMenu(true);
                CanvasManager.Instance.SelectFirstButtonMenu();
                TargetManager.Instance.Deactivate();
                PlayerManager.Instance.player.ResetPlayer();
            }
            else if (_currentScene == SceneIndexes.LEVEL_01 || _currentScene == SceneIndexes.LEVEL_02 || _currentScene == SceneIndexes.LEVEL_03)
            {
                
                if(_currentScene == SceneIndexes.LEVEL_01)
                    GraphicsSettings.renderPipelineAsset = level01RP;
                else if (_currentScene == SceneIndexes.LEVEL_02)
                    GraphicsSettings.renderPipelineAsset = level02RP;
                else if (_currentScene == SceneIndexes.LEVEL_03)
                    GraphicsSettings.renderPipelineAsset = level03RP;

                PlayerManager.Instance.SetPath(LevelManager.Instancia.Path);
                CameraController.Instance.ActivatePlayerCamera();
                CanvasManager.Instance.SetActiveMainMenu(false);
                CanvasManager.Instance.SetActiveUI(false);

                CanvasManager.Instance.uiExpressions.SetExpression(0);
                PlayerManager.Instance.SetActivePlayerHolder(true);
                PlayerManager.Instance.CreateWeapons();
                PlayerManager.Instance.player.ResetPlayer();
                
            }
            else if (_currentScene == SceneIndexes.GAME_OVER)
            {
                GraphicsSettings.renderPipelineAsset = scoreBoardRP;

                PlayerManager.Instance.playerController.EnableMenuControls();

                CameraController.Instance.CamerasOFF();
                //CameraController.Instance.SetActive(false);
                CanvasManager.Instance.SetActiveUI(false);
                CanvasManager.Instance.SetActiveMainMenu(false);
                CanvasManager.Instance.SetActiveScoreBoard(true);
                CanvasManager.Instance.SelectNextButton();
                TargetManager.Instance.Deactivate();
                PlayerManager.Instance.player.ResetPlayer();
            }
            
            yield return new WaitForSeconds(0.5f);
            StartFadeOut2();

        }
        #endregion

    }
}

