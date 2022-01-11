using PolloScripts;
using PolloScripts.Enemies.Bosses;
using PolloScripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBoss01 : MonoBehaviour
{
    public GameObject bossPref;
    public GameObject tunnel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanvasManager.Instance.energyBar.StopCountDown();
            GameObject go = Instantiate(bossPref, transform.position, Quaternion.identity);
            go.transform.parent = PlayerManager.Instance.enemyHolder;

            go.GetComponent<Boss01>().tunnel = tunnel;
            go.GetComponent<Boss01>().Activate();

        }
    }
}
