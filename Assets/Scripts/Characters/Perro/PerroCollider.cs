using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PolloProyect
{
    public class PerroCollider : MonoBehaviour
    {
        public delegate void Contact(Collider otro);
        public event Contact OnContact;

        private void OnTriggerEnter(Collider other)
        {
            OnContact(other);
        }
    }
}

