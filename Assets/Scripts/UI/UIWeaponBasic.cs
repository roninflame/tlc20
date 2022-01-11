using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//**** YA NO SE USA
namespace PolloScripts.UI
{
    public class UIWeaponBasic : MonoBehaviour
    {
        //public static UIWeaponBasic Instance { get; private set; }

        [SerializeField] private Text _contadorText;
        [SerializeField] private Color32 _activeColor;
        [SerializeField] private Color32 _inactiveColor;
        [SerializeField] private Image[] _levelImageList;
        [SerializeField] private int _currentLevel;

        private int _currentLevelPoints;
        private int _levelPointsMax;

        //////private void Awake()
        //////{
        //////    if(Instance == null)
        //////    {
        //////        Instance = this;
        //////        ResetLevel();
        //////    }
        //////    else
        //////    {
        //////        Destroy(gameObject);
        //////    }
        //////}

        public void AddValue(int newValue)
        {
            _currentLevelPoints = Mathf.Clamp(_currentLevelPoints + newValue, 0, _levelPointsMax);

            if (_currentLevelPoints == _levelPointsMax && _currentLevel < 3)
            {
                _levelImageList[_currentLevel].color = _activeColor;
                _currentLevel++;
                _levelPointsMax++;
                _currentLevelPoints = 0;
                if (_currentLevel == 3)
                    _contadorText.text = "";
                else 
                    _contadorText.text = "0/" + _levelPointsMax;
            }
            else if (_currentLevelPoints < _levelPointsMax && _currentLevel < 3)
            {
                _contadorText.text = _currentLevelPoints + "/" + _levelPointsMax;
            }
           
        }

        public void ResetLevel()
        {
            for (int i = 0; i < _levelImageList.Length; i++)
            {
                _levelImageList[i].color = _inactiveColor;
            }
            _currentLevel = 0;
            _contadorText.text = "0/3";
            _levelPointsMax = 3;
            _currentLevelPoints = 0;
        }
    }
}

