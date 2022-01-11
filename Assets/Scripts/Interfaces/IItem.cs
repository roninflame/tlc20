
namespace PolloScripts.Interfaces
{
    public interface IItem
    {
        bool IsActive { get; }
        
        void Activate();
        void Deactivate();
    }

}
