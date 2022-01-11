using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace PolloProyect
{
    public class GameOverManager : MonoBehaviour
    {
        [SerializeField] private Text _txtHighScore;
        [SerializeField] private Text _txtYourScore;
        [SerializeField] private GameObject _guardar;
        [SerializeField] private GameObject _resetNext;
        private Text _txtResetNext;
        //private PlayerData _playerData;
        private string _mensaje;
        void Start()
        {
            _txtResetNext = _resetNext.GetComponentInChildren<Text>();

            Cursor.visible = true;

            //_playerData = SaveSystem.LoadPlayer();

            //if (_playerData == null)
            //{
            //    _playerData = new PlayerData(200, 0, 0, 0);
            //}

            //_txtHighScore.text = _playerData.HighScore.ToString();
            //_txtYourScore.text = GameManager.Instancia.Score.ToString();

            //if (GameManager.Instancia.Score > _playerData.HighScore)
            //{
            //    _playerData.HighScore = GameManager.Instancia.Score;
            //    SaveSystem.SavePlayer(_playerData);
            //}

        }

        public void ReiniciarNext()
        {
            print("------------------------------");
            //GameManager.Instancia.IrUltimoNivel();
        }

        public void MainMenu()
        {
            //GameManager.Instancia.IrMainMenu();
        }

        public void Salir()
        {
            //GameManager.Instancia.Salir();
        }

        //public void Share()
        //{
        //    //_mensaje = "Por fin, alcance una puntuación de: " + GameManager.Instancia.Score.ToString();
        //    StartCoroutine(TakeScreenshotAndShare());
        //}

        //private IEnumerator TakeScreenshotAndShare()
        //{
        //    yield return new WaitForEndOfFrame();

        //    Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        //    ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        //    ss.Apply();

        //    string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        //    File.WriteAllBytes(filePath, ss.EncodeToPNG());

        //    // To avoid memory leaks
        //    Destroy(ss);

        //    new NativeShare().AddFile(filePath)
        //        .SetSubject("The Last Conquerors").SetText(_mensaje)
        //        .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
        //        .Share();

        //    // Share on WhatsApp only, if installed (Android only)
        //    //if( NativeShare.TargetExists( "com.whatsapp" ) )
        //    //	new NativeShare().AddFile( filePath ).AddTarget( "com.whatsapp" ).Share();
        //}
    }
}

