using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PolloScripts.UI
{
    public class EnergyBar : Bar
    {
        //public static EnergyBar Instance;
        [Header("Energy Parameters")]
        //[SerializeField] private Slider sliderBar;
        [SerializeField] private TextMeshProUGUI tmpEnergy;
        //[SerializeField] private TMP_Text tmpEnergy;
        [SerializeField] private TextMeshProUGUI tmpEnergyPorc;
        private int currentPorc;
        //private void Awake()
        //{
        //    if (Instance == null)
        //        Instance = this;
        //    else
        //        Destroy(gameObject);
        //}
        public void SetMaxValues()
        {
            //sliderBar.maxValue = maxValue;
            //sliderBar.value = maxValue;
          
            currentValue = maxValue;
            SetTmpEnergy();
        }
        public override void SubtractValue(int newValue)
        {
            base.SubtractValue(newValue);

            SetTmpEnergy();
            //sliderBar.value = currentValue;
        }
        public override void AddValue(int newValue)
        {
            StopAllCoroutines();
            currentValue = Mathf.Clamp(currentValue + newValue, 0, maxValue);
            SetTmpEnergy();
            //sliderBar.value = currentValue;
            StartCoroutine(IenCountDown());
        }

        private void SetTmpEnergy()
        {
            currentPorc = currentValue * 100 / maxValue;
            tmpEnergy.text = currentPorc.ToString();

            if(currentPorc <= 15)
            {
                OnImGoingToDie?.Invoke();
            }
            else if(currentPorc >15 && currentPorc <= 30)
            {
                tmpEnergy.color = Color.red;
                tmpEnergyPorc. color = Color.red;
            }
            else if (currentPorc > 30 && currentPorc <= 50)
            {
                tmpEnergy.color = Color.yellow;
                tmpEnergyPorc.color = Color.yellow;
            }
            else if (currentPorc > 50)
            {
                tmpEnergy.color = Color.white;
                tmpEnergyPorc.color = Color.white;
            }
        }
    }

}
