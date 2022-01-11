

using DG.Tweening;
using System;
using UnityEngine;

namespace PolloScripts.Interfaces
{
    public interface ITrajectoryPattern
    {
        Sequence Move(Transform target, Vector3 destiny, Vector3 rotation, Action OnDeactivate);
    }
}
