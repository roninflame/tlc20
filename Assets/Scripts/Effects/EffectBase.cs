using PolloScripts.Enums;
using UnityEngine;

namespace PolloScripts.Effects
{
    public abstract class EffectBase : MonoBehaviour
    {
        [Space]
        [Header("****** Base ******")]
        [Space]
        [SerializeField] protected GameObject _holderGO;
        [SerializeField] protected GameObject _modelGO;
        [SerializeField] protected int _scoreValue;

        public virtual AssetType AssetType => AssetType.Effect;
        public virtual void Activate()
        {
            gameObject.SetActive(true);
        }
        public virtual void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public virtual void ShowModel(bool show)
        {
            _modelGO.SetActive(show);
        }
    }
}

