using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FolkNav : MonoBehaviour {

    NavMeshAgent agent;
    public SphereCollider sphereCollider;

    Townfolk folk;

    public bool Unloading;

    private void Awake() {
        agent  = GetComponent<NavMeshAgent>();
        folk = GetComponent<Townfolk>();
        sphereCollider = transform.GetComponent<SphereCollider>();
    }

    public void GoTo(Vector3 target) {
        agent.SetDestination(target);
    }

    private void OnTriggerEnter(Collider other) {
        
        if(other.transform.GetComponent<Building>()){

            if(other.transform.GetComponent<Building>().WorkingFolks.Contains(folk) && !Unloading ) {
                ActivateModel();
                sphereCollider.enabled = false;
            }
        }

        if(other.transform.GetComponent<Warehouse>() == Player.instance.PlayerWarehouse && Unloading) {
            GoTo(folk.AssignedBuilding.transform.position);
            Unloading = false;
        }
    }

    public void ActivateModel() {
        if(folk.ModelHolder.gameObject.activeInHierarchy) {
            folk.ModelHolder.gameObject.SetActive(false);
        } else {
            folk.ModelHolder.gameObject.SetActive(true);
        }

        print("ActivateModel");
    }


}
