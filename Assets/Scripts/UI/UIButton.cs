using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[Serializable]
public class MyButtonEvent : UnityEvent<GameObject>
{

}

[Serializable]
public class MyButtonInt : UnityEvent<int>
{

}


public class UIButton : MonoBehaviour
{


    private TextMeshProUGUI _text;
    public GameObject parentGO;
    [SerializeField] private int _textNormalSize = 80;
    [SerializeField] private int _textHoverSize = 90;
    [SerializeField] private Color _textIdleColor;
    [SerializeField] private Color _textHoverColor;
    [SerializeField] private Color _textActiveColor;

    public MyButtonEvent OnClik;
    public MyButtonInt OnClikInt;
    public int index;
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        ResetColor();
    }
    public void ResetColor()
    {
        _text.color = _textIdleColor;
        _text.fontSize = _textNormalSize;
    }
    
    public void OnPointerClick()
    {
        _text.color = _textActiveColor;
        _text.fontSize = _textHoverSize;
        OnClik?.Invoke(parentGO);
        OnClikInt?.Invoke(index);

        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/UISelect");
    }
    //public void OnPointerEnter()
    //{
    //    _text.color = _textHoverColor;
    //    _text.fontSize = _textHoverSize;

    //    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/UIHover");
    //}

    //public void OnPointerExit()
    //{
    //    _text.color = _textIdleColor;
    //    _text.fontSize = _textNormalSize;
    //}

    public void OnSelect()
    {
        _text.color = _textHoverColor;
        _text.fontSize = _textHoverSize;

        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/UIHover");
    }

    public void OnDeselect()
    {
        _text.color = _textIdleColor;
        _text.fontSize = _textNormalSize;
    }
}
