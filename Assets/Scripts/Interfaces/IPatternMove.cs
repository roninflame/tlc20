
using DG.Tweening;
using System;
using UnityEngine;

namespace PolloScripts.Interfaces
{
    public interface IPatternMove
    {
        Sequence Move(Transform target,  Action OnShoot, Action OnDeactivate, Action OnLookAt);

    }
}
