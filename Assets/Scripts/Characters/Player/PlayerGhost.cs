using PolloScripts.ObjectPoolSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts
{
    public class PlayerGhost : MonoBehaviour
    {
        public Transform rootBone1Parent;
        public Transform rootBone2Parent;

        public GameObject rootBone1;
        public GameObject rootBone2;

        public float lifeTime = 0.15f;
        public float lifeTime2 = 0.5f;
        private Transform[] rootBones1;
        private Transform[] rootBones2;

        public void Initialized(Transform spawnParent, Transform rootParent, GameObject bones)
        {
            //transform.parent = spawnParent;

            if (rootBones1 == null)
            {
                rootBone1Parent = rootParent;
                rootBone1 = bones;

                rootBones1 = rootBone1.GetComponentsInChildren<Transform>();
                rootBones2 = rootBone2.GetComponentsInChildren<Transform>();
            }
        }
        public void CopyObjectBones()
        {

            Activate();
            rootBone2Parent.position = rootBone1Parent.position;
            rootBone2Parent.rotation = rootBone1Parent.rotation;

            for (int i = 0; i < rootBones1.Length; i++)
            {
                rootBones2[i].localPosition = rootBones1[i].localPosition;
                rootBones2[i].localRotation = rootBones1[i].localRotation;

            }

            StartCoroutine(LifeTime());

        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }
        public void Deactivate()
        {
            if (gameObject.activeInHierarchy)
            {
                ObjectPoolPlayerGhost.Instance.ReturnToPool(this);
            }
            
        }

        IEnumerator LifeTime()
        {
            if(PlayerManager.Instance.DollyActivated){
                 yield return new WaitForSeconds(lifeTime);
            }else{
                 yield return new WaitForSeconds(lifeTime2);
            }
           
            Deactivate();
        }
    }
}

