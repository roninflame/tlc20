using PolloScripts;
using PolloScripts.DialogueSystem;
using PolloScripts.Enemies;
using PolloScripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMiniBoss01 : MonoBehaviour
{
    public GameObject miniBossPref;
    public GameObject tunnel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanvasManager.Instance.energyBar.StopCountDown();
            GameObject go = Instantiate(miniBossPref, transform.position, Quaternion.identity);
            go.transform.parent = PlayerManager.Instance.enemyHolder;

            go.GetComponent<MiniBoss01>().tunnel = this.tunnel;
            go.GetComponent<MiniBoss01>().Activate();
            PlayerManager.Instance.StopDollyCart();
        }
    }
}
