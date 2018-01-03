using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerClickHandler {
    public string menuName;
    public GameObject panel;
    void OnSelect()
    {
        if (menuName == "reset")
            UIManager.instance.PushGameReset();

        if (menuName == "start")
            UIManager.instance.PushGameStart();

        panel.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (menuName == "reset")
            UIManager.instance.PushGameReset();

        if (menuName == "start")
            UIManager.instance.PushGameStart();

        panel.SetActive(false);
    }
}
