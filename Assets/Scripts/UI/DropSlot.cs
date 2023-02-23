using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DropSlot : MonoBehaviour, IDropHandler {

    public GameObject UsedRecipe;
    public TextMeshProUGUI slotTimer;
    public bool Occupado;
    
    public void OnDrop(PointerEventData eventData) {
        Debug.Log("OnDrop");
        // if(eventData.pointerDrag != null) {
        //     eventData.pointerDrag.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
        // }
    }
}
