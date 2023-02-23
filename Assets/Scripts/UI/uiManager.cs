using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiManager : MonoBehaviour
{
    public static uiManager instance;

    public ProcessingUI processingUI;

    public GameObject selectedBuilding;

    public GameObject ProcessUI;


    private void Awake() {
        instance = this;
        
        processingUI = GetComponent<ProcessingUI>();
    }
}
