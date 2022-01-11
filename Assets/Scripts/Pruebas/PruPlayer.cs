using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using PolloScripts;
using UnityEngine.InputSystem;

public class PruPlayer : MonoBehaviour
{
    //public Transform playerHolder;
    //public Transform enemyParent;
    void Start()
    {
        //AssetManager.Instance.CreateAssets();
        ////PlayerWeapons.Instance.CreateBasic();
        PlayerManager.Instance.player.Activate();
        PolloScripts.TargetSystem.TargetManager.Instance.Activate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (Time.timeScale == 1)
                Time.timeScale = 0.1f;
            else
                Time.timeScale = 1;
        }

        //if (enemyParent != null)
        //{
        //    enemyParent.rotation = playerHolder.rotation;
        //    enemyParent.position = playerHolder.position;
        //}
      
    }
}
