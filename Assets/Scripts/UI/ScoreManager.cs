using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace  PolloScripts.UI{

    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private GameObject scoreBoardGO;
        [SerializeField] private TextMeshProUGUI _scoreValueTMP;
        [SerializeField] private TextMeshProUGUI _scoreValueTMP2;
        [SerializeField] private TextMeshProUGUI _highScoreValueTMP;
        public GameObject nextButtonGO;
        [Header("====== Testing ======")]
        [Space]
        public bool resetScoreData;
       
        private int _currentScore = 0;
        private const string _highScoreData = "highScore";
        private int _highScore = 0;

        private bool _nextPressed;

        private void Start() {
            if(resetScoreData)
                ResetScoreData();
        }
        public void ResetScoreValue(){
            _nextPressed = false;
            _scoreValueTMP.text = "0";
            _currentScore = 0;
        }

        public void AddScore(int value){
            
            _currentScore += value;
            _scoreValueTMP.text = _currentScore.ToString();
            _scoreValueTMP2.text = _currentScore.ToString();
        }
        public int GetCurrentScore(){
            return _currentScore;
        }
        public void LoadScore(){
            string nameData = _highScoreData + "_" + GameManager.Instance.CurrentScene.ToString();
            _highScore = PlayerPrefs.GetInt(nameData);
            _highScoreValueTMP.text = _highScore.ToString();
        }
        public void SaveScore(){
            string nameData = _highScoreData + "_" + GameManager.Instance.CurrentScene.ToString();
            if(_highScore<_currentScore){
                PlayerPrefs.SetInt(nameData, _currentScore);
                 _highScoreValueTMP.text = _currentScore.ToString();
            }
        }
        public void ResetScoreData(){
            PlayerPrefs.DeleteAll();
        }
        public void ShowScoreBoard(){
            scoreBoardGO.SetActive(true);
        }
        public void HideScoreBoard(){
            scoreBoardGO.SetActive(false);
        }
        public void GoToMainMenu(){
            if(!_nextPressed){
                _nextPressed = true;
                GameManager.Instance.GoToMainMenu();
            }
            
        }
    }
}
