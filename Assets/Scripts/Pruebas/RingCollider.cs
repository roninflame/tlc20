using PolloScripts;
using PolloScripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IPlayer player = other.GetComponentInParent<IPlayer>();

        if(player != null)
        {
            other.GetComponentInParent<Player>().PruebasDano();
        }

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        IPlayer player = collision.transform.GetComponentInParent<IPlayer>();

        if (player != null)
        {
            collision.transform.GetComponentInParent<Player>().PruebasDano();
        }
    }

}
