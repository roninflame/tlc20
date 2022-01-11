using PolloProyect;
using PolloScripts.Enemies;
using PolloScripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts
{
    public class DeactivateAssets : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            other.GetComponentInParent<IVisible>()?.Deactivate();
            other.GetComponentInParent<IProp>()?.Deactivate();

            //other.GetComponentInParent<AsteroidBase>()?.Deactivate();
            //if (other.GetComponentInParent<IVisible>() != null)
            //{
            //   print(other.transform.parent.name);

            //}
        }
    }

}
