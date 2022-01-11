using DG.Tweening;
using PolloScripts.Data;
using PolloScripts.Enums;
using PolloScripts.Interfaces;
using PolloScripts.ObjectPoolSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.WeaponSystem
{
    public class BossRocket : Projectile
    {
        [Space]
        [Header("***** Rocket *****")]
        public LineRenderer lineRenderer;
        public float lineRange = 30;

        private Vector4 _length = new Vector4(1, 1, 1, 1);

        private Sequence _sequence;

        private void Update()
        {
            if (lineRenderer.enabled)
            {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, transform.position + transform.forward * lineRange);
            }
        }
        public void Init(ProjectileData data, TargetType targetType, Transform parent, Vector3 position, Quaternion rotation)
        {
            _targetType = targetType;
            _speed = data.speed;
            _damage = data.damage;
            //_fireRate = data.fireRate;

            if (parent != null)
            {
                transform.parent = parent;
                transform.rotation = rotation;
                transform.position = position;

            }
            else
            {
                transform.rotation = rotation;
                transform.position = position;
            }
        }
        public override void Shoot()
        {
            _canMove = true;
        }
        public void Move(IRocketMovement pattern, Vector3 destiny, Vector3 rotation)
        {
            _sequence = pattern.Move(transform, destiny, rotation, LineRendererON, LineRendererOFF, DeactivateByTween);
        }
        //public void Move(ITrajectoryPattern pattern, Vector3 destiny, Vector3 rotation)
        //{
        //    _sequence = pattern.Move(transform,destiny, rotation, DeactivateByTween);
        //}
        public void LineRendererON()
        {
            lineRenderer.material.SetTextureScale("_MainTex", new Vector2(_length[0], _length[1]));
            lineRenderer.material.SetTextureScale("_Noise", new Vector2(_length[2], _length[3]));

            lineRenderer.enabled = true;
        }
        public void LineRendererOFF()
        {
            lineRenderer.enabled = false;
        }
        public override void Deactivate()
        {
            base.Deactivate();
            _sequence?.Kill();
            _sequence = null;
        }
        private void DeactivateByTween()
        {
            Deactivate();
        }
        protected void OnTriggerEnter(Collider other)
        {
            if (DoDamage(other.gameObject))
            {
                Deactivate();
            }
        }
        protected override void ReturnToPool()
        {
            ObjectPoolBossRocket.Instance.ReturnToPool(this);
        }

        protected override void PlayWeaponSound()
        {
   
        }
    }

}

