
using DG.Tweening;
using System;
using UnityEngine;

namespace PolloScripts.Interfaces
{
    public interface IPatternMovement
    {
        Sequence Movement(Transform parent, Action OnShoot, Action OnDeactivate, Action OnLookAt);

    }
}
