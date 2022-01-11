
namespace PolloScripts.Interfaces
{
    public interface IWeaponDamage
    {
        int Damage { get;}
        void Activate();
        void Deactivate();

        //void DoDamage(IPlayer player, int damage);
    }

}
