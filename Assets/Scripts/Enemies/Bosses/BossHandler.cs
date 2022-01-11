using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossHandler : MonoBehaviour
{
    public UnityEvent OnFireRocket;
    public UnityEvent OnStopRocket;

    public UnityEvent OnLoadLaser;

    public UnityEvent OnFireLaser;

    public void FireRocket()
    {
        OnFireRocket?.Invoke();
    }

    public void StopRocket()
    {
        OnStopRocket?.Invoke();
    }

    public void LoadLaser()
    {
        OnLoadLaser?.Invoke();
    }

    public void FireLaser()
    {
        OnFireLaser?.Invoke();
    }
}
