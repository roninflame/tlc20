using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    [SerializeField] private Color _tabIdleColor;
    [SerializeField] private Color _tabHoverColor;
    [SerializeField] private Color _tabActiveColor;
    public List<TabButton> TabButtons;
    public TabButton SelectedTab;
    public List<GameObject> ObjectsToSwap;
    private void Start()
    {
        foreach (var item in TabButtons)
        {
            item.SetIdleColors(_tabIdleColor);
            item.SetHoverColors(_tabHoverColor);
            item.SetActiveColors(_tabActiveColor);
        }
    }
    public void Subscribe(TabButton button) { 
        if(TabButtons == null)
        {
            TabButtons = new List<TabButton>();
        }
        //button.Background.color = _tabIdleColor;
        //button.SetIdleColors(_tabIdleColor);
        TabButtons.Add(button);
    }
    public void OnTabEnter(TabButton button)
    {
        ResetTab();
        if(SelectedTab == null || button != SelectedTab)
        {
            //button.Background.color = _tabHoverColor;
            //button.SetHoverColors(_tabHoverColor);
        }
    }
    public void OnTabExit(TabButton button)
    {
        ResetTab();
        
    }
    public void OnTabSelected(TabButton button)
    {
        if(SelectedTab != null)
        {
            SelectedTab.Deselect();
        }

        SelectedTab = button;
        SelectedTab.Select();
        ResetTab();
        //button.Background.color = _tabActiveColor;
        //button.SetActiveColors(_tabActiveColor);

        int index = button.Index;
        for (int i = 0; i < ObjectsToSwap.Count; i++)
        {
            if (i == index)
                ObjectsToSwap[i].SetActive(true);
            else
                ObjectsToSwap[i].SetActive(false);
        }
    }
    public void ResetTab()
    {
        foreach (TabButton item in TabButtons)
        {
            if(SelectedTab != null && item == SelectedTab) { continue; }
            //item.Background.color = _tabIdleColor;
            //item.SetIdleColors(_tabIdleColor);
        }
    }

    public void CerrarVentanas()
    {
        SelectedTab.Deselect();
        foreach (TabButton item in TabButtons)
        {
            //if (SelectedTab != null && item == SelectedTab) { continue; }
            ObjectsToSwap[item.Index].SetActive(false);
            //item.Background.color = _tabIdleColor;
            //item.SetIdleColors(_tabIdleColor);
        }
        SelectedTab = null;
    }
    public void OnNavigate(InputAction.CallbackContext value)
    {
        if (value.started)
        {

        }
    }
}
