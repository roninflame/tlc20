using PolloScripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AssetBase : MonoBehaviour
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
    public virtual void ReturnToPool()
    {
       
    }
}
