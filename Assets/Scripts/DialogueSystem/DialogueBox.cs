using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PolloScripts.DialogueSystem
{
    public class DialogueBox : MonoBehaviour
    {
        public TMP_Animated animatedText;
        public TextMeshProUGUI nameTMP;
        public GameObject nextGO;

        public Image characterImg;

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public void ResetDialogueBox()
        {
            animatedText.text = "";
            nameTMP.text = "";
            if(nextGO != null)
                nextGO.SetActive(false);
            characterImg.gameObject.SetActive(false);
        }
        public void SetCharNameAndColor(CharacterInfoData data)
        {
            nameTMP.text = data.characterName.ToString();
            nameTMP.color = data.characterNameColor;
        }
        public void SetCharacterEmotion(Sprite spriteEmotion)
        {
            characterImg.sprite = spriteEmotion;
            characterImg.gameObject.SetActive(true);
        }
        public void NextActive(bool value)
        {
            if (nextGO != null)
                nextGO.SetActive(value);
        }
        public void ReadText(string words)
        {
            animatedText.ReadText(words);
        }
        public void StopReading()
        {
            animatedText.StopReading();
        }



    }

}
