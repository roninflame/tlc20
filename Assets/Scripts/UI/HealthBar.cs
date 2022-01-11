using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PolloScripts.UI
{
    [Serializable]
    public class UIArmor
    {
        public GameObject normal;
        public GameObject damaged;
        //public Image Base;
        //public Sprite Normal;
        //public Sprite Cracked;
        public bool Active;
    }
    public class HealthBar : Bar
    {
        //public static HealthBar Instance;
        [Header("Heath Parameters")]
        [SerializeField] private List<UIArmor> armors;

        public void SetMaxValues()
        {
            foreach (var item in armors)
            {
                SetArmor(item, true);
            }
            currentValue = maxValue;
        }
        public override void SubtractValue(int newValue)
        {
            StopAllCoroutines();
            base.SubtractValue(newValue);
            int cont = 0;
            if (currentValue >= 0)
            {
                if (currentValue < 2)
                    OnImGoingToDie?.Invoke();

                for (int i = 0; i < maxValue; i++)
                {
                    if (armors[i].Active)
                    {
                        armors[i] = SetArmor(armors[i], false);
                        cont++;
                    }
                    if(cont >= newValue)
                    {
                        break;
                    }
                }
                StartCoroutine(IenCoolDown());
            }
        }
 
        public override void AddValue(int newValue)
        {
            //currentValue = Mathf.Clamp(currentValue + newValue, 0, maxValue);
            //SliderBar.value = currentValue;
            

            int cont = 0;
   
            if (currentValue < maxValue)
            {
                //StopAllCoroutines();
               
                for (int i = maxValue -1; i >= 0 ; i--)
                {
                    if (!armors[i].Active)
                    {
                        armors[i] = SetArmor(armors[i], true);
                        cont++;
                    }
                    if (cont >= newValue)
                    {
                        break;
                    }
                }
                currentValue = Mathf.Clamp(currentValue + newValue, 0, maxValue);
                //StartCoroutine(IenCountDown());
            }
            else
            {
                currentValue = Mathf.Clamp(currentValue, 0, maxValue);
            }

          
        }
        private UIArmor SetArmor(UIArmor armor, bool active)
        {
            //Color color = armor.Base.color;
            //armor.Base.color = new Color(color.r, color.g, color.b, active ? 1:0.2f);

            if (active)
            {
                armor.normal.SetActive(true);
                armor.damaged.SetActive(false);
            }
            else
            {
                armor.normal.SetActive(false);
                armor.damaged.SetActive(true);
            }

            armor.Active = active;
            return armor;
        }

        private IEnumerator IenCoolDown()
        {
            while (currentValue < maxValue)
            {
                yield return new WaitForSeconds(timeToConsume);
                AddValue(1);
            }

        }
    }
}
