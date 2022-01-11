using PolloScripts.Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class EnemyEvent : UnityEvent<GameObject> { }
public class EnemyScan : MonoBehaviour
{

    public EnemyEvent OnEnemyEvent;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.transform.GetComponentInParent<SpaceShip>())
        {
            //print(other.transform.parent.name);
            OnEnemyEvent?.Invoke(other.gameObject);
        }
    }

}
