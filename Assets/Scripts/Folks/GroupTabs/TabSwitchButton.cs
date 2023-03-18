using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TabSwitchButton : MonoBehaviour
{
    public Button ThisTabButton;

    public GameObject AssignedTab;

    public TextMeshProUGUI ThisButtonText;

    public bool DefaultButton;

    private void Awake() {
        ThisTabButton = GetComponent<Button>();
        ThisButtonText = GetComponentInChildren<TextMeshProUGUI>();

        if(DefaultButton) {
            ASsignButton();
        }
    }

    void ASsignButton() {
        ThisTabButton.onClick.AddListener(() => {
            TabHandler.instance.CloseTabs();
            TownfolksUI.instance.DefaultTab.SetActive(true);
            TabHandler.instance.UsedTab = TownfolksUI.instance.DefaultTab;
        });
    }
}
