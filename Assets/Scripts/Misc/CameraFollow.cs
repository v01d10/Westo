using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform cam;
    public Transform player;
    public float cameraHeight;
    public float cameraOffset;

    public bool canMove; 

    private void FixedUpdate () {
        if(canMove) {

            cam.transform.position = new Vector3 (player.position.x, cameraHeight, player.position.z + cameraOffset);
        }
    }
}

