using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.UI
{
    
    public class UIWeaponSlots : MonoBehaviour
    {
        public UIWeaponBasic uiWeaponBasic;
        public List<UIWeaponSlot> weaponSlotList;
        public UIWeaponSlot selectedSlot;
        public int currentSlot = -1;

        [SerializeField] private Sprite _basicWeaponSprite;

        //Se resetean los slots UI pero sin contar con el primero que es para la arma basica
        public void ResetSlots()
        {
           
            for (int i = 0; i < weaponSlotList.Count; i++)
            {
                weaponSlotList[i].ResetSlot();
            }
            SetSlot(0, _basicWeaponSprite, 0, 0, 0);
            HighlightSlot(0);
        }
        public void SetSlot(int index, Sprite iconSprite, int arms, float duration, float cooldDown)
        {
            weaponSlotList[index].SetSlot(iconSprite, arms,duration,cooldDown, false);
        }

        public void SetSlot(int index, Sprite iconSprite, int arms, float duration, float cooldDown, bool showDuration)
        {
            weaponSlotList[index].SetSlot(iconSprite, arms, duration, cooldDown, showDuration);
        }

        public void SetArms(int index, int arms)
        {
            weaponSlotList[index].SetArms(arms);
        }
        public void ResetSlot(int index)
        {
            weaponSlotList[index].ResetSlot();
        }
        public void HighlightSlot(int index)
        {
            if (selectedSlot != null)
            {
                selectedSlot.Deselect();
                selectedSlot.SetDefaultColor();
            }

            selectedSlot = weaponSlotList[index];
            //selectedSlot.Select();

            selectedSlot.SetHighlightColor();

            currentSlot = index;           
        }
        public void SelectSlot (int index)
        {
            if (selectedSlot != null)
            {
                selectedSlot.Deselect();
                selectedSlot.SetDefaultColor();
            }

            selectedSlot = weaponSlotList[index];
            selectedSlot.Select();
            selectedSlot.SetSelectColor();

            currentSlot = index;
        }
        public void DeselectSlot()
        {
            if (selectedSlot != null)
            {
                selectedSlot.Deselect();
                selectedSlot.SetDefaultColor();
            }
            //selectedSlot = null;
            //currentSlot = -1;
        }
        public void SetDuration(int index, int value)
        {
            if (selectedSlot != null)
            {
                weaponSlotList[index].SetDuration(value);
            }

           
        }
    }
}

