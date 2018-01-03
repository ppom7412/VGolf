using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TapButton : MonoBehaviour, IPointerClickHandler {
    public GameObject menuPanel;

    private void Start()
    {
        menuPanel.SetActive(false);
    }

    void OnSelect()
    {
        if (menuPanel.active)
        {
            menuPanel.SetActive(false);
        }
        else
        {
            menuPanel.SetActive(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (menuPanel.active)
        {
            //Debug.Log("Active On");
            menuPanel.SetActive(false);
        }
        else
        {
            //Debug.Log("Active Off");
            menuPanel.SetActive(true);
        }
    }
}
