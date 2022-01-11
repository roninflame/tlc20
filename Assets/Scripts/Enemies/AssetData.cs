using PolloScripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Data
{
    public class AssetData : ScriptableObject
    {
        [Header("Parameters")]
        public AssetType asset;
        public int typeID;
        public float speed;
        public int health;
        public int damageValue;
        public int scoreValue;
        public float windSpeed;
        public float windStrength;

    }
}

