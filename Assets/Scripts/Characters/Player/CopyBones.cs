using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PolloScripts
{
    public class CopyBones : MonoBehaviour
    {
        public GameObject sourceObject;
        public GameObject sourceBone;
        public GameObject boneObject;
        public GameObject boneRoot;

        private Transform[] sourceBones;
        private Transform[] boneRoots;

        private void Start()
        {
            if(sourceBone && boneRoot)
            {
                Initialized();
            }
        }
        private void Update()
        {
            if (Keyboard.current.cKey.isPressed)
            {
                CopyObjectBones();
            }
        }
        public void Initialized()
        {
            sourceBones = sourceBone.GetComponentsInChildren<Transform>();
            boneRoots = boneRoot.GetComponentsInChildren<Transform>();

            Array.Sort(sourceBones, CompareName);
            Array.Sort(boneRoots, CompareName);
        }
        public void CopyObjectBones()
        {
            for (int i = 0; i < sourceBones.Length; i++)
            {
                boneRoots[i].position = sourceBones[i].position;
                boneRoots[i].rotation = sourceBones[i].rotation;
                //boneRoots[i].localScale = sourceBones[i].localScale;
            }
        }

        private int CompareName(Transform a, Transform b)
        {
            return a.name.CompareTo(b.name);
        }

    }

}
