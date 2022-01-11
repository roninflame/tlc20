using PolloScripts.Effects;
using PolloScripts.Enums;
using PolloScripts.ObjectPoolSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Enemies
{
    public class IndestructibleAsteroid : AsteroidBase
    {
        public override AsteroidType TypeID => AsteroidType.Indestructible;
        public override void DamageReceived(int value)
        {
            if (!_isTakingDamage && _canTakeDamage)
            {
                StartCoroutine(IenDamageCoolDown());
            }
            HitSound();
            if (_colorMaterialHitCor == null)
            {
                _colorMaterialHitCor = StartCoroutine(IenColorMaterialHit());
            }
            else
            {
                StopCoroutine(_colorMaterialHitCor);
                _colorMaterialHitCor = StartCoroutine(IenColorMaterialHit());
            }
        }
        public override void Death()
        {
            base.Death();
            Deactivate();
            //Explosion
            ExplosionBase explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, transform.position, Quaternion.identity);
            explosion.ReSize(30);
            explosion.Activate();
        }
        protected override void ReturnToPool()
        {
            ObjectPoolIndestructibleAsteroid.Instance.ReturnToPool(this);
        }
        protected override void HitSound()
        {
            if (_hitSound.Length > 0)
                FMODUnity.RuntimeManager.PlayOneShot(_hitSound, transform.position);
        }
    }

}
