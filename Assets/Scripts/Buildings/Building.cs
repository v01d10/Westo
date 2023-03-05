using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Building : MonoBehaviour
{
    public float BuildingHealth;

    public float BuildingLevel;
    public float UpgradePrice;

    public float ProductionRate;
    public float MaxProduced;

    public float ProductionTime;
    public float ProductionTimer;

    public float ProducedAmount0;
    public float ProducedAmount1;
    public float ProducedAmount2;

    public List<Goodie> ProducedGoods = new List<Goodie>();
    public List<Townfolk> WorkingFolks = new List<Townfolk>();

    public void UpgradeBuilding(Building thisBuilding){
        if(Warehouse.instance.Money >= thisBuilding.UpgradePrice){
            thisBuilding.BuildingLevel++;
            Warehouse.instance.Money -= thisBuilding.UpgradePrice;
            thisBuilding.UpgradePrice *= 1.7f;
            thisBuilding.ProductionRate *= 1.5f;
        } else {
            print("Not enough money!");
        }
    }

    void Produce() {

        StopAllCoroutines();
        StartCoroutine("ProduceGoodies");
    }

    public IEnumerator ProduceGoodies(){
        
        if(WorkingFolks.Any()){

            StartCoroutine("productionTimer");

            if(ProducedGoods.Count >= 1 && ProducedAmount0 < MaxProduced / ProducedGoods.Count) {
                
                ProducedAmount0 += ProductionRate / ProducedGoods.Count;
                if(ProducedGoods.Count >= 2 && ProducedAmount1 < MaxProduced / ProducedGoods.Count && BuildingLevel > 6) {

                    ProducedAmount1 += ProductionRate / ProducedGoods.Count;
                    if(ProducedGoods.Count >= 3 && ProducedAmount2 < MaxProduced / ProducedGoods.Count && BuildingLevel > 12) {

                        ProducedAmount2 += ProductionRate / ProducedGoods.Count;
                    }
                } 
            } 

            for (int i = 0; i < WorkingFolks.Count; i++) {
                WorkingFolks[i].AddExp(0.03f);
            }
            
            Debug.Log("Producing goodies");
        } else {

            Debug.LogError("No workers " + name);
        }

        yield return new WaitForSeconds(ProductionTime);
        Produce();
    }

    IEnumerator productionTimer() {

        if(uiManager.instance.ProdUI.activeInHierarchy && uiManager.instance.selectedBuilding == this.gameObject){

            uiManager.instance.productionUI.LoadProgressBar();
            uiManager.instance.productionUI.LoadMainText();
        }

        if(ProductionTimer < ProductionTime) {

            ProductionTimer++;
            yield return new WaitForSeconds(1);
            StartCoroutine("productionTimer");

        } else {

            ProductionTimer = 0;
            StartCoroutine("productionTimer");
        }
    }

    private void OnMouseDown() {
        
        if(!uiManager.IsMouseOverUIIgnores()){

            uiManager.instance.selectedBuilding = this.gameObject;
            uiManager.instance.OpenBuildingMenu();
        }
    }

    public void GatherGoodies() {

        uiManager.instance.ButtonClickEffect(uiManager.instance.productionUI.GatherButton);

        for (int i = 0; i < ProducedGoods.Count; i++) {
            
            for (int y = 0; y < Warehouse.instance.StoredGoodies.Count; y++) {
                
                if(ProducedGoods[i].GoodieName == Warehouse.instance.StoredGoodies[y].GoodieName) {

                    if(i == 0) {

                        Warehouse.instance.StoredGoodies[y].GoodieAmount += ProducedAmount0;
                        ProducedAmount0 = 0;
                    }
                    if(i == 1) {

                        Warehouse.instance.StoredGoodies[y].GoodieAmount += ProducedAmount1;
                        ProducedAmount1 = 0;
                    }
                    if(i == 2) {

                        Warehouse.instance.StoredGoodies[y].GoodieAmount += ProducedAmount2;
                        ProducedAmount2 = 0;
                    }
                    
                }
            }
        }

        uiManager.instance.productionUI.LoadMainText();
        print("Gathered Goodies");
    }

}
