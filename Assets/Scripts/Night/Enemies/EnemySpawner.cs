using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    
    public static EnemySpawner instance;
    
    [SerializeField] private Enemy[] Enemies;
    public Transform[] SpawnPoints;

    private double accumulatedWeights;
    private System.Random rand = new System.Random() ;

    public int EnemiesToSpawn;
    public int spawnIndex;

    private void Awake() {
        
        instance = this;
        CalculateWeight();
    }

    public IEnumerator SpawnEnemies() {

        if(spawnIndex < EnemiesToSpawn) {

            SpawnRandomEnemy(SpawnPoints[Random.Range(0, SpawnPoints.Length)].position);
        } else {
            spawnIndex = 0;
            StopAllCoroutines();
        }

        yield return new WaitForSeconds( 120 / EnemiesToSpawn);
        
        spawnIndex++;
        StartCoroutine("SpawnEnemies");
    }

    private void SpawnRandomEnemy(Vector3 position) {
        Enemy randomEnemy = Enemies[ GetRandomEnemyIndex() ];

        Instantiate (randomEnemy.Prefab, position, Quaternion.identity, transform);
        
        Debug.Log(randomEnemy.name);
    }

    private int GetRandomEnemyIndex() {

        double r = rand.NextDouble() * accumulatedWeights;

        for (int i = 0; i < Enemies.Length; i++) {
            if(Enemies[i]._weight >= r)
                return i;
        }

        return 0;
    }

    private void CalculateWeight() {

        accumulatedWeights = 0f;

        foreach (var enemy in Enemies) {
            
            accumulatedWeights += enemy.ChanceToSpawn;
            enemy._weight = accumulatedWeights;
        }
    }

    
}
