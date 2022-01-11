using PolloScripts.DialogueSystem;
using PolloScripts.Interfaces;
using PolloScripts.TargetSystem;
using PolloScripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts
{
    public class EndLevel : MonoBehaviour
    {
        public ConversationIndex index;
        [SerializeField] private GameObject _tunnel;
        [SerializeField] private float _waitToEnd = 2;

        private bool _contact;
        private void Awake()
        {
            _tunnel.SetActive(false);
        }
        private void OnTriggerEnter(Collider other)
        {
            IPlayer player = other.GetComponentInParent<IPlayer>();

            if(player != null && !_contact)
            {
                _contact = true;
                player.Deactivate();
                //_tunnel.SetActive(true);
                StartCoroutine(IenEndLevel());
            }
        }

       
        IEnumerator IenEndLevel()
        {
            CanvasManager.Instance.SetActiveMainMenu(false);
            CanvasManager.Instance.SetActiveUI(false);
            PlayerManager.Instance.playerController.EnableMenuControls();
            
            CanvasManager.Instance.OnSaveScore.Invoke();

            DialogueManager dia = DialogueManager.instance;

            dia.StartDialogue(index);

            while (dia.InDialogue) { yield return null; }

            _tunnel.SetActive(true);

            PlayerManager.Instance.playerController.EnableGameplayControls();

            yield return new WaitForSeconds(_waitToEnd);
            GameManager.Instance.Death();
        }
    }

}
