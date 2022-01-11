
using PolloScripts.Enums;
using PolloScripts.ObjectPoolSystem;
using UnityEngine;
namespace PolloScripts.Effects
{
    public class FireExplosion : ExplosionBase
    {
        public override ExplosionType ExplosionType => ExplosionType.Fire;

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
            ObjectPoolFireExplosion.Instance.ReturnToPool(this);
        }

        public override void ReSize(float scale)
        {
            _modelGO.transform.localScale = Vector3.one * scale;
        }

        protected override void PlaySound()
        {
            anEmitter.Target.Play();
        }
    }

}