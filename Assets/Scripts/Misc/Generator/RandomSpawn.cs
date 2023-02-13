using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour {
    
    public GameObject objToSpawn0;
    public float SpawnAmount0;

    public GameObject objToSpawn1;
    public float SpawnAmount1;

    public GameObject objToSpawn2;
    public float SpawnAmount2;


    public float xSpread;
    public float ySpread;
    public float zSpread;


    void Start(){
        GenerateWorld();
    }

    void GenerateWorld() {
        for (int i = 0; i < SpawnAmount0; i++) {
            Spawn(objToSpawn0);
        }

        for (int i = 0; i < SpawnAmount1; i++) {
            Spawn(objToSpawn1);
        }
        
        for (int i = 0; i < SpawnAmount2; i++) {
            Spawn(objToSpawn2);
        }
    }

    public void Spawn(GameObject obj){
        Vector3 randPosition = new Vector3(Random.Range(-xSpread, xSpread), Random.Range(-ySpread, ySpread), Random.Range(-zSpread, zSpread)) + transform.position;
        GameObject clone = Instantiate(obj, randPosition, Quaternion.identity);

    }
}