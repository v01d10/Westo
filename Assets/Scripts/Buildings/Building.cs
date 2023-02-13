using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public float BuildingHealth;

    public float BuildingLevel;
    public float UpgradePrice;

    public float ProductionRate;

    public float ProducedAmount0;
    public float MaxProducedAmount0;
    public float ProducedAmount1;
    public float MaxProducedAmount1;
    public float ProducedAmount2;
    public float MaxProducedAmount2;

    public List<Townfolk> WorkingFolks = new List<Townfolk>();
    public List<Goodie> ProducedGoods = new List<Goodie>();

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
        Debug.Log(ProducedGoods.Count);

        if(ProducedGoods.Count >= 1 && ProducedAmount0 < MaxProducedAmount0) {
            
            ProducedAmount0 += ProductionRate / ProducedGoods.Count;
            if(ProducedGoods.Count >= 2 && ProducedAmount1 < MaxProducedAmount1 && BuildingLevel > 6) {

                ProducedAmount1 += ProductionRate / ProducedGoods.Count;
                if(ProducedGoods.Count >= 3 && ProducedAmount2 < MaxProducedAmount2 && BuildingLevel > 12) {

                    ProducedAmount2 += ProductionRate / ProducedGoods.Count;
                }
            } 
        } else {
            yield return null;
        }

        Debug.Log("Producing goodies");
        yield return new WaitForSeconds(3);
        StartCoroutine(ProduceGoodies());
    }

}
