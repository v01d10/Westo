using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightManager : MonoBehaviour
{
    public static NightManager instance;

    public GameObject Joysticks;

    public int NightIndex;

    private void Awake() {
        instance = this;
    }

    public void StartNextNight() {

        MakeSunset();
        EnemySpawner.instance.StartCoroutine("SpawnEnemies");
        Joysticks.SetActive(true); 
    }

    public void EndNight() {
        
        Joysticks.SetActive(false); 
    }

    public void MakeSunset() {

    }

}
