using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SideMenu : MonoBehaviour
{
    public static SideMenu instance;

    public GameObject SideMenuButton;

    public Button FolkPanelButton;

    bool Opened;

    private void Awake() {
        instance = this;
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    private void OnDestroy() {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameState state) {
        HideSideMenuButton();
    }

    private void Start() {

        SideMenuButton.GetComponent<Button>().onClick.AddListener(() => {
            MoveSideMenu();
        });

        AssignSideMenuButtons();
    }

    public void MoveSideMenu() {
        Vector3 defaultPosition = SideMenuButton.transform.position;

        if(!Opened){

            SideMenuButton.transform.DOLocalMoveX(800, .3f);
            Opened = true;
        } else {

            SideMenuButton.transform.DOLocalMoveX(930, .3f);
            Opened = false;
        }
    }

    public void AssignSideMenuButtons() {
        FolkPanelButton.onClick.AddListener(() => { TownfolksUI.instance.OpenFolkPanel(false); MoveSideMenu(); });
    }

    public void HideSideMenuButton() {
        if(SideMenuButton.activeInHierarchy) {
            SideMenuButton.SetActive(false);
        } else {
            SideMenuButton.SetActive(true);
        }  
        
    }
}
