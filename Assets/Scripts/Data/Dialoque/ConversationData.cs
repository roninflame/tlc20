using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Conversation Data", menuName = "Data/Dialogue System/Conversation")]

    public class ConversationData : ScriptableObject
    {
        public ConversationIndex conversationIndex;
        [TextArea(2, 2)]
        public string description = "Dialogue";

        public List<DialogueData> dialogueList;
    }


}

