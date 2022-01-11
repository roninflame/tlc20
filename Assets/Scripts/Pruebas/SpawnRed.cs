using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRed : MonoBehaviour
{
    public GameObject redPrefab;
    //public float numeroRayos;
    public float speed;
    public bool ola;
    public float tiempoRep = 1;
    public float tiempoOla = 2;
    public List<Vector3> positionList;
    public void Fire()
    {
        StartCoroutine(IenFire());
    }
    
    IEnumerator IenFire()
    {
        do
        {
            for (int i = 0; i < positionList.Count; i++)
            {
                Red red = Instantiate(redPrefab, transform.position, transform.rotation).GetComponent<Red>();
                red.MoveTo(positionList[i],speed);
                yield return new WaitForSeconds(tiempoRep);

                Red red2 = Instantiate(redPrefab, transform.position, transform.rotation).GetComponent<Red>();
                red2.MoveTo(positionList[i],speed);
                yield return new WaitForSeconds(tiempoRep);
            }
            yield return new WaitForSeconds(tiempoOla);
        } while (ola);
    }
}
