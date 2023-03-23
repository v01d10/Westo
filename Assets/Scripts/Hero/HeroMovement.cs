using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovement : MonoBehaviour {

    public FixedJoystick moveStick;
    public FixedJoystick aimStick;

    Vector3 moveVelocity;
    Vector3 aimVelocity;

    Vector3 moveInput;
    Vector3 moveDir;

    Vector3 aimInput;
    Vector3 lookAtPoint;

    public float moveSpeed;
    public float fireSpeed;

    public float reloadTime;
    public float damage;

    LineRenderer aimLaser;
    public Transform laserOrigin;
    public float laserRange;

    bool shooting;

    Rigidbody rb;
    RaycastHit hit;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        aimLaser = GetComponentInChildren<LineRenderer>();
    }

    private void FixedUpdate() {

        if(GameManager.instance.State == GameState.Night) {
            
            moveVelocity = new Vector3(moveStick.Horizontal, 0f, moveStick.Vertical);

            moveInput = new Vector3 (moveVelocity.x, 0f, moveVelocity.z);
            moveDir = moveInput.normalized * moveSpeed;
            rb.MovePosition (rb.position + moveDir * Time.deltaTime);

            aimVelocity = new Vector3(aimStick.Horizontal, 0f, aimStick.Vertical);
            aimInput = new Vector3(aimVelocity.x, 0f, aimVelocity.z);
            lookAtPoint = transform.position + aimInput;
            transform.LookAt(lookAtPoint);
            
            aimLaser.SetPosition(0, laserOrigin.position);
            if(Physics.Raycast(laserOrigin.position, laserOrigin.forward, out hit, laserRange)) {
                
                aimLaser.SetPosition(1, hit.point);

                if(hit.transform.GetComponent<Enemy>() ) {
                    if(!shooting){
                        StartCoroutine("Shoot");
                    }
                }
            } else {

                aimLaser.SetPosition(1, laserOrigin.position + (laserOrigin.forward * laserRange));
            }
        }
    }

    IEnumerator Shoot() {

        if(hit.transform != null) {
            
            PlayGunSound();
            shooting = true;
            hit.transform.GetComponent<Enemy>().SubtractHealth(damage);

            yield return new WaitForSeconds(reloadTime);

            shooting = false;
            yield return null;
        } else {
            yield return null;
        }
    }

    void PlayGunSound() {
        print("playing sound");
    }

}
