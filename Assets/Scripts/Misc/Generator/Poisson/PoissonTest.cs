using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoissonTest : MonoBehaviour
{
    public float radius = 1;
    public Vector2 regionSize;
    public int rejectionSamples = 30;
    public float displayRadius = 1;
    public GameObject prefab;

    List<Vector2> points;

    void OnValidate() {
        points = PoissonDiscSampler.GeneratePoints(radius, regionSize, rejectionSamples);
    }

    private void Start() {
        OnValidate(); 
        
        if(points != null) {
            foreach (Vector2 point in points)
            {
                Vector3 spawnPoint = new Vector3(point.x, 0 , point.y); 
                Instantiate(prefab, spawnPoint, Quaternion.identity);
            }
        }  
    }
}
