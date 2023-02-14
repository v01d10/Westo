using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingProcessed : MonoBehaviour {

    public List<Recipe> recipesAvailable = new List<Recipe>();
    public List<Recipe> processingQueue = new List<Recipe>(); 

    public float BuildingHealth;

    public float BuildingLevel;
    public float UpgradePrice;
    
    private void Start() {
        Process();
    }

    public void Process() {
        if(processingQueue.ElementAtOrDefault(0)) {

            StartCoroutine(ProcessGoodies(processingQueue[0]));
        } else {
            Process();
        }
    }

    public IEnumerator ProcessGoodies(Recipe recipe) {

        yield return new WaitForSeconds(recipe.processingTime);
        
        
        for (int i = 0; i < recipe.ingredients.Count; i++) {

            for (int y = 0; y < Player.instance.PlayerWarehouse.StoredGoodies.Count; y++) {
            
                if(recipe.ingredients[i].GoodieName == Player.instance.PlayerWarehouse.StoredGoodies[i].GoodieName){
                    if(Player.instance.PlayerWarehouse.StoredGoodies[i].GoodieAmount >= recipe.ingredientCost0) {

                        Player.instance.PlayerWarehouse.StoredGoodies[i].GoodieAmount -= recipe.ingredientCost0;
                    }

                    if(Player.instance.PlayerWarehouse.StoredGoodies[i].GoodieAmount >= recipe.ingredientCost1) {

                        Player.instance.PlayerWarehouse.StoredGoodies[i].GoodieAmount -= recipe.ingredientCost1;
                    }

                    if(Player.instance.PlayerWarehouse.StoredGoodies[i].GoodieAmount >= recipe.ingredientCost2) {

                        Player.instance.PlayerWarehouse.StoredGoodies[i].GoodieAmount -= recipe.ingredientCost2;
                    }

                }
            
            }
        }

        recipe.FinalProduct.ProcessedGoodieAmount += (BuildingLevel * 1.3f);
        processingQueue.RemoveAt(0);
        Process();
    }

}
