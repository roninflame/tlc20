using PolloScripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts
{
    public class PatternData
    {
        //public AssetType assetType = AssetType.None;
        public EnemyType enemyTpe = EnemyType.None;
        public ItemType itemType = ItemType.None;
        public int typeID;
        public Vector3 Position;
        public Quaternion Rotation;
        public int trayectoria;

        public void SetData(ItemType asset, int typeID, Vector3 position, Quaternion rotation)
        {
            itemType = asset;
            this.typeID = typeID;
            Position = position;
            Rotation = rotation;
        }

        public void SetData(EnemyType asset, int typeID, Vector3 position, Quaternion rotation)
        {
            enemyTpe = asset;
            this.typeID = typeID;
            Position = position;
            Rotation = rotation;
        }

        public void SetData(ItemType asset, int typeID, int trayectoria, Vector3 position, Quaternion rotation)
        {
            itemType = asset;
            this.typeID = typeID;
            Position = position;
            Rotation = rotation;
            this.trayectoria = trayectoria;
        }
    }
}

