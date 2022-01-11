using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRocket : MonoBehaviour
{
    public GameObject rocketPrefab;
    public List<Transform> lugarDisparoList;
    //public bool activate;
    public float fireRate = 1;

    public bool activarOla;
    public float tiempoOla = 2f;
    void Start()
    {
           
    }
    public void Fire()
    {
        StartCoroutine(IenLaunch());
    }
    IEnumerator IenLaunch()
    {
        do
        {
            foreach (var item in lugarDisparoList)
            {
                Instantiate(rocketPrefab, item.position, item.rotation);
                yield return new WaitForSeconds(fireRate);
            }

            yield return new WaitForSeconds(tiempoOla);
        } while (activarOla);
       
    }
}
