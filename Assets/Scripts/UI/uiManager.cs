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

    public Sprite PlusSprite;
    public Sprite CancelSprite;
    public List<Sprite> RecipeIcons = new List<Sprite>();

    public Button ConfirmButton;

[Header("Production")]
    public ProductionUI productionUI;
    public GameObject ProdUI;

[Header("Processing")]
    public ProcessingUI processingUI;
    public GameObject ProcessUI;

[Header("PopUP")]
    public GameObject PopUpPanel;

    private void Awake() {
        
        instance = this;
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

        CloseUI();
        WorldSpaceCanvas.SetActive(true);

        buildingMenu.SetActive(true);
        buildingMenu.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        buildingMenu.transform.DOScale(new Vector3(1, 1, 1), .5f);
        

        BuildingMenu.instance.BuildingPanelButton.onClick.AddListener(() => {

            if(selectedBuilding.GetComponent<BuildingProcessed>()){

                processingUI.OpenProcessingMenu();
            }
            if(selectedBuilding.GetComponent<BuildingProduction>()){
            
                productionUI.OpenProductionPanel();
            }
        });

        BuildingMenu.instance.WorkerPanelButton.onClick.AddListener(() => TownfolksUI.instance.OpenFolkPanel(true));
        
        buildingMenu.transform.position = selectedBuilding.transform.position;
    }

    public void CloseBuildingMenu() {

        buildingMenu.GetComponent<BuildingMenu>().UpgradeButton.onClick.RemoveAllListeners();
        buildingMenu.GetComponent<BuildingMenu>().BuildingPanelButton.onClick.RemoveAllListeners();
        buildingMenu.GetComponent<BuildingMenu>().WorkerPanelButton.onClick.RemoveAllListeners();

        buildingMenu.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.2f).onComplete = (() => {

            WorldSpaceCanvas.SetActive(false);
            buildingMenu.SetActive(false);
        });
    }

    public void OpenPopUp(string text) {

        PopUpPanel.SetActive(true);
        PopUp.instance.SetText(text);
    }

    public void ClosePopUp() {

        PopUpPanel.SetActive(false);
    }

    public void CloseUI() {

        if(buildingMenu.activeInHierarchy){
            CloseBuildingMenu();
        }
        if(ProdUI.activeInHierarchy) {
            productionUI.CloseProductionPanel();
        }
        if(ProcessUI.activeInHierarchy){
            processingUI.CloseProcessingMenu(true, true);
        }
        if(TownfolksUI.instance.FolkUI.activeInHierarchy) {
            TownfolksUI.instance.CloseFolkPanel();
        }
        if(TownfolksUI.instance.FolkDetailUI.gameObject.activeInHierarchy) {
            TownfolksUI.instance.CloseFolkDetail();
        }
    }

    public void ButtonClickEffect(Button button, float amount, float time) {

        button.transform.DOScale(new Vector3(amount, amount, amount), time).onComplete = () => {
            button.transform.DOScale(new Vector3(1f, 1f, 1f), .1f);
        };
    }
}
