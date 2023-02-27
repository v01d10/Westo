using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingProcessed : MonoBehaviour {

    public List<Recipe> recipesAvailable = new List<Recipe>();
    public List<Recipe> processingQueue = new List<Recipe>(); 

    List<GameObject> recipes;
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
            StartCoroutine("ProcessGoodies", processingQueue[0]);
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
        
        processingQueue.RemoveAt(0);

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
        if(processingUI.isActiveAndEnabled && processingUI.openedBuilding == this){     

            recipes = processingUI.ActiveRecipes;
            slots = processingUI.ActiveSlots;
            
            if(!Cancel){

                Destroy(recipes[0]);
                recipes.RemoveAt(0);
            }

            for (int i = 0; i < recipes.Count; i++) {

                recipes[i].GetComponent<DragDrop>().usedSlot = slots[i];
                slots[i].GetComponent<DropSlot>().UsedRecipe = recipes[i];
                recipes[i].transform.position = recipes[i].GetComponent<DragDrop>().usedSlot.transform.position;            
            }

            for (int i = 0; i < recipes.Count; i++) {
                if(slots.Count > recipes.Count) {
                    slots[recipes.Count].GetComponent<DropSlot>().UsedRecipe = null;
                    slots[recipes.Count].GetComponent<DropSlot>().slotTimer.gameObject.SetActive(false);
                }
            }

            if(recipes.Count == 0) {
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

        uiManager.instance.selectedBuilding = this.gameObject;
        uiManager.instance.processingUI.OpenProcessingMenu();

        processingUI.RemoveCancelButtons();
        processingUI.AssignCancelButtons();

    }

}
