using PolloScripts.Enums;
using PolloScripts.ObjectPoolSystem;
using UnityEngine;
namespace PolloScripts.Effects
{
	public class EnergyExplosion : ExplosionBase
    {
        //public float size;
        public override ExplosionType ExplosionType => ExplosionType.Energy;

        public void Init(float lifeTime, Transform parent, Vector3 position, Quaternion rotation)
        {
            _lifeTime = lifeTime;

            if(parent != null)
            {
                transform.parent = parent;
                transform.localPosition = position;
                transform.localRotation = rotation;
            }
            else
            {
                transform.position = position;
                transform.rotation = rotation;
            }
        }
        //public void Resize(float size)
        //{
        //    GAP_ParticleSystemController.ParticleSystemController mparticle =_modelGO.GetComponent<GAP_ParticleSystemController.ParticleSystemController>();
        //    mparticle.size = size;
        //    mparticle.UpdateParticleSystem();
        //}
        public override void Activate()
        {
            base.Activate();
            PlaySound();
            StartCoroutine(IenLifeTime());
        }
        public override void Deactivate()
        {
            base.Deactivate();
            ReturnToPool();
        }
        protected override void ReturnToPool()
        {
            ObjectPoolEnergyExplosion.Instance.ReturnToPool(this);
        }

        public override void ReSize(float scale)
        {
            //Explosion
            //ExplosionBase explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Energy, null, transform.position, Quaternion.identity);
            ////explosion.ReSize(1);
            //explosion.Activate();
        }

        protected override void PlaySound()
        {
            anEmitter.Target.Play();
            //FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Enemy/Explosion", transform.position);
        }
    }

}
