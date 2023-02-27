using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    public static bool IsMouseOverUI(){
        return EventSystem.current.IsPointerOverGameObject();
    }

    public static bool IsMouseOverUIIgnores(){
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        for (int i = 0; i < raycastResults.Count; i++) {
            if(raycastResults[i].gameObject.GetComponent<MouseClickThrough>() != null) {
                raycastResults.RemoveAt(i);
                i--;
            }
        }

        return raycastResults.Count > 0;
    }
}
