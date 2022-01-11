using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PolloScripts
{
    public class EventSystemManager : MonoBehaviour
    {
        [Header("Component References")]
        public EventSystem eventSystem;

        public GameObject lastButton;
        public static EventSystemManager Instance => _instance;
     
        private static EventSystemManager _instance;
        private void Awake()
        {
            if(_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetCurrentSelectedGameObject(GameObject newSelectedGameObject)
        {
            eventSystem.SetSelectedGameObject(newSelectedGameObject);
            Button newSelectable = newSelectedGameObject.GetComponent<Button>();
            newSelectable.Select();
            newSelectable.OnSelect(null);
        }

        public void SetLastSelectedGameObject()
        {
            eventSystem.SetSelectedGameObject(lastButton);
            Button newSelectable = lastButton.GetComponent<Button>();
            newSelectable.Select();
            newSelectable.OnSelect(null);
        }
    }

}
