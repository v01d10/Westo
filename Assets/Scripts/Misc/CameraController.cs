using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public Vector3 defaultCamPosition;

    public Transform hero;

    public float MoveTime;

    CameraFollow camFollow;
    MobileCameraController mobileCamController;

    private void Awake() {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
        
        camFollow = GetComponent<CameraFollow>();
        mobileCamController = GetComponent<MobileCameraController>();
    }

    private void OnDestroy() { GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged; }

    private void GameManagerOnGameStateChanged(GameState state) {
        if(state == GameState.Day) {
            camFollow.canMove = false;
            Camera.main.transform.DOMove(defaultCamPosition, MoveTime);
            
            camFollow.enabled = false;
            mobileCamController.enabled = true;
        }
        if(state == GameState.Night){
            camFollow.cam.DOMove(new Vector3 (camFollow.player.position.x, camFollow.cameraHeight, camFollow.player.position.z + camFollow.cameraOffset), MoveTime).onComplete = () => {
                camFollow.canMove = true;
            };

            camFollow.enabled = true;
            mobileCamController.enabled = false;
        }
        
    }
}
