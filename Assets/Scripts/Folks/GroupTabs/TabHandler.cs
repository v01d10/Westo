using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabHandler : MonoBehaviour
{
    public static TabHandler instance;
    
    public Transform TabButtonHolder;

    public GameObject TabButtonPrefab;
    public GameObject TabPrefab;

    public GameObject UsedTab;

    public List<TabSwitchButton> TabButtons = new List<TabSwitchButton>();
    public List<GameObject> Tabs = new List<GameObject>();

    private void Awake() {
        instance = this;
    }

    public void CreateTab() {
        
        GameObject tab = Instantiate(TabPrefab, TownfolksUI.instance.FolkScrollBackground.transform);
        Tabs.Add(tab);
        TownfolkManager.instance.TownfolkGroups.Add(tab.GetComponent<TownfolkGroup>());
        TownfolkManager.instance.GroupIndex = TownfolkManager.instance.TownfolkGroups.IndexOf(tab.GetComponent<TownfolkGroup>());
        UsedTab = tab;

        CreateTabButton(tab);
    }

    public void CreateTabButton(GameObject tabToAssign) {

        GameObject tabButton = Instantiate(TabButtonPrefab, TabButtonHolder);
        TabButtons.Add(tabButton.GetComponent<TabSwitchButton>());

        tabButton.GetComponent<TabSwitchButton>().ThisButtonText.text = (TabButtons.Count).ToString();
        tabButton.GetComponent<TabSwitchButton>().AssignedTab = tabToAssign;
        tabToAssign.GetComponent<TownfolkGroup>().AssignedTabButton = tabButton;
        tabButton.GetComponent<TabSwitchButton>().ThisTabButton.onClick.AddListener(() => { SwitchTabs(tabToAssign); });
    }

    public void SwitchTabs(GameObject tabToSwitch) {

        if(!TownfolksUI.instance.Grouping){

            UsedTab.SetActive(false);
            tabToSwitch.SetActive(true);
        }

        UsedTab = tabToSwitch;
    }

    public void CloseTabs() {
        for (int i = 0; i < Tabs.Count; i++) {
            Tabs[i].SetActive(false);
        }
    }
}
