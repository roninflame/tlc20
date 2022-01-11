using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Interfaces
{
    public interface IVisible
    {
        void Activate();
        void Deactivate();
        void ShowModel(bool option);
    }

}
