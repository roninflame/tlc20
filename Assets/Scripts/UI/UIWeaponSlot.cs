using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PolloScripts.UI
{
    public class UIWeaponSlot : MonoBehaviour
    {
        public Image backgroundImg;
        public Image iconImg;
        public TextMeshProUGUI armsText;
        public Sprite defaultIconSprite;
        public float duration;
        public float coolDown;
        public bool inUse;
        public bool showDuration;
        [SerializeField] private Color32 _highlightColor;
        [SerializeField] Color32 _selectedColor;
        [SerializeField] Color32 _defaultColor;
        public UnityEvent OnUsed;

        public UnityEvent OnSelected;
        public UnityEvent OnDeselected;

        public void ResetSlot()
        {
            iconImg.gameObject.SetActive(false);
            //iconImg.sprite = defaultIconSprite;
            armsText.text = "";
            backgroundImg.color = _defaultColor;
            inUse = false;
            duration = 0;
            coolDown = 0;
            showDuration = false;
        }

        public void SetSlot(Sprite sprite, int arms, float duration, float coolDown, bool showDuration)
        {
            iconImg.sprite = sprite;
            iconImg.gameObject.SetActive(true);

            if (arms == 0)
            {
                armsText.text = "";
            }
            else
            {
                if(showDuration)
                    armsText.text = duration.ToString(); //arms.ToString();
                else
                    armsText.text = arms.ToString();
            }

            this.showDuration = showDuration;
            this.duration = duration;
            this.coolDown = coolDown;
            //backgroundImg.color = selectedColor;
        }
        public void SetArms(int arms)
        {
            armsText.text = arms.ToString();
        }
        public void SetDuration(int value)
        {
            duration = value;
            armsText.text = duration.ToString();
        }
        public void SetHighlightColor()
        {
            backgroundImg.color = _highlightColor;
        }
        public void SetSelectColor()
        {
            backgroundImg.color = _selectedColor;
        }
        public void SetDefaultColor()
        {
            backgroundImg.color = _defaultColor;
        }
        public void Select()
        {
            OnSelected?.Invoke();
        }
        public void Deselect()
        {
            OnDeselected?.Invoke();
        }
    }

}
