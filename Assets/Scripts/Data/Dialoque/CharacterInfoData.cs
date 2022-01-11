using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Character info Data", menuName = "Data/Dialogue System/Character Info")]
    public class CharacterInfoData : ScriptableObject
    {
        public CharacterName characterName;
        public Color characterColor;
        public Color characterNameColor;

        [Space]
        [Header("***** Emotions *****")]
        public CharacterEmotion characterEmotion;

        //public CharacterAudio characterAudio;

        [Space]
        [Header("***** FMOD *****")]
        [FMODUnity.EventRef]
        public string voicesFMOD = "event:/Speach/Chicken/Speach";
        [Space]
        [FMODUnity.EventRef]
        public string punctuationsFMOD = "event:/Speach/Chicken/ChickenVoice";

        //[FMODUnity.EventRef]
        //public string punctuationsFMOD = "event:/Speach/Chicken/ChickenVoice";

        //public DialogueData dialogue;
    }
}