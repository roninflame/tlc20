﻿using PolloScripts.Effects;
using PolloScripts.Enums;
using PolloScripts.ObjectPoolSystem;
using UnityEngine;

namespace PolloScripts.Enemies
{
    public class BigAsteroid : AsteroidBase
    {
        public override AsteroidType TypeID => AsteroidType.Big;
        
        public override void Death()
        {
            base.Death();
            Deactivate();
            //Explosion
            ExplosionBase explosion = ObjectPoolManager.Instance.ActivateExplosion(ExplosionType.Fire, null, transform.position, Quaternion.identity);
            explosion.ReSize(25);
            explosion.Activate();

        }
        protected override void ReturnToPool()
        {
            ObjectPoolBigAsteroid.Instance.ReturnToPool(this);
        }
        protected override void HitSound()
        {
            if(_hitSound.Length > 0)
                FMODUnity.RuntimeManager.PlayOneShot(_hitSound, transform.position);
        }
    }

}