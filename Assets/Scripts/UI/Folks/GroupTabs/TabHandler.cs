using System.Linq;
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
    public GameObject DefaultTab;


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

        tabButton.GetComponent<TabSwitchButton>().ThisButtonText.text = (TabButtons.Count - 1).ToString();
        tabButton.GetComponent<TabSwitchButton>().AssignedTab = tabToAssign;
        tabToAssign.GetComponent<TownfolkGroup>().AssignedTabButton = tabButton;
        tabButton.GetComponent<TabSwitchButton>().ThisTabButton.onClick.AddListener(() => { SwitchTabs(tabToAssign); });
        
        OpenTab(DefaultTab);
    }

    public void SwitchTabs(GameObject tabToSwitch) {

        if(!TownfolksUI.instance.Grouping) {

            UsedTab = tabToSwitch;
            TownfolkManager.instance.GroupIndex = TownfolkManager.instance.TownfolkGroups.IndexOf(tabToSwitch.GetComponent<TownfolkGroup>());
        }

        OpenTab(tabToSwitch);
    }

    public void OpenTab(GameObject tabToOpen) {
        CloseTabs();
        tabToOpen.SetActive(true);
        if(!TownfolksUI.instance.Grouping)
            UsedTab = tabToOpen;

        tabToOpen.GetComponent<TownfolkGroup>().AssignedTabButton.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    public void CloseTabs() {
        for (int i = 0; i < Tabs.Count; i++) {
            if(DefaultTab.activeInHierarchy) {

                DefaultTab.GetComponent<TownfolkGroup>().AssignedTabButton.transform.localScale = new Vector3(1f, 1f, 1f);
                DefaultTab.SetActive(false);
            }
            if(Tabs[i].activeInHierarchy) {

                Tabs[i].GetComponent<TownfolkGroup>().AssignedTabButton.transform.localScale = new Vector3(1f, 1f, 1f);
                Tabs[i].SetActive(false);
            }
        }
    }
}
