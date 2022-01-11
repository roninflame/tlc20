using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolloScripts.Enums;
namespace PolloScripts
{
    public class AssetReference : MonoBehaviour
    {
        public AssetRefType Asset;
        public int Type;
        [Space]
        [Header("Caracter Item")]
        [Space]
        public int trayectoria = 0;
        public Vector3 GetPosition()
        {
            return transform.position;
        }
        public Quaternion GetRotation()
        {
            return transform.rotation;
        }
    }

}
