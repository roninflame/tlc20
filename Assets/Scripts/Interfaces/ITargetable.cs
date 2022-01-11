

using UnityEngine;
namespace PolloScripts.Interfaces
{
    public interface ITargetable
    {
        int Index { get; set; }
        Vector2 ScreenPosition { get; set; }
        float DistanceFromCenter { get; set; }
        //[HideInInspector] public TargetUI TargetSelected { get; set; }
        bool Activado { get; set; }
        bool EnRango { get; set; }
        [HideInInspector] float DistanceFromPlayer { get; set; }

        void UpdateTargetPosition();
        void ActivateTarget();
        void DeactivateTarget();

    }

}
