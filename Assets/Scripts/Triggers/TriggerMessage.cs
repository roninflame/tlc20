using PolloScripts.DialogueSystem;
using UnityEngine;

namespace PolloScripts
{
    public class TriggerMessage : MonoBehaviour
    {

        public ConversationsData messageData;
        private bool _triggered;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !_triggered)
            {
                _triggered = true;
                MessageBox.instance.StartMessage(messageData);
            }
        }
    }

}
