
namespace PolloScripts.Interfaces
{
    public interface IEnemy
    {
        bool CanTakeDamage { get; }
        void DamageReceived(int value);
        void Activate();

        void Deactivate();

        void Death();
     
    }

}


