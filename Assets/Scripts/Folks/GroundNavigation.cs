using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundNavigation : MonoBehaviour
{
    private void OnMouseDown() {
        if(GameManager.instance.State == GameState.Day) {
            if(TownfolksUI.instance.Positioning && TownfolksUI.instance.selectedFolks.Any() && !uiManager.IsMouseOverUIIgnores()) {

                Vector3 mouse = Input.mousePosition;
                Ray castPoint = Camera.main.ScreenPointToRay(mouse);
                RaycastHit hit;

                if(Physics.Raycast(castPoint, out hit, Mathf.Infinity)){

                    for (int i = 0; i < TownfolksUI.instance.selectedFolks.Count; i++) {
                        TownfolksUI.instance.selectedFolks[i].navigation.GoTo(hit.point);
                    }
                }

            }
        }
    }
}
