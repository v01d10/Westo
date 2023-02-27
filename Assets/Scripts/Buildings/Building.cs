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

    public float ProducedAmount0;
    //public float MaxProducedAmount0;
    public float ProducedAmount1;
    //public float MaxProducedAmount1;
    public float ProducedAmount2;
    //public float MaxProducedAmount2;

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

    public IEnumerator ProduceGoodies(){
        
        if(WorkingFolks.Any()){

            if(ProducedGoods.Count >= 1 && ProducedAmount0 < MaxProduced / ProducedGoods.Count) {
                
                ProducedAmount0 += ProductionRate / ProducedGoods.Count;
                if(ProducedGoods.Count >= 2 && ProducedAmount1 < MaxProduced / ProducedGoods.Count && BuildingLevel > 6) {

                    ProducedAmount1 += ProductionRate / ProducedGoods.Count;
                    if(ProducedGoods.Count >= 3 && ProducedAmount2 < MaxProduced / ProducedGoods.Count && BuildingLevel > 12) {

                        ProducedAmount2 += ProductionRate / ProducedGoods.Count;
                    }
                } 
            } 
        }

        Debug.Log("Producing goodies");
        yield return new WaitForSeconds(10);
        StartCoroutine(ProduceGoodies());
    }

    private void OnMouseDown() {
        
    }

    public void GatherGoodies() {

        
    }

}
