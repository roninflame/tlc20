using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Conversations Data", menuName = "Data/Dialogue System/Conversations")]

    public class ConversationsData : ScriptableObject
    {
        public List<ConversationData> Converstion_Eng;

        public List<ConversationData> Converstion_Spa;
    }


}

