using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastAligner : MonoBehaviour
{

    public GameObject[] itemsToPickFrom;

    public float raycastDistance;
    public float overlapTestBoxSize;

    public LayerMask spawnedObjectLayer;
    Collider[] collidersInsideOverlapBox = new Collider[1];
        
    private void Start() {
        PositionRaycast();
    }

    void PositionRaycast(){
        RaycastHit hit;

        if(Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance)) {
            
            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            Vector3 overlapTestBoxScale = new Vector3(overlapTestBoxSize, overlapTestBoxSize, overlapTestBoxSize);
            int numberOfCollidersFound = Physics.OverlapBoxNonAlloc(hit.point, overlapTestBoxScale, collidersInsideOverlapBox, spawnRotation, spawnedObjectLayer);

            Debug.Log("Number of colliders found " + numberOfCollidersFound);

            if(numberOfCollidersFound == 0) {
                Debug.Log("Spawning");
                pick(hit.point, spawnRotation);                
                
            } else {
                Debug.Log("Name of collider 0 found " + collidersInsideOverlapBox[0].name);
            }
        }
        
        Destroy(this.gameObject);
    }

    void pick(Vector3 positionToSpawn, Quaternion rotationToSpawn){
 
        int randomIndex = Random.Range(0, itemsToPickFrom.Length);
        GameObject clone = Instantiate(itemsToPickFrom[randomIndex], positionToSpawn, rotationToSpawn);
    }
}
