using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {
    
    [SerializeField] private Canvas canvas;

    public Recipe usedRecipe;
    public GameObject usedSlot;
    
    MobileCameraController cameraController;
    
    public int recipeIndex;
    public bool staticRecipe;

    DropSlot slot;
    GameObject instRecipe;

    private void Start() {
        Invoke("SetCanvas", 0.1f);
        cameraController = Camera.main.GetComponentInParent<MobileCameraController>();
    }

    void SetCanvas(){
        if(staticRecipe)
            canvas = transform.parent.parent.parent.parent.parent.GetComponent<Canvas>();
        else
            canvas = transform.GetComponent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        if(uiManager.instance.selectedBuilding.GetComponent<BuildingProcessed>().WorkingFolks.Count > 0) {

            instRecipe = Instantiate(eventData.pointerPressRaycast.gameObject, eventData.position, Quaternion.identity);
            instRecipe.transform.SetParent(canvas.transform);
            instRecipe.GetComponent<DragDrop>().staticRecipe = false;
            eventData.pointerDrag = instRecipe;

            instRecipe.transform.localScale = new Vector3( 1.3f, 1.3f, 1.3f);
            instRecipe.GetComponent<CanvasGroup>().blocksRaycasts = false;
            instRecipe.GetComponent<CanvasGroup>().alpha = 0.6f;

            Debug.Log(instRecipe);
            instRecipe.GetComponent<DragDrop>().usedRecipe = uiManager.instance.processingUI.openedBuilding.recipesAvailable[recipeIndex];
            
            cameraController.enabled = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!staticRecipe) {
            
            transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        if(uiManager.instance.selectedBuilding.GetComponent<BuildingProcessed>().WorkingFolks.Count > 0) {
            Debug.Log("OnEndDrag");
            transform.localScale = new Vector3( 1f, 1f, 1f);
            
                Debug.LogWarning(eventData.pointerCurrentRaycast.gameObject);
                BuildingProcessed buildingProcessed = uiManager.instance.processingUI.openedBuilding;

                if(eventData.pointerCurrentRaycast.gameObject != null){
                    if(!eventData.pointerCurrentRaycast.gameObject.GetComponent<DropSlot>() || buildingProcessed.processingQueue.Count >= uiManager.instance.processingUI.ActiveSlots.Count){
                        
                        Destroy(gameObject);
                    } else {

                        if(!buildingProcessed.processingQueue.Any()){

                            transform.position = uiManager.instance.processingUI.ActiveSlots[0].transform.position;
                            slot = uiManager.instance.processingUI.ActiveSlots[0].GetComponent<DropSlot>();
                        } else {

                            transform.position = uiManager.instance.processingUI.ActiveSlots[buildingProcessed.processingQueue.Count].transform.position;
                            slot = uiManager.instance.processingUI.ActiveSlots[buildingProcessed.processingQueue.Count].GetComponent<DropSlot>();
                        }

                        slot.UsedRecipe = gameObject;
                        slot.slotTimer.gameObject.SetActive(true);
                        usedSlot = slot.gameObject;
                        
                        buildingProcessed.processingQueue.Add(gameObject);
                        
                        buildingProcessed.ProcessTime = buildingProcessed.recipesAvailable[recipeIndex].processingTime;
                        slot.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = buildingProcessed.recipesAvailable[recipeIndex].processingTime.ToString();
                        
                        if(!buildingProcessed.Processing) buildingProcessed.Process();
                    }
                } else {
                    Destroy(gameObject);
                }
            
            cameraController.enabled = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if(uiManager.instance.selectedBuilding.GetComponent<BuildingProcessed>().WorkingFolks.Count > 0) {
            Debug.Log("OnPointerDown");
        } else {

            uiManager.instance.OpenPopUp("This building got no workers? Do you wanna assign?");
            PopUp.instance.AssignButtons(() => {
                ProcessingUI.instance.CloseProcessingMenu(true, false);
                TownfolksUI.instance.OpenFolkPanel(true);
                uiManager.instance.ClosePopUp();
            }, () => uiManager.instance.ClosePopUp());
        }
    }
}
