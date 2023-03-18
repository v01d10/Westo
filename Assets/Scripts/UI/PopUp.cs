using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class PopUp : MonoBehaviour
{
    public static PopUp instance;

    public TextMeshProUGUI PopUpText;

    public Button AcceptButton;
    public Button CancelButton;

    private void Awake() {
        instance = this;
    }

    public void SetText(string text) {

        PopUpText.text = text;
    }

    public void AssignButtons(UnityAction AcceptAction, UnityAction CancelAction) {
       
        AcceptButton.onClick.RemoveAllListeners();
        CancelButton.onClick.RemoveAllListeners();

        AcceptButton.onClick.AddListener(AcceptAction);
        CancelButton.onClick.AddListener(CancelAction);
    }
}
