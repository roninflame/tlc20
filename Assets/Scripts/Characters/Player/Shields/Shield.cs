using System.Collections;
using UnityEngine;

namespace PolloScripts
{
    public abstract class Shield : MonoBehaviour
    {
        public bool IsActive => isActive;
        [SerializeField] protected Renderer meshRender;
        [ColorUsage(true, true)]
        [SerializeField] protected Color normalColor;
        [ColorUsage(true, true)]
        [SerializeField] protected Color crackColor;
        protected Material material;
        private bool isActive;

        protected virtual void Start()
        {
            SetNormalColor();
            meshRender.enabled = false;
        }
        public void SetNormalColor()
        {
            meshRender.material.SetColor("Emision", normalColor);
        }
        public void SetCrackColor()
        {
            meshRender.material.SetColor("Emision", crackColor);
        }
        public virtual void Show()
        {
            isActive = true;
            meshRender.enabled = true;
            //StartCoroutine(IenActivate());
        }

        public virtual void Hide()
        {
            meshRender.enabled = false;
            isActive = false;
            //StopCoroutine(IenActivate());
        }

        //IEnumerator IenActivate()
        //{
        //    isActive = true;
        //    meshRender.enabled = true;
        //    yield return new WaitForSeconds(1f);
        //    meshRender.enabled = false;
        //    isActive = false;
        //}
    }
}

