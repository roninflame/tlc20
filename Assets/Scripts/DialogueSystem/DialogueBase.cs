using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.DialogueSystem
{
    public class DialogueBase : MonoBehaviour
    {


    }

    #region Dialogue Data Detail
    [Serializable]
    public class DialogueData
    {
        public CharacterName characterName;
        [TextArea(4, 4)]
        public List<string> conversationBlock;
    }
    #endregion


    #region Emotion
    [Serializable]
    public class CharacterEmotion
    {
        [Serializable]
        public class CharacterEmotionDetail
        {
            public string emotion;
            public Sprite sprite;
        }

        public CharacterEmotionDetail[] emotions;

        ////================================================
        ////Public Variable
        ////================================================
        //private Dictionary<string, Sprite> _data;
        //public Dictionary<string, Sprite> Data
        //{
        //    get
        //    {
        //        if (_data == null) _init_emotionList();
        //        return _data;
        //    }
        //}

        //public string[] _emotion = new string[] { "Normal" };
        //public Sprite[] _sprite;

        ////================================================
        ////Private Method
        ////================================================
        //private void _init_emotionList()
        //{
        //    _data = new Dictionary<string, Sprite>();

        //    if (_emotion.Length != _sprite.Length)
        //        Debug.LogError("Emotion and Sprite have different lengths");

        //    for (int i = 0; i < _emotion.Length; i++)
        //        _data.Add(_emotion[i], _sprite[i]);
        //}
    }
    #endregion

    #region Audio
    [Serializable]
    public class CharacterAudio
    {
        [Space]
        public AudioClip[] voices;
        public AudioClip[] punctuations;
    }
    #endregion

    #region Enum
    public enum State
    {
        Active,
        Wait,
        Deactivate
    }
    public enum CharacterName
    {
        Broster,
        Coronel,
        Perro,
        Oso
    }
    public enum ConversationIndex
    {
        Lvl_01_B01,
        Lvl_01_F01,

        Lvl_02_B01,
        Lvl_02_F01,

        Lvl_03_B01,
        Lvl_03_F01,

    }
    #endregion
}
