using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodWorker : Building
{
    void Start() {
        StartCoroutine(ProduceGoodies());
    }

}
