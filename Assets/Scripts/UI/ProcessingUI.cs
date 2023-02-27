using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProcessingUI : MonoBehaviour
{
    public GameObject processSlotPrefab;

    public GameObject addRecipeSlotPrefab;
    public GameObject recipeSlotPrefab;
    public GameObject recipePrefab;

    public GameObject processScroll;
    public GameObject recipeSlotScroll;

    public List<Sprite> RecipeIcons = new List<Sprite>();

    
    public List<GameObject> RecipeSlots = new List<GameObject>();
    public List<GameObject> ActiveRecipes = new List<GameObject>();

    public List<GameObject> ActiveSlots = new List<GameObject>();
    
    public BuildingProcessed openedBuilding;

    GameObject addSlot;

    private void Start()
    {

        OpenProcessingMenu();
    }

    public void OpenProcessingMenu() {

        uiManager.instance.ProcessUI.SetActive(true);

        openedBuilding = uiManager.instance.selectedBuilding.GetComponent<BuildingProcessed>();

        SpawnSlots(false);
        SpawnRecipes();

        RemoveCancelButtons();
        AssignCancelButtons();
    }

    public void CloseProcessingMenu() {

        foreach (var slot in ActiveSlots) {
            Destroy(slot);
        }
        foreach (var recipe in ActiveRecipes) {
            Destroy(recipe);
        }
        foreach (var recipeSlot in RecipeSlots) {
            Destroy(recipeSlot);
        }

        ActiveSlots.Clear();
        ActiveRecipes.Clear();
        RecipeSlots.Clear();

        Destroy(addSlot);

        uiManager.instance.selectedBuilding = null;
        uiManager.instance.ProcessUI.SetActive(false);
    }

    public void SpawnSlots(bool single) {
        if (single)
        {
            GameObject slot = Instantiate(processSlotPrefab, processScroll.transform);
            ActiveSlots.Add(slot);
            slot.transform.SetAsLastSibling();
        }
        else
        {
            for (int i = 0; i < openedBuilding.BuildingProcessSlots; i++)
            {
                GameObject slot = Instantiate(processSlotPrefab, processScroll.transform);
                ActiveSlots.Add(slot);
                slot.GetComponent<DropSlot>().slotIndex = i;
            }

            addSlot = Instantiate(addRecipeSlotPrefab, processScroll.transform);
            addSlot.GetComponent<Button>().onClick.AddListener(() => BuySlot());
        }
    }

    void SpawnRecipes()
    {

        for (int i = 0; i < openedBuilding.recipesAvailable.Count; i++)
        {
            GameObject recipeSlot = Instantiate(recipeSlotPrefab, recipeSlotScroll.transform);
            RecipeSlots.Add(recipeSlot);

            for (int y = 0; y < RecipeIcons.Count; y++)
            {
                string[] names = openedBuilding.recipesAvailable[i].FinalProduct.ProcessedGoodieName.Split(" ");

                for (int x = 0; x < name.Length; x++)
                {
                    if (Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(RecipeIcons[y])).Contains(names[x]))
                    {
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

    public void AssignCancelButtons() {

        foreach(var slot in ActiveSlots)
        {
            slot.GetComponent<Button>().onClick.AddListener(() =>
            {
                Recipe recipe = openedBuilding.processingQueue[slot.GetComponent<DropSlot>().slotIndex];

                Destroy(slot.GetComponent<DropSlot>().UsedRecipe);
                ActiveRecipes.Remove(slot.GetComponent<DropSlot>().UsedRecipe);

                openedBuilding.HandleUI(true);

                openedBuilding.processingQueue.RemoveAt(slot.GetComponent<DropSlot>().slotIndex);

                if(slot.GetComponent<DropSlot>().slotIndex == 0){

                    openedBuilding.StopAllCoroutines();
                    openedBuilding.Processing = false;
                    openedBuilding.Process();
                }

            });
        }
    }

    public void RemoveCancelButtons() {
        foreach(var slot in ActiveSlots) {
            slot.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    void BuySlot()
    {
        //Subtract premium currency
        SpawnSlots(true);
        addSlot.transform.SetAsLastSibling();
    }


}
