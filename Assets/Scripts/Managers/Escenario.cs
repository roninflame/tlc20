using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PolloProyect
{
    public class Escenario : MonoBehaviour
    {
        public Vector3 Bounds;
        //public float Rapidez;
        public bool Moverse;
        //void Update()
        //{
        //    if(Moverse)
        //        transform.position += transform.forward * EscenarioManager.Instancia.RapidezActual * Time.deltaTime;
        //}
        public Vector3 GetPosicion()
        {
            return transform.position;
        }
        public void SetActive(bool b)
        {
            gameObject.SetActive(b);
        }
    }

   
}

