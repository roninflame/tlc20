using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Interfaces
{
    public interface ITakeDamage
    {
        bool CanTakeDamage { get; set; }
        void DamageReceived(int value);
    }
}
