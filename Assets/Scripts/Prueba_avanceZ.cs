using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prueba_avanceZ : MonoBehaviour
{
    public float Rapidez = -0.1f;
    void Update()
    {
        transform.position += transform.forward * Rapidez * Time.deltaTime;
    }
}
