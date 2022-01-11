using PolloScripts.Data;
using PolloScripts.Effects;
using PolloScripts.Enums;
using PolloScripts.ObjectPoolSystem;
using UnityEngine;

public class ObjectPoolEffect 
{
    public ExplosionBase GetExplosion(ExplosionType itemID, ExplosionEffectData data, Transform parent, Vector3 position, Quaternion rotation)
    {
        ExplosionBase result = null;

        switch (itemID)
        {
            case ExplosionType.None:
                break;
            case ExplosionType.Fire:
                result = GetFireExplosion(data, parent, position, rotation);
                break;
            case ExplosionType.Energy:
                result = GetEneryExplosion(data, parent, position, rotation);
                break;
            case ExplosionType.Player:
                result = GetPlayerExplosion(data, parent, position, rotation);
                break;
            default:
                break;
        }
        return result;
    }
    private ExplosionBase GetEneryExplosion(ExplosionEffectData data, Transform parent, Vector3 position, Quaternion rotation)
    {
        EnergyExplosion obj = ObjectPoolEnergyExplosion.Instance.ActivateFromPool();
        obj.Init(data.lifeTime, parent, position, rotation);
        //obj.Activate();
        return obj;
    }
    private ExplosionBase GetFireExplosion(ExplosionEffectData data, Transform parent, Vector3 position, Quaternion rotation)
    {
        FireExplosion obj = ObjectPoolFireExplosion.Instance.ActivateFromPool();
        obj.Init(data.lifeTime, parent, position, rotation);
        //obj.Activate();
        return obj;
    }
    private ExplosionBase GetPlayerExplosion(ExplosionEffectData data, Transform parent, Vector3 position, Quaternion rotation)
    {
        PlayerExplosion obj = ObjectPoolPlayerExplosion.Instance.ActivateFromPool();
        obj.Init(data.lifeTime, parent, position, rotation);
        //obj.Activate();
        return obj;
    }
}
