using UnityEngine;

namespace PolloScripts.Data
{
    public abstract class WeaponData : ScriptableObject
    {
        public int typeID;
        public int damage = 1;
    }

}
