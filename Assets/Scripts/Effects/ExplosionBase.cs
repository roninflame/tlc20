using PolloScripts.Enums;
using System.Collections;
using UnityEngine;
using FMODUnity;
namespace PolloScripts.Effects
{
    public abstract class ExplosionBase : EffectBase
    {
        [Space]
        [Header("****** Explosion ******")]
        [SerializeField] protected float _lifeTime = 1f;
        //FMOD
        [Space]
        [Header("****** FMOD ******")]
        [SerializeField] protected EmitterRef anEmitter;

       
        protected abstract void PlaySound();
        public abstract ExplosionType ExplosionType { get; }

        protected virtual IEnumerator IenLifeTime()
        {
            yield return new WaitForSeconds(_lifeTime);
            Deactivate();
        }

        public abstract void ReSize(float scale);
        protected abstract void ReturnToPool();

    }

}
