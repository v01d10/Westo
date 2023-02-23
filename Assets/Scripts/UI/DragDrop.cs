using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {
    
    [SerializeField] private Canvas canvas;
    GameObject instRecipe;
    public GameObject usedSlot;
    public int recipeIndex;
    public bool staticRecipe;

    private void Start() {
        Invoke("SetCanvas", 0.1f);
    }

    void SetCanvas(){
        if(staticRecipe)
            canvas = transform.parent.parent.parent.parent.parent.GetComponent<Canvas>();
        else
            canvas = transform.GetComponent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        
        instRecipe = Instantiate(eventData.pointerPressRaycast.gameObject, eventData.position, Quaternion.identity);
        instRecipe.transform.SetParent(canvas.transform);
        instRecipe.GetComponent<DragDrop>().staticRecipe = false;
        eventData.pointerDrag = instRecipe;

        instRecipe.transform.localScale = new Vector3( 1.3f, 1.3f, 1.3f);
        instRecipe.GetComponent<CanvasGroup>().blocksRaycasts = false;
        instRecipe.GetComponent<CanvasGroup>().alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!staticRecipe) {
            transform.position = eventData.position;
            // GetComponent<RectTransform>().anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        Debug.Log("OnEndDrag");
        transform.localScale = new Vector3( 1f, 1f, 1f);
        
            Debug.LogWarning(eventData.pointerCurrentRaycast.gameObject);

            if(eventData.pointerCurrentRaycast.gameObject != null){
                if(!eventData.pointerCurrentRaycast.gameObject.GetComponent<DropSlot>()){
                    
                    Destroy(gameObject);
                } else {
                    BuildingProcessed buildingProcessed = uiManager.instance.processingUI.openedBuilding;

                    transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;

                    eventData.pointerCurrentRaycast.gameObject.GetComponent<DropSlot>().Occupado = true;
                    eventData.pointerCurrentRaycast.gameObject.GetComponent<DropSlot>().UsedRecipe = gameObject;
                    eventData.pointerCurrentRaycast.gameObject.GetComponent<DropSlot>().slotTimer.gameObject.SetActive(true);
                    
                    uiManager.instance.processingUI.ActiveRecipes.Add(gameObject);
                    usedSlot = eventData.pointerCurrentRaycast.gameObject;
                    
                    buildingProcessed.processingQueue.Add(buildingProcessed.recipesAvailable[recipeIndex]);

                    buildingProcessed.ProcessTime = buildingProcessed.recipesAvailable[recipeIndex].processingTime;
                    eventData.pointerCurrentRaycast.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = buildingProcessed.recipesAvailable[recipeIndex].processingTime.ToString();
                    if(!buildingProcessed.Processing) buildingProcessed.Process();

                }
            } else {
                Destroy(gameObject);
            }
        
    }

    public void OnPointerDown(PointerEventData eventData) {
        Debug.Log("OnPointerDown");
        
    }

}
