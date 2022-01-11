using PolloScripts.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PolloScripts.UI
{

    public class UIExpressions : MonoBehaviour
    {
        [SerializeField] private CharacterData _characterData;
        [SerializeField] private GameObject _base;
        [SerializeField] private Image _charaterImg;
        [SerializeField] private Image _bannerImg;
        [SerializeField] private Sprite _tastySprite;
        [SerializeField] private Sprite _omgSprite;

        [SerializeField] private float _bannerTime = 2;

        private bool _bannerActivated;

        public void Stop()
        {
            StopAllCoroutines();
            _bannerActivated = false;

        }

        public void Blaster()
        {
            if (!_bannerActivated)
            {
                _bannerImg.sprite = _tastySprite;
                _bannerActivated = true;
                StartCoroutine(IenShowBanner());
            }
            //_charaterImg.sprite = _characterData.characterData[0].characterEmotion.emotions[8].sprite;
        }
        public void Omg()
        {
            if (!_bannerActivated)
            {
                _bannerImg.sprite = _omgSprite;
                _bannerActivated = true;
                StartCoroutine(IenShowBanner());
            }
        }
        public void ShowBanner(bool value)
        {
            _bannerImg.gameObject.SetActive(value);
        }
        public void ShowBase(bool value)
        {
            _base.SetActive(value);
        }

        public void ShowNormalExpression()
        {
            if (!PlayerManager.Instance.playerWeapons.specialWeaponActivated && !PlayerManager.Instance.playerController.IsFiring)
            {
                _charaterImg.sprite = _characterData.characterData[0].characterEmotion.emotions[4].sprite;
            }
            else if (!PlayerManager.Instance.playerWeapons.specialWeaponActivated && PlayerManager.Instance.playerController.IsFiring)
            {
                ShowShootinExpression(true);
            }
        }

        public void ShowShootinExpression(bool value)
        {
            if (!PlayerManager.Instance.playerWeapons.specialWeaponActivated && value)
            {
                _charaterImg.sprite = _characterData.characterData[0].characterEmotion.emotions[8].sprite;
            }
            else if (!PlayerManager.Instance.playerWeapons.specialWeaponActivated && !value)
            {
                _charaterImg.sprite = _characterData.characterData[0].characterEmotion.emotions[4].sprite;
            }
        }

       public void ShowDamageExpression()
        {
            _charaterImg.sprite = _characterData.characterData[0].characterEmotion.emotions[3].sprite;
        }

        public void SetExpression(int index)
        {
            _charaterImg.sprite = _characterData.characterData[0].characterEmotion.emotions[index].sprite;
        }

        IEnumerator IenShowBanner()
        {
            ShowBanner(true);
            ShowBase(false);
            yield return new WaitForSeconds(_bannerTime);
            ShowBanner(false);
            ShowBase(true);
            _bannerActivated = false;
        }
    }

}
