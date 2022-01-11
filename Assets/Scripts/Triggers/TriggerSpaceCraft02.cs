using DG.Tweening;
using PolloScripts.Enemies;
using PolloScripts.Enemies.Patterns;
using PolloScripts.Enums;
using PolloScripts.ObjectPoolSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Triggers
{
    public class TriggerSpaceCraft02 : MonoBehaviour
    {
        public GameObject cube;
        [Range(1, 11)]
        public int pattern = 1;
        [Range(1, 8)]
        public int enemies = 1;


        [Space]
        [Header("***** Pruebas *****")]
        [Space]
        public GameObject zeus;

        private bool _triggered;
        //private List<SpaceShip> _spaceShiptList;
        void Start()
        {
            cube.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_triggered)
            {
                if (other.CompareTag("Player"))
                {
                    _triggered = true;
                    SpawnSpaceCrafts();
                }
            }
        }

        private void SpawnSpaceCrafts()
        {
            Transform enemyParent = PlayerManager.Instance.enemyHolder;
            //_spaceShiptList = new List<SpaceShip>();
            for (int i = 0; i < enemies; i++)
            {
              
                if(pattern == 1)
                {
                    SpaceCraft6 enemy1 = (SpaceCraft6)ObjectPoolManager.Instance.ActivateSpaceCraft(SpaceCraftType.SpaceCraft6, enemyParent, new Vector3(0, 50, 0), Quaternion.identity);
                    enemy1.Activate();
                    enemy1.SetAttackPattern(ObjectPoolManager.Instance.GetAttackPattern(pattern - 1));
                    enemy1.Movement(new Pattern_M01(i + 1));
                }
                else if (pattern == 2)
                {
                    SpaceCraft6 enemy2 = (SpaceCraft6)ObjectPoolManager.Instance.ActivateSpaceCraft(SpaceCraftType.SpaceCraft6, enemyParent, new Vector3(0, 50, 0), Quaternion.identity);
                    enemy2.Activate();
                    enemy2.SetAttackPattern(ObjectPoolManager.Instance.GetAttackPattern(pattern - 1));
                    enemy2.Movement(new Pattern_M02(i + 1));
                }
                else if (pattern == 3)
                {
                    SpaceCraft7 enemy3 = (SpaceCraft7)ObjectPoolManager.Instance.ActivateSpaceCraft(SpaceCraftType.SpaceCraft7, enemyParent, new Vector3(0, 50, 0), Quaternion.identity);
                    enemy3.Activate();
                    enemy3.SetAttackPattern(ObjectPoolManager.Instance.GetAttackPattern(pattern - 1));
                    enemy3.Movement(new Pattern_M03(i + 1));
                }
                else if (pattern == 4)
                {
                    SpaceCraft7 enemy4 = (SpaceCraft7)ObjectPoolManager.Instance.ActivateSpaceCraft(SpaceCraftType.SpaceCraft7, enemyParent, new Vector3(0, 50, 0), Quaternion.identity);
                    enemy4.Activate();
                    enemy4.SetAttackPattern(ObjectPoolManager.Instance.GetAttackPattern(pattern - 1));
                    enemy4.Movement(new Pattern_M04(i + 1));
                }
                else if (pattern == 5)
                {
                    SpaceCraft2 enemy5 = (SpaceCraft2)ObjectPoolManager.Instance.ActivateSpaceCraft(SpaceCraftType.SpaceCraft2, enemyParent, new Vector3(0, 50, 0), Quaternion.identity);
                    enemy5.Activate();
                    enemy5.SetAttackPattern(ObjectPoolManager.Instance.GetAttackPattern(pattern - 1));
                    enemy5.Movement(new Pattern_M05(i + 1));
                }
                else if (pattern == 6)
                {
                    SpaceCraft2 enemy6 = (SpaceCraft2)ObjectPoolManager.Instance.ActivateSpaceCraft(SpaceCraftType.SpaceCraft2, enemyParent, new Vector3(0, 50, 0), Quaternion.identity);
                    enemy6.Activate();
                    enemy6.SetAttackPattern(ObjectPoolManager.Instance.GetAttackPattern(pattern - 1));
                    enemy6.Movement(new Pattern_M06(i + 1));
                }
                else if (pattern == 7)
                {
                    SpaceCraft2 enemy7 = (SpaceCraft2)ObjectPoolManager.Instance.ActivateSpaceCraft(SpaceCraftType.SpaceCraft2, enemyParent, new Vector3(0, 50, 0), Quaternion.identity);
                    enemy7.Activate();
                    enemy7.SetAttackPattern(ObjectPoolManager.Instance.GetAttackPattern(pattern - 1));

                    enemy7.Movement(new Pattern_M07(i + 1));
                }
                else if (pattern == 8)
                {
                    Cherub1 enemy8 = (Cherub1)ObjectPoolManager.Instance.ActivateEnemy(EnemyType.Cherub, 1,enemyParent, new Vector3(0, 50, 0), Quaternion.identity);
                    enemy8.Activate();
                    //enemy7.SetAttackPattern(ObjectPoolManager.Instance.GetAttackPattern(pattern - 1));

                    enemy8.Movement(new Pattern_M08(Mathf.Clamp( i + 1,1,5)));
                }
                else if (pattern == 9)
                {
                    SpaceCraft6 enemy2 = (SpaceCraft6)ObjectPoolManager.Instance.ActivateSpaceCraft(SpaceCraftType.SpaceCraft6, enemyParent, new Vector3(0, 50, 0), Quaternion.identity);
                    enemy2.Activate();
                    enemy2.SetAttackPattern(ObjectPoolManager.Instance.GetAttackPattern(pattern - 1));
                    enemy2.Movement(new Pattern_M09(i + 1));

                }
                else if (pattern == 10)
                {

                    Cherub1 enemy9 = (Cherub1)ObjectPoolManager.Instance.ActivateEnemy(EnemyType.Cherub, 1, enemyParent, new Vector3(0, 50, 0), Quaternion.identity);
                    enemy9.Activate();
                    //enemy7.SetAttackPattern(ObjectPoolManager.Instance.GetAttackPattern(pattern - 1));

                    enemy9.Movement(new Pattern_M07(Mathf.Clamp(i + 1, 1, 5)));
                }
                //_spaceShiptList.Add(enemy);
            }
            if (pattern == 11)
            {
                StartCoroutine(IenZeus());

            }
        }

        IEnumerator IenZeus()
        {
            Vector3[] localPath;
            float timeToMove1 = 7;
            float delayLaser = 1;
            float rotation = 160;
            Transform enemyParent = PlayerManager.Instance.enemyHolder;

            GameObject go = Instantiate(zeus, enemyParent.position, Quaternion.identity);
            go.transform.SetParent(enemyParent);
            go.transform.localPosition = new Vector3(-140, 120, 250);
            PruEnemy01 ene = go.GetComponent<PruEnemy01>();

            ene.direction = PruEnemy01.Direction.right;
            ene.laserDelay = delayLaser;
            ene.rotation = rotation;
           
            localPath = new Vector3[3];
            localPath[0] = new Vector3(-10, 50, 200);
            localPath[1] = new Vector3(-10, 26.5f, 100);
            localPath[2] = new Vector3(-30, 26.5f, -5);

            Tween t1 = go.transform.DOLocalPath(localPath, timeToMove1, PathType.CatmullRom, PathMode.Full3D).OnComplete(ene.Death).SetLookAt(0.01f).SetEase(Ease.InSine);

            yield return new WaitForSeconds(2);

            GameObject go2 = Instantiate(zeus, enemyParent.position, Quaternion.identity);
            go2.transform.SetParent(enemyParent);
            go2.transform.localPosition = new Vector3(140, 120, 250);
            PruEnemy01 ene2 = go2.GetComponent<PruEnemy01>();

            ene2.direction = PruEnemy01.Direction.left;
            ene2.laserDelay = delayLaser;
            ene2.rotation = rotation;

            localPath = new Vector3[3];
            localPath[0] = new Vector3(10, 50, 200);
            localPath[1] = new Vector3(10, 26.5f, 100);
            localPath[2] = new Vector3(30, 26.5f, -5);

            Tween t2 = go2.transform.DOLocalPath(localPath, timeToMove1, PathType.CatmullRom, PathMode.Full3D).OnComplete(ene2.Death).SetLookAt(0.01f).SetEase(Ease.InSine);

            yield return new WaitForSeconds(2);

            GameObject go3 = Instantiate(zeus, enemyParent.position, Quaternion.identity);
            go3.transform.SetParent(enemyParent);
            go3.transform.localPosition = new Vector3(-140, -120, 250);
            PruEnemy01 ene3 = go3.GetComponent<PruEnemy01>();

            ene3.direction = PruEnemy01.Direction.right;
            ene3.laserDelay = delayLaser;
            ene3.rotation = rotation;

            localPath = new Vector3[3];
            localPath[0] = new Vector3(-10, -50, 200);
            localPath[1] = new Vector3(-10, -16.5f, 100);
            localPath[2] = new Vector3(-30, -16.5f, -5);

            Tween t3 = go3.transform.DOLocalPath(localPath, timeToMove1, PathType.CatmullRom, PathMode.Full3D).OnComplete(ene3.Death).SetLookAt(0.01f).SetEase(Ease.InSine);

            yield return new WaitForSeconds(2);

            GameObject go4 = Instantiate(zeus, enemyParent.position, Quaternion.identity);
            go4.transform.SetParent(enemyParent);
            go4.transform.localPosition = new Vector3(140, -120, 250);
            PruEnemy01 ene4 = go4.GetComponent<PruEnemy01>();

            ene4.direction = PruEnemy01.Direction.left;
            ene4.laserDelay = delayLaser;
            ene4.rotation = rotation;

            localPath = new Vector3[3];
            localPath[0] = new Vector3(10, -50, 200);
            localPath[1] = new Vector3(10, -16.5f, 100);
            localPath[2] = new Vector3(30, -16.5f, -5);

            Tween t4 = go4.transform.DOLocalPath(localPath, timeToMove1, PathType.CatmullRom, PathMode.Full3D).OnComplete(ene4.Death).SetLookAt(0.01f).SetEase(Ease.InSine);

            yield return new WaitForSeconds(2);

            GameObject go5 = Instantiate(zeus, enemyParent.position, Quaternion.identity);
            go5.transform.SetParent(enemyParent);
            go5.transform.localPosition = new Vector3(0, 120, 250);
            PruEnemy01 ene5 = go5.GetComponent<PruEnemy01>();

            ene5.direction = PruEnemy01.Direction.down;
            ene5.laserDelay = delayLaser;
            ene5.rotation = rotation;

            localPath = new Vector3[3];
            localPath[0] = new Vector3(0, 50, 200);
            localPath[1] = new Vector3(0, 20, 100);
            localPath[2] = new Vector3(0, -40, -5);

            Tween t5 = go5.transform.DOLocalPath(localPath, timeToMove1, PathType.CatmullRom, PathMode.Full3D).OnComplete(ene5.Death).SetLookAt(0.01f).SetEase(Ease.InSine);


            yield return null;

        }

    }
}

