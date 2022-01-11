using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Triggers
{
    public class TriggerAssets : MonoBehaviour
    {
        public Transform holder;
        public Transform spawn;
        void Start()
        {
            spawn.gameObject.SetActive(false);
            holder.position = spawn.position;
            holder.rotation = spawn.rotation;
        }

        private void OnTriggerEnter(Collider other)
        {
             if (other.CompareTag("Player"))
            {
                if (LevelManager.Instancia != null)
                    LevelManager.Instancia.ShowAssets(holder);
                else if(LevelPruebasManager.Instancia != null)
                    LevelPruebasManager.Instancia.ShowAssets(holder);
      
                    
            }
            //if(GameManager.Instancia.GetActiveCurrentScene() == Enums.E_SceneIndexes.NIVEL_01)
            //{

            //}
        }
    }
}


