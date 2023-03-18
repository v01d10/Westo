using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class TownfolkNavOld : MonoBehaviour {
    
    public Townfolk thisFolk;

    public bool Loading;
    public LayerMask Layers;
    
    
    [Header("Navigation")]
    public float ProxRadius;
    NavMeshAgent Agent;
    RaycastHit Hit;
    Building b;

    [Header("Work Attributes")]
    public float MaxCarryAmount;
    public float CarryAmount0;
    public float CarryAmount1;
    public float CarryAmount2;

    public Building AssignedBuilding;
    public Warehouse warehouse;
    public List<Goodie> CarriedGoodies = new List<Goodie>();

    private void Start() {
        Agent = GetComponent<NavMeshAgent>();
        thisFolk = GetComponent<Townfolk>();
        GoToLoad();
    }

    public void GoToLoad(){
        Agent.SetDestination(AssignedBuilding.transform.position);
        Loading = true;
    }

    public void GoToUnload(){
        Agent.SetDestination(warehouse.transform.position);
        Loading = false;
    }

    public void LoadGoodies(){

        BuildingProduction AssociatedBuilding = AssignedBuilding.GetComponent<BuildingProduction>();
        
        for (int i = 0; i < AssociatedBuilding.ProducedGoods.Count; i++){
            if(!CarriedGoodies.Contains(AssociatedBuilding.ProducedGoods[i])) {

                CarriedGoodies.Add(AssociatedBuilding.ProducedGoods[i]);
            }
        }
        
        if(MaxCarryAmount > AssociatedBuilding.ProducedAmounts[0] && CarryAmount0 + CarryAmount1 + CarryAmount2 < MaxCarryAmount) {
            CarryAmount0 += AssociatedBuilding.ProducedAmounts[0];
            AssociatedBuilding.ProducedAmounts[0] = 0;

            if(MaxCarryAmount - CarryAmount0 > AssociatedBuilding.ProducedAmounts[1] && CarryAmount0 + CarryAmount1 + CarryAmount2 < MaxCarryAmount){
                CarryAmount1 += AssociatedBuilding.ProducedAmounts[1];
                AssociatedBuilding.ProducedAmounts[1] = 0;

                if(MaxCarryAmount - (CarryAmount0 + CarryAmount1) > AssociatedBuilding.ProducedAmounts[2] && CarryAmount0 + CarryAmount1 + CarryAmount2 < MaxCarryAmount){
                    CarryAmount2 += AssociatedBuilding.ProducedAmounts[2];
                    AssociatedBuilding.ProducedAmounts[2] = 0;

                } else {
                    CarryAmount2 += MaxCarryAmount - (CarryAmount1 + CarryAmount0);
                    AssociatedBuilding.ProducedAmounts[2] -= CarryAmount2;
                }

            } else {
                CarryAmount1 += MaxCarryAmount - CarryAmount0;
                AssociatedBuilding.ProducedAmounts[1] -= CarryAmount1;
            }

        } else {
            CarryAmount0 = MaxCarryAmount;
            AssociatedBuilding.ProducedAmounts[0] -= CarryAmount0;
        }

        GoToUnload();
    }

    public void UnloadGoodies(){
        foreach (var goodie in CarriedGoodies){
            for (int i = 0; i < CarriedGoodies.Count; i++){

                print(CarriedGoodies[i]);

                if(CarriedGoodies.ElementAtOrDefault(0) != null) {
                    if(goodie.GoodieName == CarriedGoodies[0].GoodieName) {
                        goodie.GoodieAmount += CarryAmount0;
                        CarryAmount0 = 0;
                    } 
                } else {
                    CarryAmount0 = 0;
                }

                if(CarriedGoodies.ElementAtOrDefault(1) != null) {
                    if(goodie.GoodieName == CarriedGoodies[1].GoodieName) {
                        goodie.GoodieAmount += CarryAmount1;
                        CarryAmount1 = 0;
                    } 
                } else {
                    CarryAmount1 = 0;
                }

                if(CarriedGoodies.ElementAtOrDefault(2) != null) {
                    if(goodie.GoodieName == CarriedGoodies[2].GoodieName) {
                        goodie.GoodieAmount += CarryAmount2;
                        CarryAmount2 = 0;
                    }  else {
                        CarryAmount2 = 0;
                    }
                }
            }
        }

        GoToLoad();
    }

    private void OnTriggerEnter(Collider other) {
        print(other.transform);

        if(other.GetComponentInParent<Building>() == AssignedBuilding && Loading) {
            LoadGoodies();
                
        }

        if(other.GetComponentInParent<Warehouse>() == warehouse && !Loading){
            UnloadGoodies();

        }        
    }

    IEnumerator ProxCheck(){
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, ProxRadius, Layers);

        for (int i = 0; i < hitColliders.Length; i++){

            if(Physics.Raycast(transform.position, hitColliders[i].transform.position, out Hit)) {
                print(Hit.transform.name);
                
            }
        }

            yield return new WaitForSeconds(0.7f);
            StartCoroutine(ProxCheck());
    }
}