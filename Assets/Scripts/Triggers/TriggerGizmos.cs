using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGizmos : MonoBehaviour
{
    [ExecuteInEditMode]
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        //Draw the suspension
        Gizmos.DrawLine(
            Vector3.zero,
            Vector3.up
        );

        //draw force application point
        Gizmos.DrawWireSphere(transform.position, 10f);

        Gizmos.color = Color.green;
#if UNITY_EDITOR

#endif
    }
}
