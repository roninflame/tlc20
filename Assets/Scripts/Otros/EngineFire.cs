using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineFire : MonoBehaviour
{
    [SerializeField] ParticleSystemRenderer _engine;
    [SerializeField] ParticleSystemRenderer _glow;

    ParticleSystem _psEngine;
    ParticleSystem _psGolw;

    public void SetEngine(Material mat)
    {
        _engine.material = mat;
        _engine.trailMaterial = mat;
        _psEngine = _engine.GetComponent<ParticleSystem>();
 
    }

    public void SetGlow(Material mat)
    {
        _glow.material = mat;
        _glow.trailMaterial = mat;
        _psGolw = _glow.GetComponent<ParticleSystem>();
    }

    public void SetSize(float size)
    {
        transform.localScale = Vector3.one * size;
    }
    public void SetEngineAlpha(float a)
    {
        if(_psEngine != null)
        {
            var main1 = _psEngine.main;
            var main2 = _psGolw.main;
            main1.startColor = new Color(main1.startColor.color.r, main1.startColor.color.g, main1.startColor.color.b, a);
            main2.startColor = new Color(main2.startColor.color.r, main2.startColor.color.g, main2.startColor.color.b, a);
        }
        
    }
}
