using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Character Data", menuName = "Data/Dialogue System/Character Data")]
    public class CharacterData : ScriptableObject
    {
        public List<CharacterInfoData> characterData;
        
    }

}
