using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using PolloScripts.FactorySystem;
using PolloScripts.ObjectPoolSystem;
using PolloScripts.Enemies;
using PolloScripts.Enums;
using PolloScripts.UI;
using PolloScripts.DialogueSystem;
using PolloScripts.Enemies.Patterns;

namespace PolloScripts
{
    public class BossManager : LevelBase
    {
        public static BossManager Instancia;
        public Transform spaceCraftTra;

        [SerializeField] private bool followPlayer;
        private void Awake()
        {
            if (Instancia == null)
                Instancia = this;
            else
                Destroy(this);
        }
        private void Start()
        {
            PatternIndex = 0;
            Initialize();
        }
        public override void Initialize()
        {
            base.Initialize();
            Pattern.Initialize();

            //FactoryManager.Instance.CreateAssets();
            //para las barras
            //UICanvas.Instance.SetActive(true);

            CanvasManager.Instance.energyBar.SetBar();
            CanvasManager.Instance.energyBar.ShowHolder(true);
            CanvasManager.Instance.healthBar.SetBar();
            CanvasManager.Instance.healthBar.ShowHolder(true);
            CanvasManager.Instance.energyBar.SetMaxValues();
            CanvasManager.Instance.healthBar.SetMaxValues();

            PlayerManager.Instance.playerWeapons.ResetSlots();
            CanvasManager.Instance.uiWeaponSlots.ResetSlots();



            StartCoroutine(IenStartLevel());

        }
        public override void StartLevel()
        {
            base.StartLevel();
        }
        public void ShowAssets(Transform parent)
        {
            foreach (var item in Pattern.LevelPatternData[PatternIndex])
            {
                //AssetManager.Instance.GetAsset(item.Asset, item.Type, parent, item.Position, item.Rotation);

                if (item.itemType != Enums.ItemType.None)
                {
                    switch (item.itemType)
                    {
                        case ItemType.None:
                            break;

                        case ItemType.Common:
                            ObjectPoolManager.Instance.ActivateItem(item.itemType, item.typeID, parent, item.Position, item.Rotation);
                            break;

                        case ItemType.Weapon:
                            ObjectPoolManager.Instance.ActivateWeaponItem((WeaponItemType)item.typeID, parent, item.Position, item.Rotation);
                            break;

                        default:
                            break;
                    }
                }
                else if (item.enemyTpe != Enums.EnemyType.None)
                {
                    ObjectPoolManager.Instance.ActivateEnemy(item.enemyTpe, item.typeID, parent, item.Position, item.Rotation);
                }
            }
            PatternIndex++;
        }
        public void ShowSpaceCraft(List<Triggers.TriggerSpaceCraft.SCList> spaceList)
        {
            StartCoroutine(IenShowSpaceCraft(spaceList));
        }
        IEnumerator IenShowSpaceCraft(List<Triggers.TriggerSpaceCraft.SCList> spaceList)
        {
            int sum = 1;
            foreach (Triggers.TriggerSpaceCraft.SCList item in spaceList)
            {
                sum += item.spaceCraft2.spaceCraftInstances;
                if (item.spaceCraft2.activate)
                {
                    sum += 2;
                    for (int i = 0; i < item.spaceCraft2.spaceCraftInstances; i++)
                    {
                        SpaceShip enemy = ObjectPoolManager.Instance.ActivateSpaceCraft(SpaceCraftType.SpaceCraft2, spaceCraftTra, new Vector3(0, 50, 0), Quaternion.identity);
                        enemy.Activate();
                        enemy.Move(new PatternMoveSpaceCraft2_A(i + 1));
                    }
                }

                if (item.spaceCraft3.activate)
                {
                    SpaceShip enemy = ObjectPoolManager.Instance.ActivateSpaceCraft(SpaceCraftType.SpaceCraft3, spaceCraftTra, new Vector3(0, 50, 0), Quaternion.identity);
                    enemy.Activate();
                    enemy.Move(new PatternMoveSpaceCraft3_A(1, sum));
                    //SpaceCraft sc = AssetManager.Instance.GetEnemy(EnemyType.SpaceCraft, 3, null, Vector3.zero, Quaternion.identity).GetComponent<SpaceCraft>();
                    //sc.Initialize(0, item.spaceCraft2.activate ? item.spaceCraft2.spaceCraftInstances : 0, false);
                }

                yield return new WaitForSeconds(sum);
            }

            yield return null;
        }
        IEnumerator IenStartLevel()
        {
            //yield return new WaitForSeconds(2f);

            //DialogueManager dia = DialogueManager.instance;

            //dia.StartDialogue(ConversationIndex.Lvl_01_B01);

            //while (dia.InDialogue) { yield return null; }

            yield return new WaitForSeconds(1);
            //CanvasManager.Instance.energyBar.StartCountDown();
            TargetSystem.TargetManager.Instance.Activate();
            followPlayer = true;

            //Player manager
            //PlayerManager.Instance.StartDollyCart();
            PlayerManager.Instance.player.Activate();
        }
    }
}
