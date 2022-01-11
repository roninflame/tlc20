using PolloScripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PolloScripts.UI
{
    public abstract class Bar : MonoBehaviour
    {
        [Header("Bar Parameters")]
        public BarType BarType;
        [SerializeField] protected int maxValue;
        [SerializeField] protected int valueToConsume;
        [SerializeField] protected float timeToConsume;
        [SerializeField] protected GameObject holder;
        protected int currentValue;
        protected bool active;

        public UnityEvent OnImGoingToDie;
        //Share
        public bool Active => active;
        public int CurrentValue => currentValue;

        public virtual void SetBar()
        {
            if(PlayerManager.Instance != null)
            {
                if (BarType == BarType.Health)
                    maxValue = PlayerManager.Instance.player.Health;
                else if (BarType == BarType.Energy)
                    maxValue = PlayerManager.Instance.player.Energy;
            }
            else
            {
                if (BarType == BarType.Health)
                    maxValue = 3;
                else if (BarType == BarType.Energy)
                    maxValue = 100;
            }
            currentValue = maxValue;
            active = false;
            //ShowHolder(false);
        }

        public void ShowHolder(bool b)
        {
            holder.SetActive(b);
        }

        public virtual void StartCountDown()
        {
            active = true;
            StopAllCoroutines();
            StartCoroutine(IenCountDown());
        }
        public virtual void StopCountDown()
        {
            active = false;
            StopAllCoroutines();
        }

        public virtual void SubtractValue(int newValue)
        {
            currentValue = Mathf.Clamp(currentValue - newValue, 0, maxValue);
            //SliderBar.value = currentValue;
        }
        public abstract void AddValue(int newValue);
     
           

        protected IEnumerator IenCountDown()
        {
            while (currentValue > 0)
            {
                yield return new WaitForSeconds(timeToConsume);
                SubtractValue(valueToConsume);

                if(BarType != BarType.SlowMotion)
                {
                    if (currentValue <= 0)
                    {
                        StopCountDown();
                        print("Bar : Player died");
                        PlayerManager.Instance.player.Death();
                        break;
                    }
                }
         
            }
        }
    }
}

