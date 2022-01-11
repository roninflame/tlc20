using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolloScripts.Enums;
using System;

namespace PolloScripts
{
    
    public abstract class LevelBase : MonoBehaviour
    {
        public GameObject Path;
        public GameObject PathCamera;
        public PatternManager Pattern;
        public int PatternIndex = -1;

        public void ActivatePathCamera(bool b)
        {
            if (PathCamera)PathCamera.SetActive(b);
        }
        public virtual void Initialize()
        {
            ActivatePathCamera(false);
        }

        public virtual void StartLevel()
        {

        }
        public virtual void CreateAssets()
        {

        }
    }
}
