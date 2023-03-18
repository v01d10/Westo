using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Building : MonoBehaviour
{
    public TownfolkRole BuildingRole;
    public float BuildingHealth;

    public float BuildingLevel;
    public float UpgradePrice;

    public int WorkersCapacity;

    public List<Townfolk> WorkingFolks = new List<Townfolk>();


    public void UpgradeBuilding(GameObject building) {

        if (Player.instance.Money >= UpgradePrice) {
            BuildingLevel++;
            Player.instance.Money -= UpgradePrice;
            UpgradePrice *= 1.7f;
            
            if(BuildingLevel == 6 || BuildingLevel == 12) {
                WorkersCapacity++;
                WorkingFolks.Add(null);
            }

            if(building.GetComponent<BuildingProduction>()) {

                building.GetComponent<BuildingProduction>().ProductionRate *= 1.5f;
                
            } else if(building.GetComponent<BuildingProcessed>()) {

            }

        } else {
            print("Not enough money!");
        }
    }

    private void OnMouseDown(){

        if (!uiManager.IsMouseOverUIIgnores()){

            uiManager.instance.selectedBuilding = this.gameObject;
            uiManager.instance.OpenBuildingMenu();
        }
    }


}