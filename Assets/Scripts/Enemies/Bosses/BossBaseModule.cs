using PolloScripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts.Enemies.Bosses
{
    public abstract class BossBaseModule : MonoBehaviour
    {
        [Header("Explosions")]
        [SerializeField] protected List<Vector3> _explosionPointsList;
        [SerializeField] protected int _scoreValue = 100;

        protected int _currentLevel;
        public abstract void OnLostHealth();
        public abstract void OnDeath();

        public abstract void StartModule();
        protected abstract void SetWeaponData(int level);

        protected abstract void ActivateWeapons();

        protected abstract void StopWeapons();
        protected void AddScore()
        {
            CanvasManager.Instance.OnAddScore?.Invoke(_scoreValue);
        }

        protected abstract IEnumerator IenExplosions();
      
    }
}