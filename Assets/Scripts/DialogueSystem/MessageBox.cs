using DG.Tweening;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PolloScripts.DialogueSystem
{


    public class MessageBox : MonoBehaviour
    {
        public static MessageBox instance;
        public float animationTime = 2f;
        public GameObject iconAlert;
        public Image iconAlertOnImg;

        [Space]
        [Header("Messages UI")]
        public DialogueBox dialogueBox;
        public CanvasGroup dialogueCnvGrp;

        [Space]
        [Header("Characters")]
        public CharacterData characterData;

        //public ConversationsData messageData;
        private bool _inDialogue;
        private ConversationData currentMessageData;
        private CharacterInfoData _currentCharacer;
        private State _state;

        //FMOD
        private FMOD.Studio.EventInstance aVoiceSoundInstance;
        //FMOD
        private FMOD.Studio.EventInstance aPunctuationSoundInstance;
        private void Start()
        {
            if (instance == null)
            {
                instance = this;

            }
            else
            {
                Destroy(this);
            }
            dialogueCnvGrp.alpha = 0;
            iconAlert.SetActive(false);
        }
        private void StartAnimation()
        {
            iconAlert.SetActive(true);
            dialogueCnvGrp.alpha = 1;
            StartCoroutine(IenStartAnimation());
        }

        private IEnumerator IenStartAnimation()
        {
    
            Tween tScale = iconAlert.transform.DOScale(1.3f, 0.1f).SetLoops(-1, LoopType.Yoyo);
            Tween tFade = iconAlertOnImg.DOFade(1, 0.1f).SetLoops(-1, LoopType.Yoyo);
            yield return new WaitForSeconds(animationTime);

            tFade.Rewind(true);
            tScale.Rewind(true);
            //yield return new WaitForSeconds(0.2f);

            iconAlert.SetActive(false);
            dialogueCnvGrp.alpha = 0;

            if (currentMessageData != null)
            {
                //AddListeners();
                ResetComponents();
                FadeUI(true, .1f, .15f);
            }

            while (_inDialogue) { yield return null; }

        }

        //=================== MESSAGE SYSTEM ===========================0

        public void StartMessage(ConversationsData messagesData)
        {
            if (Lean.Localization.LeanLocalization.GetOrCreateInstance().CurrentLanguage == "English")
            {
                currentMessageData = messagesData.Converstion_Eng[0];
            }
            else if (Lean.Localization.LeanLocalization.GetOrCreateInstance().CurrentLanguage == "Spanish")
            {
                currentMessageData = messagesData.Converstion_Spa[0];
            }

            StartAnimation();
        }

        private void ResetComponents()
        {

            _inDialogue = true;
            dialogueBox.ResetDialogueBox();
            dialogueBox.SetActive(false);
        }
        private void FadeUI(bool show, float time, float delay)
        {
            Sequence s = DOTween.Sequence();
            s.AppendInterval(delay);
            s.Append(dialogueCnvGrp.DOFade(show ? 1 : 0, time));
            if (show)
            {
                s.Join(dialogueCnvGrp.transform.DOScale(0, time * 2).From().SetEase(Ease.OutBack));
                s.AppendCallback(() => StartCoroutine(Activate_List()));
            }

        }

        private IEnumerator Activate_List()
        {

            _state = State.Active;

            foreach (var Data in currentMessageData.dialogueList)
            {

                GetCharacterInfo(Data.characterName);
                AddListeners();
                SetCharNameAndColor();
                SetVoicesFmod();
                EmotionChanger(EmotionType.normal);

                foreach (var item in Data.conversationBlock)
                {
                    //Se desactiva al inicio de dialogo
                    dialogueBox.NextActive(false);

                    _state = State.Active;

                    dialogueBox.ReadText(item);

                    while (_state != State.Deactivate) { yield return null; }

                    yield return new WaitForSeconds(1);

                    _state = State.Wait;

                }

                RemoveListeners();

            }

            yield return new WaitForSeconds(1f);

            FadeUI(false, .2f, 0);
            Sequence s = DOTween.Sequence();
            s.AppendInterval(.8f);
            s.AppendCallback(() => ResetState());
        }

        private void GetCharacterInfo(CharacterName name)
        {
            dialogueBox.SetActive(true);
            _currentCharacer = characterData.characterData.Single(x => x.characterName == name);
        }
        private void SetCharNameAndColor()
        {
            dialogueBox.SetCharNameAndColor(_currentCharacer);
        }
        private void AddListeners()
        {
            dialogueBox.animatedText.onDialogueFinish.AddListener(() => FinishDialogue());
            dialogueBox.animatedText.onTextReveal.AddListener((newChar) => ReproduceSound(newChar));
            dialogueBox.animatedText.onEmotionChange.AddListener((newEmotion) => EmotionChanger(newEmotion));
            dialogueBox.animatedText.onAction.AddListener((action) => SetAction(action));
        }
        private void RemoveListeners()
        {
            dialogueBox.animatedText.onDialogueFinish.RemoveListener(() => FinishDialogue());
            dialogueBox.animatedText.onTextReveal.RemoveListener((newChar) => ReproduceSound(newChar));
            dialogueBox.animatedText.onEmotionChange.RemoveListener((newEmotion) => EmotionChanger(newEmotion));
            dialogueBox.animatedText.onAction.RemoveListener((action) => SetAction(action));

        }
        private void FinishDialogue()
        {
            _state = State.Deactivate;
        }
        private void ReproduceSound(char c)
        {
            FMOD.Studio.PLAYBACK_STATE playbackState;
            aVoiceSoundInstance.getPlaybackState(out playbackState);

            FMOD.Studio.PLAYBACK_STATE playbackState2;
            aPunctuationSoundInstance.getPlaybackState(out playbackState2);

            if(GameManager.Instance != null)
            {
                if (GameManager.Instance.soundPerLetter)
                {
                    if (char.IsPunctuation(c))
                    {
                        aPunctuationSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                        aPunctuationSoundInstance.start();
                    }

                    if (char.IsLetter(c))
                    {
                        aVoiceSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                        aVoiceSoundInstance.start();
                    }
                }
                else
                {
                    if (char.IsPunctuation(c) && !(playbackState2 == FMOD.Studio.PLAYBACK_STATE.PLAYING))
                    {
                        aPunctuationSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                        aPunctuationSoundInstance.start();
                    }

                    if (char.IsLetter(c) && !(playbackState == FMOD.Studio.PLAYBACK_STATE.PLAYING))
                    {
                        aVoiceSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                        aVoiceSoundInstance.start();
                    }
                }
            }
            else
            {
                if (char.IsPunctuation(c))
                {
                    aPunctuationSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    aPunctuationSoundInstance.start();
                }

                if (char.IsLetter(c))
                {
                    aVoiceSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    aVoiceSoundInstance.start();
                }
            }

            


        }
        private void EmotionChanger(EmotionType emotion)
        {
            dialogueBox.SetCharacterEmotion(_currentCharacer.characterEmotion.emotions[(int)emotion].sprite);
        }
        private void SetAction(string action)
        {

        }
        private void ResetState()
        {
            dialogueBox.SetActive(false);
            _inDialogue = false;
        }
        private void SetVoicesFmod()
        {
            aVoiceSoundInstance.release();
            aVoiceSoundInstance = RuntimeManager.CreateInstance(_currentCharacer.voicesFMOD);

            aPunctuationSoundInstance.release();
            aPunctuationSoundInstance = RuntimeManager.CreateInstance(_currentCharacer.punctuationsFMOD);
        }

    }
}

