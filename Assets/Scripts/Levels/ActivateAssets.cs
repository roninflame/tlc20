using PolloScripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts
{
    public class ActivateAssets : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            other.GetComponentInParent<IVisible>()?.ShowModel(true);

            //if (other.GetComponentInParent<IVisible>() != null)
            //{
            //    print(other.transform.parent.name);
            //}
           
        }
    }

}
