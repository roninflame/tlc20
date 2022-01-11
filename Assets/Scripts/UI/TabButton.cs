using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public TabGroup TabGrp;
    public int Index;
    public Image Background;
    public UnityEvent OnTabSelected;
    public UnityEvent OnTabDeselected;
    private Button _button;
    void Awake()
    {
        //Background.color = new Color(Background.color.r, Background.color.g, Background.color.b, 0f);
        _button = GetComponent<Button>();
        //TabGrp.Subscribe(this);

    }
    public void SetIdleColors(Color color)
    {
        //Background.color = color;
        ColorBlock colors = _button.colors;
        colors.normalColor = color;
        _button.colors = colors;
    }
    public void SetHoverColors(Color color)
    {
        //Background.color = color;
        ColorBlock colors = _button.colors;
        colors.highlightedColor = color;
        colors.selectedColor = color;
        _button.colors = colors;
    }
    public void SetActiveColors(Color color)
    {
        //Background.color = color;
        ColorBlock colors = _button.colors;
        colors.pressedColor = color;
        _button.colors = colors;
        //colors.selectedColor = color;
    }
    public void Select()
    {
        if (OnTabSelected != null)
        {
            OnTabSelected.Invoke();
        }
    }
    public void Deselect()
    {
        if (OnTabDeselected != null)
        {
            OnTabDeselected.Invoke();
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        TabGrp.OnTabSelected(this);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        TabGrp.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TabGrp.OnTabExit(this);
    }

    
}
