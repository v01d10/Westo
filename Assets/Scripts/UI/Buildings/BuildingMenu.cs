using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenu : MonoBehaviour {

    public static BuildingMenu instance;

    public Button UpgradeButton;
    public Button BuildingPanelButton;
    public Button WorkerPanelButton;

    private void Awake() {
        
        instance = this;
    }
}
