using PolloScripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts
{
    public class PatternManager : MonoBehaviour
    {
        [SerializeField] List<GameObject> levelPatterns;
        public List<List<PatternData>> LevelPatternData;
        public void Initialize()
        {
            LevelPatternData = new List<List<PatternData>>();

            AssetReference[] assets;

            for (int i = 0; i < levelPatterns.Count; i++)
            {
                assets = levelPatterns[i].GetComponentsInChildren<AssetReference>();
                AddPattern(assets);
            }
        }
        private void AddPattern(AssetReference[] assets)
        {
            List<PatternData> patterData = new List<PatternData>();
            PatternData data;
            if (assets != null)
            {
                for (int i = 0; i < assets.Length; i++)
                {
                    data = new PatternData();

                    switch (assets[i].Asset)
                    {
                        case AssetRefType.None:
                            break;
                        case AssetRefType.Item:
                            data.SetData(ItemType.Common, assets[i].Type, assets[i].GetPosition(), assets[i].GetRotation());

                            break;
                        case AssetRefType.Asteroid:
                            data.SetData(EnemyType.Asteroid, assets[i].Type, assets[i].GetPosition(), assets[i].GetRotation());

                            break;
                        case AssetRefType.SpaceCraft:
                            data.SetData(EnemyType.SpaceCraft, assets[i].Type, assets[i].GetPosition(), assets[i].GetRotation());
                            break;
                        case AssetRefType.WeaponItem:
                            data.SetData(ItemType.Weapon, assets[i].Type, assets[i].GetPosition(), assets[i].GetRotation());
                            break;
                        case AssetRefType.Cherub:
                            data.SetData(EnemyType.Cherub, assets[i].Type, assets[i].GetPosition(), assets[i].GetRotation());
                            break;
                        case AssetRefType.CharacterItem:
                            data.SetData(ItemType.Character, assets[i].Type, assets[i].trayectoria, assets[i].GetPosition(), assets[i].GetRotation());
                            break;

                    }

                   patterData.Add(data);
                }
            }
            LevelPatternData.Add(patterData);
        }
    }
}

