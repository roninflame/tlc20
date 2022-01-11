using PolloScripts.Enums;
using PolloScripts.ObjectPoolSystem;
using UnityEngine;

namespace PolloScripts.Effects
{
    public class PlayerExplosion : ExplosionBase
    {
        //public float size;
        public override ExplosionType ExplosionType => ExplosionType.Player;

        public void Init(float lifeTime, Transform parent, Vector3 position, Quaternion rotation)
        {
            _lifeTime = lifeTime;

            if (parent != null)
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
            ObjectPoolPlayerExplosion.Instance.ReturnToPool(this);
        }

        public override void ReSize(float scale)
        {
            ////Explosion
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
