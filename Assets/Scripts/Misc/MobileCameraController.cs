using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileCameraController : MonoBehaviour
{
    public Camera Camera;
    protected Plane Plane;

    private void Awake() {
        if(Camera == null) {
            Camera = Camera.main;
        }
    }

    private void Update() {
        if(!uiManager.IsMouseOverUIIgnores()) {

            MoveCamera();
        }
    }

    void MoveCamera() {
        if(Input.touchCount >= 1) {

            Plane.SetNormalAndPosition(transform.up, transform.position);
        }

        var Delta1 = Vector3.zero;
        var Delta2 = Vector3.zero;

        if(Input.touchCount >= 1) {

            Delta1 = PlanePositionDelta(Input.GetTouch(0));
            if(Input.GetTouch(0).phase == TouchPhase.Moved) {
                
                Camera.transform.Translate(Delta1, Space.World);

                if(uiManager.instance.ProcessUI.activeInHierarchy){
                    uiManager.instance.processingUI.CloseProcessingMenu();
                }
            }
        }
    }

    void ZoomCamera() {
        if(Input.touchCount >= 2) {

            var pos0 = PlanePosition(Input.GetTouch(0).position);
            var pos1 = PlanePosition(Input.GetTouch(1).position);
            var pos0b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(0).deltaPosition);
            var pos1b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

            var zoom = Vector3.Distance(pos0, pos1) / Vector3.Distance(pos0b, pos1b);

            if(zoom == 0 || zoom > 10) {
                return;
            }

            Camera.transform.position = Vector3.Lerp(pos1, Camera.transform.position, 1 / zoom);
        }
    }

    protected Vector3 PlanePositionDelta(Touch touch) {
        if(touch.phase != TouchPhase.Moved) {
            return Vector3.zero;
        }

        var rayBefore = Camera.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = Camera.ScreenPointToRay(touch.position);
        if(Plane.Raycast(rayBefore, out var enterBefore) && Plane.Raycast(rayNow, out var EnterNow)){
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(EnterNow);
        }

        return Vector3.zero;
    }

    protected Vector3 PlanePosition(Vector2 screenPos) {
        var rayNow = Camera.ScreenPointToRay(screenPos);
        if(Plane.Raycast(rayNow, out var enterNow)) {
            return rayNow.GetPoint(enterNow);
        }

        return Vector3.zero;
    }
}
