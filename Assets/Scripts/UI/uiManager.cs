using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class uiManager : MonoBehaviour
{
    public static uiManager instance;

    public GameObject WorldSpaceCanvas;
    public GameObject buildingMenu;
    public GameObject selectedBuilding;

    public Sprite CancelSprite;
    public List<Sprite> RecipeIcons = new List<Sprite>();

[Header("Production")]
    public ProductionUI productionUI;
    public GameObject ProdUI;

[Header("Processing")]
    public ProcessingUI processingUI;
    public GameObject ProcessUI;

    private void Awake() {
        instance = this;
        
        processingUI = GetComponent<ProcessingUI>();
    }

    public static bool IsMouseOverUI(){
        return EventSystem.current.IsPointerOverGameObject();
    }

    public static bool IsMouseOverUIIgnores(){
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        for (int i = 0; i < raycastResults.Count; i++) {
            if(raycastResults[i].gameObject.GetComponent<MouseClickThrough>() != null) {
                raycastResults.RemoveAt(i);
                i--;
            }
        }

        return raycastResults.Count > 0;
    }

    public void OpenBuildingMenu() {

        WorldSpaceCanvas.SetActive(true);

        buildingMenu.SetActive(true);
        buildingMenu.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        buildingMenu.transform.DOScale(new Vector3(1, 1, 1), .5f);
        
        if(selectedBuilding.GetComponent<BuildingProcessed>()){

            BuildingMenu.instance.BuildingPanelButton.onClick.AddListener(() => processingUI.OpenProcessingMenu());
        }

        if(selectedBuilding.GetComponent<Building>()){
            
            BuildingMenu.instance.BuildingPanelButton.onClick.AddListener(() => productionUI.OpenBuildingPanel());
        }

        Vector3 buildindPosition = new Vector3(selectedBuilding.transform.position.x, selectedBuilding.transform.position.y, selectedBuilding.transform.position.z - 2);
        buildingMenu.transform.position = selectedBuilding.transform.position;
    }

    public void CloseBuildingMenu() {

        BuildingMenu.instance.UpgradeButton.onClick.RemoveAllListeners();
        BuildingMenu.instance.BuildingPanelButton.onClick.RemoveAllListeners();
        BuildingMenu.instance.WorkerPanelButton.onClick.RemoveAllListeners();

        buildingMenu.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.2f).onComplete = (() => {

            WorldSpaceCanvas.SetActive(false);
            buildingMenu.SetActive(false);
        });

    }


}
