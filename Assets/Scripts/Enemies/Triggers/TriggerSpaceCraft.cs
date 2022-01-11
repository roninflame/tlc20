using PolloScripts.Enemies;
using PolloScripts.Enemies.Patterns;
using PolloScripts.Enums;
using PolloScripts.Interfaces;
using PolloScripts.ObjectPoolSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Triggers
{
    public class TriggerSpaceCraft : MonoBehaviour
    {
        [System.Serializable]
        public class SCList
        {
            [Space]
            public SC2 spaceCraft2;
            [Space]
            public SC3 spaceCraft3;
        }
        [System.Serializable]
        public class SC2
        {
            public bool activate;
            //public bool startInRighttSide;
            [Range(1, 2)]
            public int patronType = 1;
            [Range(1, 4)]
            public int spaceCraftInstances = 1;
        }
        [System.Serializable]
        public class SC3
        {
            public bool activate;
            [Range(1, 2)]
            public int patronType = 1;
            [Range(1, 2)]
            public int spaceCraftInstances = 1;
        }

        [SerializeField] private Transform _spawn;
        [Space]
        [Header("Parameters")]
        [Space]
        [SerializeField] private List<SCList> _spaceCraftList = new List<SCList>();

        private bool _contactPlayer;
        private List<SpaceShip> spaceShipList;
        private int countShip;
        private int _currentIndex;
        void Start()
        {
            _spawn.gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_contactPlayer)
            {
                if (other.CompareTag("Player"))
                {
                    _contactPlayer = true;
                    SpawnStarCrafts(0);
                }
            }
           
        }

        private void SpawnStarCrafts(int index)
        {
            float sumShip2 = 2;
            spaceShipList = new List<SpaceShip>();
            //sum += item.spaceCraft2.spaceCraftInstances;
            if (_spaceCraftList[index].spaceCraft2.activate)
            {
                //sum += 2;
                for (int i = 0; i < _spaceCraftList[index].spaceCraft2.spaceCraftInstances; i++)
                {
                    SpaceShip enemy = ObjectPoolManager.Instance.ActivateSpaceCraft(SpaceCraftType.SpaceCraft2, HolderFollowPlayer.Instance.transform, new Vector3(0, 50, 0), Quaternion.identity);
                    enemy.Activate();
                    if(_spaceCraftList[index].spaceCraft2.patronType == 1)
                    {
                        enemy.Move(new PatternMoveSpaceCraft2_A(i + 1));
                        sumShip2 += 0.5f;
                    }
                    else if (_spaceCraftList[index].spaceCraft2.patronType == 2)
                    {
                        enemy.Move(new PatternMoveSpaceCraft2_B(i + 1));
                        sumShip2 += 0.1f;
                    }

                    spaceShipList.Add(enemy);
                    //enemy.OnDeactivate += ShipDeactivated;
                }
            }

            if (_spaceCraftList[index].spaceCraft3.activate)
            {
                for (int i = 0; i < _spaceCraftList[index].spaceCraft3.spaceCraftInstances; i++)
                {
                    SpaceShip enemy = ObjectPoolManager.Instance.ActivateSpaceCraft(SpaceCraftType.SpaceCraft3, HolderFollowPlayer.Instance.transform, new Vector3(0, 50, 0), Quaternion.identity);
                    enemy.Activate();
                    int sinstances = _spaceCraftList[index].spaceCraft3.spaceCraftInstances;
                    if (_spaceCraftList[index].spaceCraft3.patronType == 1)
                    {
                  
                        enemy.Move(new PatternMoveSpaceCraft3_A(i, sumShip2));
                    }
                    else if (_spaceCraftList[index].spaceCraft3.patronType == 2)
                    {
                        enemy.Move(new PatternMoveSpaceCraft3_B((sinstances > 1)? i+1 : i, sumShip2));
                    }
                    spaceShipList.Add(enemy);
                    //enemy.OnDeactivate += ShipDeactivated;
                }
            }

            _currentIndex++;
        }

        private void ShipDeactivated(SpaceShip ship)
        {
            countShip++;
            if (spaceShipList.Count == countShip)
            {
                SpawnStarCrafts(_currentIndex);
                countShip = 0;
            }
            //ship.OnDeactivate -= ShipDeactivated;
        }

    }
}

