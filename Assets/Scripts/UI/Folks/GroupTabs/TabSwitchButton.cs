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
            AsignDefaultButton();
        }
    }

    void AsignDefaultButton() {
        
        ThisTabButton.onClick.RemoveAllListeners();
        ThisTabButton.onClick.AddListener(() => {

            TabHandler.instance.OpenTab(TabHandler.instance.DefaultTab);
            if(!TownfolksUI.instance.Grouping)
                TownfolkManager.instance.GroupIndex = 0;
        });
    }
}
