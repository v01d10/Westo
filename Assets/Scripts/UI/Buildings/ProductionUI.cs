using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class ProductionUI : MonoBehaviour {

    Building OpenedBuilding; 

[Header("Main Panel")]
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI ProdRateText;

    public TextMeshProUGUI MaxProducedText;
    public TextMeshProUGUI Produced0Text;
    public TextMeshProUGUI Produced1Text;
    public TextMeshProUGUI Produced2Text;

[Header("Goodies Panel")]
    public TextMeshProUGUI Goodie0Name;
    public Image Goodie0Icon;
    public TextMeshProUGUI Goodie1Name;
    public Image Goodie1Icon;
    public TextMeshProUGUI Goodie2Name;
    public Image Goodie2Icon;

[Header("Progress Bar")]
    public Image progressBar;

    public void OpenBuildingPanel() {

        uiManager.instance.ProdUI.SetActive(true);
        OpenedBuilding = uiManager.instance.selectedBuilding.GetComponent<Building>();

        LoadMainText();
        LoadGoodies();
        LoadProgressBar();
    }

    public void LoadMainText() {

        HealthText.text = OpenedBuilding.BuildingHealth.ToString();
        LevelText.text = OpenedBuilding.BuildingLevel.ToString();
        ProdRateText.text = OpenedBuilding.ProductionRate.ToString();
        
        MaxProducedText.text = OpenedBuilding.MaxProduced.ToString();
        Produced0Text.text = OpenedBuilding.ProducedAmount0.ToString();
        Produced1Text.text = OpenedBuilding.ProducedAmount1.ToString();
        Produced2Text.text = OpenedBuilding.ProducedAmount2.ToString();
    }

    void LoadGoodies() {

        for (int i = 0; i < uiManager.instance.RecipeIcons.Count; i++) {

            if(OpenedBuilding.ProducedGoods[0] != null) {
                
                Goodie0Name.text = OpenedBuilding.ProducedGoods[0].GoodieName;
                string[] name0 = Goodie0Name.text.Split(" ");
                
                for (int y = 0; y < name0.Length; y++) {
                    
                    if(Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(uiManager.instance.RecipeIcons[i])).Contains(name0[y])){
                        
                        Goodie0Icon.sprite = uiManager.instance.RecipeIcons[i];
                    }
                }
            }

            if(OpenedBuilding.ProducedGoods.ElementAtOrDefault(1) != null) {
                
                Goodie1Name.text = OpenedBuilding.ProducedGoods[1].GoodieName;
                string[] name1 = Goodie1Name.text.Split(" ");

                for (int y = 0; y < name1.Length; y++) {
                    
                    if(Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(uiManager.instance.RecipeIcons[i])).Contains(name1[y])){
                        
                        Goodie0Icon.sprite = uiManager.instance.RecipeIcons[i];
                    }
                }
            } else {
                Goodie1Name.text = "None";
                Goodie1Icon.sprite = uiManager.instance.CancelSprite;
            }

            if(OpenedBuilding.ProducedGoods.ElementAtOrDefault(2) != null) {
                
                Goodie2Name.text = OpenedBuilding.ProducedGoods[2].GoodieName;
                string[] name2 = Goodie2Name.text.Split(" ");

                for (int y = 0; y < name2.Length; y++) {
                    
                    if(Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(uiManager.instance.RecipeIcons[i])).Contains(name2[y])){
                        
                        Goodie0Icon.sprite = uiManager.instance.RecipeIcons[i];
                    }
                }
            } else {
                Goodie2Name.text = "None";
                Goodie2Icon.sprite = uiManager.instance.CancelSprite;
            }
        }
    }

    public void LoadProgressBar() {
        
        progressBar.fillAmount = OpenedBuilding.ProductionTimer / OpenedBuilding.ProductionTime;
    }

}
