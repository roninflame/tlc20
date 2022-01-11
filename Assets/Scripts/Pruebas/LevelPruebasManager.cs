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
    public class LevelPruebasManager : LevelBase
    {
        public static LevelPruebasManager Instancia;
        [Space]
        [Header("===== Lvl Pruebas =====")]

        public bool iniciarConsumoEnergia;
        public bool iniciarDollyCart;
        public bool puedeTomarDano;
        public bool fight;

        public Blanco blanco;

        //control de cherub movimientos
        private int _cherubCont;
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

            CanvasManager.Instance.slowMotionBar?.SetBar();
            CanvasManager.Instance.slowMotionBar?.ShowHolder(true);
            CanvasManager.Instance.slowMotionBar?.SetMaxValues();

            PlayerManager.Instance.playerWeapons.ResetSlots();
            CanvasManager.Instance.uiWeaponSlots.ResetSlots();
            //CanvasManager.Instance.uiWeaponSlots.uiWeaponBasic.ResetLevel();


            StartCoroutine(IenStartLevel());

        }
        private void OnDestroy()
        {
            //Transform[] go = PlayerManager.Instance.enemyHolder.GetComponentsInChildren<Transform>();

            for (int i = 0; i < PlayerManager.Instance.enemyHolder.childCount; i++)
            {
                Destroy(PlayerManager.Instance.enemyHolder.GetChild(i).gameObject);
            }

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

                        case ItemType.Character:
                            ObjectPoolManager.Instance.ActivateCharacterItem((CharacterItemType)item.typeID, item.trayectoria, parent, item.Position, item.Rotation);
                            break;

                        default:
                            break;
                    }
                }
                else if (item.enemyTpe != Enums.EnemyType.None)
                {
                    

                    if(item.enemyTpe == EnemyType.Cherub)
                    {
                        //Cherub1 cherub = (Cherub1)ObjectPoolManager.Instance.ActivateEnemy(item.enemyTpe, item.typeID, parent, item.Position, item.Rotation);
                        //if (!cherub.TurretMode)
                        //{
                        //    cherub.transform.parent = spaceCraftTra;
                        //    cherub.ShowModel(true);
                        //    cherub.Move(new SimplePatternMove(_cherubCont));
                        //    _cherubCont++;
                        //}
                       
    }
                    else
                    {
                        ObjectPoolManager.Instance.ActivateEnemy(item.enemyTpe, item.typeID, parent, item.Position, item.Rotation);
                    }
                }
            }
            PatternIndex++;
            _cherubCont = 0;
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
                        SpaceShip enemy = ObjectPoolManager.Instance.ActivateSpaceCraft(SpaceCraftType.SpaceCraft2, PlayerManager.Instance.playerHolder.transform, new Vector3(0, 50, 0), Quaternion.identity);
                        enemy.Activate();
                        enemy.Move(new PatternMoveSpaceCraft2_A(i + 1));
                    }
                }

                if (item.spaceCraft3.activate)
                {
                    SpaceShip enemy = ObjectPoolManager.Instance.ActivateSpaceCraft(SpaceCraftType.SpaceCraft3, PlayerManager.Instance.playerHolder.transform, new Vector3(0, 50, 0), Quaternion.identity);
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

            if (!fight)
            {
                if (iniciarConsumoEnergia)
                    CanvasManager.Instance.energyBar.StartCountDown();

                CanvasManager.Instance.slowMotionBar?.EnableSlowMotion();

                TargetSystem.TargetManager.Instance.Activate();

                if (puedeTomarDano)
                    PlayerManager.Instance.player.Activate();

                if (iniciarDollyCart)
                    PlayerManager.Instance.StartDollyCart();
            }
            else
            {
                blanco.Inicio();
            }
            
        }
    }
}
