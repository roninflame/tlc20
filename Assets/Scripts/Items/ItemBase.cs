using PolloScripts.Enums;
using PolloScripts.UI;
using UnityEngine;

namespace PolloScripts.Items
{
    public abstract class ItemBase : MonoBehaviour
    {
        [Space]
        [Header("****** ItemBase ******")]
        [SerializeField] protected GameObject _holderGO;
        [SerializeField] protected GameObject _modelHolderGO;
        [SerializeField] protected GameObject _modelGO;
        [SerializeField] protected ParticleSystem _particle;
        [SerializeField] protected ParticleSystem _contactPS;


        [Space]
        [Header("Parameters")]
        [SerializeField] protected int _itemValue;
        [SerializeField] protected int _scoreValue;


        //Share
        public AssetType AssetType => AssetType.Item;
        public abstract ItemType ItemType { get; }

        
        //LOCAL
        protected bool _canInteract;

        public virtual void Activate()
        {
            gameObject.SetActive(true);
        }
        public abstract void Deactivate();

        public virtual void ShowModel(bool show)
        {
            _modelHolderGO.SetActive(show);
        }
        protected void AddScore(){
            CanvasManager.Instance.OnAddScore?.Invoke(_scoreValue);
        }

        public abstract void StartAnimation();

        public abstract void StopAnimation();
        
    }
}

