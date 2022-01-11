using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class VolumeManager : MonoBehaviour
{
    public static VolumeManager Instancia;
    public Volume Volume;
    public LensDistortion LensDistortion;
    public ChromaticAberration ChromaticAberration;
    void Start()
    {
        if(Instancia == null)
        {
            Instancia = this;
        }
        else{
            Destroy(gameObject);
        }

        LensDistortion tmp;

        if (Volume.profile.TryGet(out tmp))
        {
            LensDistortion = tmp;
        }
        ChromaticAberration tmp1;

        if (Volume.profile.TryGet(out tmp1))
        {
            ChromaticAberration = tmp1;
        }
    }

    void Update()
    {
        
    }
}
