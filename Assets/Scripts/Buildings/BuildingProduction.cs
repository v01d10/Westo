using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingProduction : Building
{
    public float ProductionRate;
    public float MaxProduced;

    public float ProductionTime;
    public float ProductionTimer;

    public List<float> ProducedAmounts = new List<float>(3);
    public List<Goodie> ProducedGoods = new List<Goodie>();

    private void Start() {
        Produce();
    }

    void Produce() {

        StopAllCoroutines();
        StartCoroutine("ProduceGoodies");
    }

    public IEnumerator ProduceGoodies(){
        
        if(WorkingFolks.Any() && WorkingFolks[0] != null){

            StartCoroutine("productionTimer");

            if(ProducedGoods.Count >= 1 && ProducedAmounts[0] < MaxProduced / ProducedGoods.Count) {
                
                ProducedAmounts[0] += ProductionRate / ProducedGoods.Count;
                if(ProducedGoods.Count >= 2 && ProducedAmounts[1] < MaxProduced / ProducedGoods.Count && BuildingLevel > 6) {

                    ProducedAmounts[1] += ProductionRate / ProducedGoods.Count;
                    if(ProducedGoods.Count >= 3 && ProducedAmounts[2] < MaxProduced / ProducedGoods.Count && BuildingLevel > 12) {

                        ProducedAmounts[2] += ProductionRate / ProducedGoods.Count;
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

    public void GatherGoodies() {

        uiManager.instance.ButtonClickEffect(uiManager.instance.productionUI.GatherButton, .8f, 0.1f);

        for (int i = 0; i < ProducedGoods.Count; i++) {
            
            for (int y = 0; y < Warehouse.instance.StoredGoodies.Count; y++) {
                
                if(ProducedGoods[i].GoodieName == Warehouse.instance.StoredGoodies[y].GoodieName) {

                    for (int a = 0; a < ProducedAmounts.Count; a++) {
                        
                        Warehouse.instance.StoredGoodies[y].GoodieAmount += ProducedAmounts[a];
                        ProducedAmounts[a] = 0;
                    }
                }
            }
        }

        uiManager.instance.productionUI.LoadMainText();
        print("Gathered Goodies");
    }

}
