using PolloScripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Interfaces
{
    public interface IWeapon
    {
        AssetType Asset { get; }
        int TypeID { get;}
        void Activate();
        void Deactivate();
        void Shoot();
    }

}
