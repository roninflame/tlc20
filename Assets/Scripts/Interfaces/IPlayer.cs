using UnityEngine;

namespace PolloScripts.Interfaces
{
    public interface IPlayer
    {
        Vector3 hitPosition { get; }
        bool IsDeath { get; }
        bool CanTakeDamage { get; }
        void DamageReceived(int value);
        void Activate();

        void Deactivate();

        void Death();
    }

}
