using PolloScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolderFollowPlayer : MonoBehaviour
{
    public static HolderFollowPlayer Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if(PlayerManager.Instance != null)
        {
            transform.rotation = PlayerManager.Instance.playerHolder.transform.rotation;
            transform.position = PlayerManager.Instance.playerHolder.transform.position;
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
