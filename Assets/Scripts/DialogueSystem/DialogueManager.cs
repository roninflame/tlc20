using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;
using Cinemachine;
using FMODUnity;

namespace PolloScripts.DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager instance;

        [Space]
        [Header("Dialogue UI")]
        public Transform background;
        public DialogueBox rightDialogueBox;
        public DialogueBox leftDialogueBox;
        public CanvasGroup dialogueCnvGrp;

 

        //public TMP_Animated animatedText;
        //public Image nameBubble;
        //public TextMeshProUGUI nameTMP;
        //public GameObject nextGO;

        [Space]
        [Header("Characters")]
        public CharacterData characterData;
        //public Image characterL;
        //public Image characterR;

        [Space]
        [Header("Cameras")]
        public GameObject gameCam;
        public GameObject dialogueCam;

        //[Space]
        //public Volume dialogueDof;

        //[Space]
        //[Header("Sounds")]
        //public AudioSource voiceSource;
        //public AudioSource punctuationSource;

        [Space]
        [Header("Dialogue")]
        public ConversationsData conversationsData;

        public bool InDialogue => _inDialogue;

        private ConversationData _currentDialogue;
        private CharacterInfoData _currentCharacer;
        private State _state;
        private bool _inDialogue;
        private bool _isSkippable;
        private bool _nextDialogue;


        //FMOD
        private FMOD.Studio.EventInstance aVoiceSoundInstance;
        //FMOD
        private FMOD.Studio.EventInstance aPunctuationSoundInstance;
        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
                rightDialogueBox.SetActive(false);
                leftDialogueBox.SetActive(false);
                _inDialogue = false;
                dialogueCnvGrp.alpha = 0;

            }
            else
            {
                Destroy(this);
            }
         
        }
        public void AddListeners()
        {
            if (_currentCharacer.characterName == CharacterName.Broster)
            {
                rightDialogueBox.animatedText.onDialogueFinish.AddListener(() => FinishDialogue());
                rightDialogueBox.animatedText.onTextReveal.AddListener((newChar) => ReproduceSound(newChar));
                rightDialogueBox.animatedText.onEmotionChange.AddListener((newEmotion) => EmotionChanger(newEmotion));
                rightDialogueBox.animatedText.onAction.AddListener((action) => SetAction(action));
            }
            else
            {
                leftDialogueBox.animatedText.onDialogueFinish.AddListener(() => FinishDialogue());
                leftDialogueBox.animatedText.onTextReveal.AddListener((newChar) => ReproduceSound(newChar));
                leftDialogueBox.animatedText.onEmotionChange.AddListener((newEmotion) => EmotionChanger(newEmotion));
                leftDialogueBox.animatedText.onAction.AddListener((action) => SetAction(action));
            }
        }
        public void RemoveListeners()
        {

            if (_currentCharacer.characterName == CharacterName.Broster)
            {
                rightDialogueBox.animatedText.onDialogueFinish.RemoveListener(() => FinishDialogue());
                rightDialogueBox.animatedText.onTextReveal.RemoveListener((newChar) => ReproduceSound(newChar));
                rightDialogueBox.animatedText.onEmotionChange.RemoveListener((newEmotion) => EmotionChanger(newEmotion));
                rightDialogueBox.animatedText.onAction.RemoveListener((action) => SetAction(action));

            }
            else
            {
                leftDialogueBox.animatedText.onDialogueFinish.RemoveListener(() => FinishDialogue());
                leftDialogueBox.animatedText.onTextReveal.RemoveListener((newChar) => ReproduceSound(newChar));
                leftDialogueBox.animatedText.onEmotionChange.RemoveListener((newEmotion) => EmotionChanger(newEmotion));
                leftDialogueBox.animatedText.onAction.RemoveListener((action) => SetAction(action));

            }

        }

        public void StartDialogue(ConversationIndex index)
        {
            PlayerManager.Instance.playerController.EnableMenuControls();

            SetConversation(index);
            if(_currentDialogue != null)
            {
                //AddListeners();
                ResetComponents();
                CameraChange(true);
                FadeUI(true, .2f, .65f);
            }
        }
        public void StartDialogue(ConversationsData data)
        {
            PlayerManager.Instance.playerController.EnableMenuControls();

            if (Lean.Localization.LeanLocalization.GetOrCreateInstance().CurrentLanguage == "English")
            {
                _currentDialogue = conversationsData.Converstion_Eng.Single(x => x.conversationIndex == 0);
            }
            else if (Lean.Localization.LeanLocalization.GetOrCreateInstance().CurrentLanguage == "Spanish")
            {
                _currentDialogue = conversationsData.Converstion_Spa.Single(x => x.conversationIndex == 0);
            }

            if (_currentDialogue != null)
            {
                //AddListeners();
                ResetComponents();
                CameraChange(true);
                FadeUI(true, .2f, .65f);
            }
        }

        private void ResetComponents()
        {
            _isSkippable = false;
            _inDialogue = true;
            _nextDialogue = true;
            leftDialogueBox.ResetDialogueBox();
            rightDialogueBox.ResetDialogueBox();
            leftDialogueBox.SetActive(false);
            rightDialogueBox.SetActive(false);
            //nextGO.SetActive(false);
        }
        private void ShowDialogue()
        {
            StartCoroutine(Activate_List());
        }

        private IEnumerator Activate_List()
        {
            
            _state = State.Active;

            foreach (var Data in _currentDialogue.dialogueList)
            {

                GetCharacterInfo(Data.characterName);
                AddListeners();
                SetCharNameAndColor();
                SetVoicesFmod();
                EmotionChanger(EmotionType.normal);

                foreach (var item in Data.conversationBlock)
                {
                    //Se desactiva al inicio de dialogo
                    if(_currentCharacer.characterName == CharacterName.Broster)
                    {
                        rightDialogueBox.NextActive(false);
                    }
                    else
                    {
                        leftDialogueBox.NextActive(false);
                    }

                    _state = State.Active;

                    StartCoroutine(IenIsSkippable());

                    if (_currentCharacer.characterName == CharacterName.Broster)
                    {
                        rightDialogueBox.ReadText(item);
                    }
                    else
                    {
                        leftDialogueBox.ReadText(item);
                    }
                 
                    while (_state != State.Deactivate) { yield return null; }

                    //yield return new WaitForSeconds(1);

                    //Muestro Next
                    if (_currentCharacer.characterName == CharacterName.Broster)
                    {
                        rightDialogueBox.NextActive(true);
                    }
                    else
                    {
                        leftDialogueBox.NextActive(true);
                    }

                    //Puede avanzar al siguiente dialogo o terminarlo
                    _state = State.Wait;

                    while (!_nextDialogue) { yield return null; }

                }

                RemoveListeners();

            }

            CameraChange(false);
            FadeUI(false, .2f, 0);
            Sequence s = DOTween.Sequence();
            s.AppendInterval(.8f);
            s.AppendCallback(() => ResetState());
        }
        private IEnumerator IenIsSkippable()
        {
            _isSkippable = false;
            yield return new WaitForSeconds(0.5f);
            _isSkippable = true;
        }
 
        private void GetCharacterInfo(CharacterName name)
        {

            if (name == CharacterName.Broster)
            {
                rightDialogueBox.SetActive(true);
                leftDialogueBox.SetActive(false);
            }
            else
            {
                rightDialogueBox.SetActive(false);
                leftDialogueBox.SetActive(true);
            }
            _currentCharacer = characterData.characterData.Single(x => x.characterName == name);
        }
        private void SetVoicesFmod()
        {
            aVoiceSoundInstance.release();
            aVoiceSoundInstance = RuntimeManager.CreateInstance(_currentCharacer.voicesFMOD);

            aPunctuationSoundInstance.release();
            aPunctuationSoundInstance = RuntimeManager.CreateInstance(_currentCharacer.punctuationsFMOD);
        }
        private void FadeUI(bool show, float time, float delay)
        {
            Sequence s = DOTween.Sequence();
            s.AppendInterval(delay);
            s.Append(dialogueCnvGrp.DOFade(show ? 1 : 0, time));
            if (show)
            {
                s.Join(dialogueCnvGrp.transform.DOScale(0, time * 2).From().SetEase(Ease.OutBack));
                s.AppendCallback(() => ShowDialogue());
            }
            
        }
        private void SetCharNameAndColor()
        {
            if(_currentCharacer.characterName == CharacterName.Broster)
            {
                rightDialogueBox.SetCharNameAndColor(_currentCharacer);
            }
            else
            {
                leftDialogueBox.SetCharNameAndColor(_currentCharacer);
            }
        }
        private void CameraChange(bool dialogue)
        {
            gameCam.SetActive(!dialogue);
            dialogueCam.SetActive(dialogue);

            //Depth of field modifier
            //float dofWeight = dialogueCam.activeSelf ? 1 : 0;
            //DOVirtual.Float(dialogueDof.weight, dofWeight, .8f, DialogueDOF);
        }

        #region CallBacks
        public void Click_Window()
        {
            switch (_state)
            {
                case State.Active:
                    if (_isSkippable)
                    {
                        _isSkippable = false;
                        if (_currentCharacer.characterName == CharacterName.Broster)
                        {
                            rightDialogueBox.StopReading(); ;
                        }
                        else
                        {
                            leftDialogueBox.StopReading(); ;
                        }
                    }

                    break;

                case State.Wait:
                    if (!_isSkippable && !_nextDialogue)
                        _nextDialogue = true;
                    break;
            }
        }
        public void ReproduceSound(char c)
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
            //if (char.IsLetter(c) && !(playbackState == FMOD.Studio.PLAYBACK_STATE.PLAYING))
            //{
            //    aSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            //    aSoundInstance.start();
            //}

            // OLD WAY
            //if (char.IsPunctuation(c) && !punctuationSource.isPlaying)
            //{
            //    aSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            //    aSoundInstance.start();
            //}

            //if (char.IsLetter(c) && !voiceSource.isPlaying)
            //{
            //    aSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            //    aSoundInstance.start();
            //}
        }

        public void EmotionChanger(EmotionType emotion)
        {
            if (_currentCharacer.characterName == CharacterName.Broster)
            {
                rightDialogueBox.SetCharacterEmotion(_currentCharacer.characterEmotion.emotions[(int)emotion].sprite);
            }
            else
            {
                leftDialogueBox.SetCharacterEmotion(_currentCharacer.characterEmotion.emotions[(int)emotion].sprite);
            }
        }

        public void SetAction(string action)
        {
            if (action == "shake")
            {
                dialogueCam.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            }

        }
        public void FinishDialogue()
        {
            _state = State.Deactivate;
            _isSkippable = false;
            _nextDialogue = false;
        }
        public void ResetState()
        {
            leftDialogueBox.SetActive(false);
            rightDialogueBox.SetActive(false);
            _inDialogue = false;
            //RemoveListeners();

            //characterL.gameObject.SetActive(false);
            //characterR.gameObject.SetActive(false);


        }
        #endregion

        #region Conversation

        private void SetConversation(ConversationIndex index)
        {

            if (Lean.Localization.LeanLocalization.GetOrCreateInstance().CurrentLanguage == "English")
            {
                _currentDialogue = conversationsData.Converstion_Eng.Single(x => x.conversationIndex == index);
            }
            else if (Lean.Localization.LeanLocalization.GetOrCreateInstance().CurrentLanguage == "Spanish")
            {
                _currentDialogue = conversationsData.Converstion_Spa.Single(x => x.conversationIndex == index);
            }
        }

        #endregion


    }

}
