using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Transform lookAt;
    private Transform localTarnsform;

    void Start() {
        localTarnsform = GetComponent<Transform>();
    }

    void Update() {
        if(lookAt){
            localTarnsform.LookAt( 2 * localTarnsform.position - lookAt.position);
        }        
    }
}
