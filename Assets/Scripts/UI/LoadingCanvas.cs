using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

namespace PolloScripts
{
    public class LoadingCanvas : MonoBehaviour
    {
        public static LoadingCanvas Instance { get; private set; }

        [SerializeField] private Image _frameImage;
        [SerializeField] private Slider _loadingSlider;
        [SerializeField] private Image _fadeScreenImage;

        private GameObject _frameGO;
        private GameObject _loadingGO;
        private GameObject _fadeScreenGO;
        //public GameObject Camera;

        public UnityEvent OnFadeInComplete;
        public UnityEvent OnFadeIn2Complete;
        public UnityEvent OnFadeOutComplete;
        public UnityEvent OnFadeOut2Complete;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                _frameGO = _frameImage.gameObject;
                _loadingGO = _loadingSlider.gameObject;
                _fadeScreenGO = _fadeScreenImage.gameObject;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public void SetActive(bool b)
        {
            gameObject.SetActive(b);
            _frameGO.SetActive(b);
            _loadingGO.SetActive(true);
            _fadeScreenGO.SetActive(b);
        }
        public void SetLoadingBarValue(float value)
        {
            _loadingSlider.value = value;
        }
        public void StartFadeIn(int fase)
        {
            StartCoroutine(IenFadeIN(fase));
        }
        public void StartFadeOut(int fase)
        {
            StartCoroutine(IenFadeOut(fase));
        }
        IEnumerator IenFadeIN(int tipo)
        {
            _fadeScreenGO.SetActive(true);
            _fadeScreenImage.color = new Color(_fadeScreenImage.color.r, _fadeScreenImage.color.g, _fadeScreenImage.color.b, 0.1f);
            if (tipo == 1)
            {
                _frameGO.SetActive(false);
            }
            else if (tipo == 2)
            {

            }

            Tween myTween = _fadeScreenImage.DOFade(1f, 1f);
            yield return myTween.WaitForCompletion();
            if (tipo == 1)
            {
                OnFadeInComplete.Invoke();
            }
            else if (tipo == 2)
            {
                OnFadeIn2Complete.Invoke();
            }

        }
        IEnumerator IenFadeOut(int fase)
        {
            _fadeScreenGO.SetActive(true);
            _fadeScreenImage.color = new Color(_fadeScreenImage.color.r, _fadeScreenImage.color.g, _fadeScreenImage.color.b, 1f);
            if(fase == 1)
            {
                _frameGO.SetActive(true);
                _loadingSlider.value = 0;
            }
            else if (fase == 2)
            {
                _frameGO.SetActive(false);

            }
            Tween myTween = _fadeScreenImage.DOFade(0.1f, 1f);
            yield return myTween.WaitForCompletion();
            _fadeScreenImage.color = new Color(_fadeScreenImage.color.r, _fadeScreenImage.color.g, _fadeScreenImage.color.b, 0f);
            _fadeScreenGO.SetActive(false);

            if (fase == 1)
            {
                OnFadeOutComplete.Invoke();
            }
            else if (fase == 2)
            {
                OnFadeOut2Complete.Invoke();
            }
           
        }

    }

}
