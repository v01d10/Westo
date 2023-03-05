using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ProcessingUI : MonoBehaviour
{
    public GameObject processSlotPrefab;

    public GameObject addRecipeSlotPrefab;
    public GameObject recipeSlotPrefab;
    public GameObject recipePrefab;

    public GameObject processScroll;
    public GameObject recipeSlotScroll;

    public List<GameObject> RecipeSlots = new List<GameObject>();
    public List<GameObject> ActiveSlots = new List<GameObject>();
    
    public BuildingProcessed openedBuilding;

    GameObject addSlot;

    public void OpenProcessingMenu() {

        uiManager.instance.CloseBuildingMenu();

        uiManager.instance.ProcessUI.SetActive(true);

        openedBuilding = uiManager.instance.selectedBuilding.GetComponent<BuildingProcessed>();
        
        CloseProcessingMenu(false);

        SpawnSlots(false);
        SpawnRecipes();
        uiManager.instance.ProcessUI.transform.localScale = new Vector3(.7f, .7f, .7f);
        uiManager.instance.ProcessUI.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f).onComplete = () => SpawnProcessing();

        RemoveCancelButtons();
        AssignCancelButtons();

    }

    public void CloseProcessingMenu(bool Complete) {

        foreach (var slot in ActiveSlots) {
            Destroy(slot);
        }
        foreach (var recipe in openedBuilding.processingQueue) {
            recipe.SetActive(false);
        }
        foreach (var recipeSlot in RecipeSlots) {
            Destroy(recipeSlot);
        }

        ActiveSlots.Clear();
        RecipeSlots.Clear();

        Destroy(addSlot);

        if(Complete){
            
            uiManager.instance.selectedBuilding = null;
            uiManager.instance.ProcessUI.transform.DOScale(new Vector3(.1f, .1f, .1f), 0.2f).onComplete = () => {
                uiManager.instance.ProcessUI.SetActive(false);
            };
        }

    }

    public void SpawnSlots(bool single) {
        if (single) {

            GameObject slot = Instantiate(processSlotPrefab, processScroll.transform);
            ActiveSlots.Add(slot);
            slot.transform.SetAsLastSibling();
        } else {

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

            for (int y = 0; y < uiManager.instance.RecipeIcons.Count; y++)
            {
                string[] names = openedBuilding.recipesAvailable[i].FinalProduct.ProcessedGoodieName.Split(" ");

                for (int x = 0; x < name.Length; x++) {
                    
                    if (Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(uiManager.instance.RecipeIcons[y])).Contains(names[x])) {
                        GameObject recipe = Instantiate(recipePrefab, recipeSlot.transform);
                        recipe.GetComponent<Image>().sprite = uiManager.instance.RecipeIcons[y];
                        recipe.GetComponent<DragDrop>().staticRecipe = true;
                        recipe.GetComponent<DragDrop>().recipeIndex = i;
                        break;
                    }
                }
            }
        }
    }

    void SpawnProcessing() {

        if(openedBuilding.processingQueue.Any()){

            for (int i = 0; i < openedBuilding.processingQueue.Count; i++) {
                
                openedBuilding.processingQueue[i].SetActive(true);
                
                ActiveSlots[i].GetComponent<DropSlot>().slotTimer.gameObject.SetActive(true);
                if(i > 0)
                    ActiveSlots[i].GetComponent<DropSlot>().slotTimer.text = openedBuilding.processingQueue[i].GetComponent<DragDrop>().usedRecipe.processingTime.ToString();
                else
                    ActiveSlots[i].GetComponent<DropSlot>().slotTimer.text = openedBuilding.ProcessTimer.ToString();

            }

            StartCoroutine("invokeHandleUI");
        }
    }

    IEnumerator invokeHandleUI() {
        yield return new WaitForSeconds(0.01f);
        openedBuilding.HandleUI(true);
    }

    public void AssignCancelButtons() {

        foreach(var slot in ActiveSlots)
        {
            slot.GetComponent<Button>().onClick.AddListener(() =>
            {
                if(openedBuilding.processingQueue.Any()){

                    Recipe recipe = openedBuilding.processingQueue[slot.GetComponent<DropSlot>().slotIndex].GetComponent<DragDrop>().usedRecipe;

                    Destroy(slot.GetComponent<DropSlot>().UsedRecipe);

                    openedBuilding.processingQueue.RemoveAt(slot.GetComponent<DropSlot>().slotIndex);

                    openedBuilding.HandleUI(true);

                    if(slot.GetComponent<DropSlot>().slotIndex == 0){

                        openedBuilding.StopAllCoroutines();
                        openedBuilding.Processing = false;
                        openedBuilding.Process();
                    }
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
        openedBuilding.BuildingProcessSlots++;
    }


}
