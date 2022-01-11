using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class UILevelSelect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private int _textNormalSize = 100;
    [SerializeField] private int _textHoverSize = 120;

    public UnityEvent OnClick;
    public void OnPointerClick()
    {
        _text.fontSize = _textHoverSize;
        OnClick?.Invoke();

        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/UISelect");
    }
    public void OnPointerEnter()
    {
        _text.fontSize = _textHoverSize;

        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/UIHover");
    }

    public void OnPointerExit()
    {
        _text.fontSize = _textNormalSize;
    }
}
