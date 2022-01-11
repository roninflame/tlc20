using System;
using UnityEngine;
using DG.Tweening;

namespace PolloScripts.Interfaces
{
    public interface IRocketMovement
    {
        Sequence Move(Transform target, Vector3 destiny, Vector3 rotation, Action OnLineRendererON, Action OnLineRendererOFF, Action OnDeactivate);
    }

}

