using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ProcessingUI : MonoBehaviour
{
    public GameObject processSlotPrefab;

    public GameObject recipeSlotPrefab;
    public GameObject recipePrefab;

    public GameObject processScroll;
    public GameObject recipeSlotScroll;

    public Sprite defaultSlotIcon;

    public List<Sprite> RecipeIcons = new List<Sprite>();
    
    public List<GameObject> ActiveSlots = new List<GameObject>();
    public List<GameObject> ActiveRecipes = new List<GameObject>(); 

    public BuildingProcessed openedBuilding;


    private void Start() {
        OpenProcessingMenu();
    }

    public void OpenProcessingMenu() {
        uiManager.instance.ProcessUI.SetActive(true);


        openedBuilding = uiManager.instance.selectedBuilding.GetComponent<BuildingProcessed>();
        
        SpawnSlots(false);
        SpawnRecipes();
        
    }

    public void SpawnSlots(bool single){
        if(single){
                GameObject slot = Instantiate(processSlotPrefab, processScroll.transform);
                ActiveSlots.Add(slot);
                slot.transform.SetAsLastSibling();
        } else {
            for (int i = 0; i < openedBuilding.BuildingProcessSlots; i++) {
                GameObject slot = Instantiate(processSlotPrefab, processScroll.transform);
                ActiveSlots.Add(slot);
            }

        }
    }

    void SpawnRecipes(){

        for (int i = 0; i < openedBuilding.recipesAvailable.Count; i++) {
            GameObject recipeSlot = Instantiate(recipeSlotPrefab, recipeSlotScroll.transform);

            for (int y = 0; y < RecipeIcons.Count; y++) {
                string[] names = openedBuilding.recipesAvailable[i].FinalProduct.ProcessedGoodieName.Split(" ");
                
                for (int x = 0; x < name.Length; x++) {
                    if(Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(RecipeIcons[y])).Contains(names[x])) {
                        GameObject recipe = Instantiate(recipePrefab, recipeSlot.transform);
                        recipe.GetComponent<Image>().sprite = RecipeIcons[y];
                        recipe.GetComponent<DragDrop>().staticRecipe = true; 
                        recipe.GetComponent<DragDrop>().recipeIndex = i; 
                        Debug.LogWarning("Found same name" + names[x]);
                        break;
                    }                    
                }
            }
        }
    }
}
