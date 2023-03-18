using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightManager : MonoBehaviour
{
    public static NightManager instance;

    public int NightIndex;

    private void Awake() {
        instance = this;
    }

    public void StartNextNight() {

        MakeSunset();
        EnemySpawner.instance.StartCoroutine("SpawnEnemies");
    }

    public void EndNight() {

    }

    public void MakeSunset() {

    }

}
