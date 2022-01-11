using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PolloScripts.UI
{
    public class SlowMotionBar : Bar
    {


        [Space]
        [Header("Slow Motion")]
        [Space]
        public float slowMotionFactor = 0.3f;
        public float playerSlowMotionFactor = 0.3f;

        [SerializeField] private Slider sliderBar;

        [SerializeField] protected int _restoreValue;
        [SerializeField] protected float _timeToRestore;
        [SerializeField] protected float _timeToCoolDown;
        [SerializeField] private float _ghostDistance;
        //[SerializeField] private GameObject _trail;

        public UnityEvent OnGhostEffect;
        public BoolEvent OnGhostTrail;
        public bool IsSlowing => _isSlowing;
        private bool _isSlowing;
        IEnumerator _ienConsumeBar;
        IEnumerator _ienRegenBar;

        private float _distanceTraveled;
        private Vector3 _lastPosition;
        private void Update()
        {

            if (_isSlowing)
            {
                //OnGhostEffect?.Invoke();
                //PlayerManager.Instance.player.GhostTrail();
                _distanceTraveled += Vector3.Distance(PlayerManager.Instance.player.hitPosition, _lastPosition);
                _lastPosition = PlayerManager.Instance.player.hitPosition;
                if(_distanceTraveled >= _ghostDistance)
                {
                   _distanceTraveled = 0;
                   OnGhostEffect?.Invoke();
                }
            }
      
        }
        public void SetMaxValues()
        {
            sliderBar.maxValue = maxValue;
            sliderBar.value = maxValue;
        }
        public override void SubtractValue(int newValue)
        {
            base.SubtractValue(newValue);
            sliderBar.value = currentValue;
        }
        public override void AddValue(int newValue)
        {
            //StopAllCoroutines();
            currentValue = Mathf.Clamp(currentValue + newValue, 0, maxValue);
            sliderBar.value = currentValue;
            //StartCoroutine(IenCountDown());
        }
        public float GetPlayerTimeScale()
        {
            if (_isSlowing)
            {
                return playerSlowMotionFactor;
            }
            else
            {
                return 1f;
            }
        }
        public void EnableSlowMotion()
        {
            active = true;
            StopAllCoroutines();
            _ienConsumeBar = null;
            _ienRegenBar = null;
            _isSlowing = false;
        }
        public void ConsumeBar(bool value)
        {
            if (active)
            {
                if (value && _ienConsumeBar == null)
                {
                    if (_ienRegenBar != null)
                        StopCoroutine(_ienRegenBar);
                    _isSlowing = false;
                    _ienConsumeBar = ConsumeBar();
                    StartCoroutine(_ienConsumeBar);
                }
                else if (!value && _ienConsumeBar != null)
                {
                    StopCoroutine(_ienConsumeBar);
                    _ienConsumeBar = null;

                    if (_ienRegenBar != null)
                        StopCoroutine(_ienRegenBar);

                    _ienRegenBar = IenCoolDown();

                    StartCoroutine(_ienRegenBar);
                }
            }
        }

        public void OnSlowMotion(bool state)
        {
            ConsumeBar(state);

            if (IsSlowing)
            {
                Time.timeScale = slowMotionFactor;
            }
            else
            {
                Time.timeScale = 1;
            }

            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
        private IEnumerator ConsumeBar()
        {
            OnGhostTrail?.Invoke(true);
            while (currentValue > 0)
            {
                _isSlowing = true;
                yield return new WaitForSecondsRealtime(timeToConsume);
                SubtractValue(valueToConsume);

                Time.timeScale = slowMotionFactor;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;


            }
            OnGhostTrail?.Invoke(false);
            Time.timeScale =1f;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
            _isSlowing = false;
        }
        private IEnumerator IenCoolDown()
        {
            _isSlowing = false;
            OnGhostTrail?.Invoke(false);
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;

            yield return new WaitForSecondsRealtime(_timeToCoolDown);

            while (currentValue < maxValue)
            {
                yield return new WaitForSecondsRealtime(_timeToRestore);
                AddValue(_restoreValue);
            }

        }

    }
}


