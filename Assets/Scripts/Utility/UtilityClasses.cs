using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PolloScripts
{
    [Serializable]
    public class BoolEvent : UnityEvent<bool> { }
    [Serializable]
    public class IntEvent : UnityEvent<int> { }
    public class UtilityClasses : MonoBehaviour
    {
        public static Vector3 PredictProjectilePosition(Transform objectOwner, float fSpeed, float fCheckTime)
        {
            float fDistance = 9999;
            Vector3 v3TargetPosition = PlayerManager.Instance.player.objectTraker.GetProjectedPosition(fCheckTime);
            Debug.DrawLine(objectOwner.position, v3TargetPosition, Color.red, 1);

            //Predict projectile position
            Vector3 v3PredictedProjectilePosition = objectOwner.position + ((v3TargetPosition - objectOwner.position).normalized * fSpeed * fCheckTime);
            Debug.DrawLine(objectOwner.position, v3PredictedProjectilePosition, Color.blue, 2);
            fDistance = (v3TargetPosition - v3PredictedProjectilePosition).magnitude;

            if (fDistance > 5f)
            {
                //fCheckTime += fTimePerCheck;
                v3TargetPosition = PlayerManager.Instance.player.objectTraker.GetProjectedPosition(fCheckTime);
                //Debug.DrawLine(goTarget.transform.position, v3TargetPosition, Color.green, 3);

                v3PredictedProjectilePosition = objectOwner.position + ((v3TargetPosition - objectOwner.position).normalized * fSpeed * fCheckTime);
                Debug.DrawLine(objectOwner.position, v3PredictedProjectilePosition, Color.green, 3);
                fDistance = (v3TargetPosition - v3PredictedProjectilePosition).magnitude;
            }
            else
            {
                v3PredictedProjectilePosition = PlayerManager.Instance.player.hitPosition;
            }

            Vector3 v3Velocity = v3TargetPosition - objectOwner.position;

            return v3PredictedProjectilePosition;
            //bullet.Shoot(v3Velocity.normalized, fSpeed);
        }

        public static Vector3 PredictProjectilePosition(Transform objectOwner, Vector3 offset, float fSpeed, float fCheckTime)
        {
            float fDistance = 9999;
            Vector3 v3TargetPosition = PlayerManager.Instance.player.objectTraker.GetProjectedPosition(fCheckTime);
            Debug.DrawLine(objectOwner.position, v3TargetPosition, Color.red, 1);

            //Predict projectile position
            Vector3 v3PredictedProjectilePosition = objectOwner.position + ((v3TargetPosition - objectOwner.position).normalized * fSpeed * fCheckTime);
            Debug.DrawLine(objectOwner.position, v3PredictedProjectilePosition, Color.blue, 2);
            fDistance = (v3TargetPosition - v3PredictedProjectilePosition).magnitude;

            if (fDistance > 5f)
            {
                //fCheckTime += fTimePerCheck;
                v3TargetPosition = PlayerManager.Instance.player.objectTraker.GetProjectedPosition(fCheckTime);
                //Debug.DrawLine(goTarget.transform.position, v3TargetPosition, Color.green, 3);

                v3PredictedProjectilePosition = objectOwner.position + ((v3TargetPosition - objectOwner.position).normalized * fSpeed * fCheckTime);
                Debug.DrawLine(objectOwner.position, v3PredictedProjectilePosition, Color.green, 3);
                fDistance = (v3TargetPosition - v3PredictedProjectilePosition).magnitude;
            }
            else
            {
                v3PredictedProjectilePosition = PlayerManager.Instance.player.hitPosition;
            }

            Vector3 v3Velocity = v3TargetPosition - objectOwner.position;

            return v3PredictedProjectilePosition;
            //bullet.Shoot(v3Velocity.normalized, fSpeed);
        }
    }

}
