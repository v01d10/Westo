using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingProcessed : MonoBehaviour {

    public List<Recipe> recipesAvailable = new List<Recipe>();
    public List<GameObject> processingQueue = new List<GameObject>(); 

    List<GameObject> slots;
    
    public Townfolk AssignedWorker;
    
    public float BuildingHealth;
    public float BuildingProcessSlots;
    public float BuildingLevel;
    public float UpgradePrice;

    public float ProcessTime;
    public float ProcessTimer;
    public bool Processing;

    ProcessingUI processingUI;
    
    private void Start() {
        
        processingUI = uiManager.instance.processingUI;

        Process();
    }

    public void Process() {
        if(processingQueue.ElementAtOrDefault(0)) {
            
            StopAllCoroutines();
            StartCoroutine("ProcessGoodies", processingQueue[0].GetComponent<DragDrop>().usedRecipe);
        } else {
            return;
        }
    }

    public IEnumerator ProcessGoodies(Recipe recipe) {
        processingUI = uiManager.instance.processingUI;
        
        Processing = true;
        ProcessTimer = ProcessTime;
        StartCoroutine("processTimer");

        yield return new WaitForSeconds(recipe.processingTime);
        
        HandleGoodies(recipe);
        HandleUI(false);
        
        Processing = false;
        Process();
    }

    void HandleGoodies(Recipe recipe) {

        for (int i = 0; i < recipe.ingredients.Count; i++) {

            for (int y = 0; y < Player.instance.PlayerWarehouse.StoredGoodies.Count; y++) {
                
                if(recipe.ingredients[i].GoodieName == Player.instance.PlayerWarehouse.StoredGoodies[y].GoodieName){

                    if(Player.instance.PlayerWarehouse.StoredGoodies[y].GoodieAmount >= recipe.ingredientCost0) {

                        Player.instance.PlayerWarehouse.StoredGoodies[y].GoodieAmount -= recipe.ingredientCost0;
                    }

                    if(Player.instance.PlayerWarehouse.StoredGoodies[y].GoodieAmount >= recipe.ingredientCost1) {

                        Player.instance.PlayerWarehouse.StoredGoodies[y].GoodieAmount -= recipe.ingredientCost1;
                    }

                    if(Player.instance.PlayerWarehouse.StoredGoodies[y].GoodieAmount >= recipe.ingredientCost2) {

                        Player.instance.PlayerWarehouse.StoredGoodies[y].GoodieAmount -= recipe.ingredientCost2;
                    }
                }
            }
        }
   
        recipe.FinalProduct.ProcessedGoodieAmount += (BuildingLevel * 1.3f);
        
    }

    public void HandleUI(bool Cancel){
        if(!Cancel){

            Destroy(processingQueue[0]);
            processingQueue.RemoveAt(0);
        }

        if(uiManager.instance.ProcessUI.activeInHierarchy && processingUI.openedBuilding == this){     

            slots = processingUI.ActiveSlots;
            
            for (int i = 0; i < processingQueue.Count; i++) {

                processingQueue[i].GetComponent<DragDrop>().usedSlot = slots[i];
                slots[i].GetComponent<DropSlot>().UsedRecipe = processingQueue[i];
                processingQueue[i].transform.position = processingQueue[i].GetComponent<DragDrop>().usedSlot.transform.position;            
            
                if(slots.Count > processingQueue.Count) {
                    slots[processingQueue.Count].GetComponent<DropSlot>().UsedRecipe = null;
                    slots[processingQueue.Count].GetComponent<DropSlot>().slotTimer.gameObject.SetActive(false);
                }
            }

            if(processingQueue.Count == 0) {
                foreach (var slot in slots) {
                    slot.GetComponent<DropSlot>().slotTimer.gameObject.SetActive(false);
                }
            }
            
            processingUI.RemoveCancelButtons();
            processingUI.AssignCancelButtons();
            
        }
    }

    IEnumerator processTimer() {
        
        if(uiManager.instance.ProcessUI.activeInHierarchy && processingUI.openedBuilding == this) {
            processingUI.ActiveSlots[0].GetComponent<DropSlot>().slotTimer.text = ProcessTimer.ToString();
        }
        
        if (ProcessTimer >= 1) {

            yield return new WaitForSeconds(1);
            ProcessTimer--;
            StartCoroutine("processTimer");
        } 
    }

    private void OnMouseDown() {
        if(!uiManager.IsMouseOverUIIgnores()){

            uiManager.instance.selectedBuilding = this.gameObject;
            uiManager.instance.OpenBuildingMenu();
        }
    }

}
