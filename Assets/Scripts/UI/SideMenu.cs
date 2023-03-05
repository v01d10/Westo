using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SideMenu : MonoBehaviour
{
    public GameObject SideMenuButton;

    public Button FolkPanelButton;

    bool Opened;

    private void Start() {

        SideMenuButton.GetComponent<Button>().onClick.AddListener(() => {

            Vector3 defaultPosition = SideMenuButton.transform.position;

            if(!Opened){

                SideMenuButton.transform.DOLocalMoveX(800, .3f);
                Opened = true;
            } else {

                SideMenuButton.transform.DOLocalMoveX(930, .3f);
                Opened = false;
            }
        });

        FolkPanelButton.onClick.AddListener(() => TownfolksUI.instance.OpenFolkPanel());
    }
}
