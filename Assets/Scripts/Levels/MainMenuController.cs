using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts
{
    public class MainMenuController : MonoBehaviour
    {
        public GameObject consoleGO;
        public GameObject cameraMM;

        private void Awake()
        {
            cameraMM.SetActive(false);
            consoleGO.SetActive(false);
            CameraController.Instance.transform.position = cameraMM.transform.position;
        }
        void Start()
        {
            cameraMM.SetActive(true);
            consoleGO.SetActive(true);
        }

        private void OnDestroy()
        {
            CameraController.Instance.transform.position = Vector3.zero;
        }
    }

}
