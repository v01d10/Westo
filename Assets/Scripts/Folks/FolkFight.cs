using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolkFight : MonoBehaviour {

    public Townfolk thisFolk;

    public bool Melee;

    public Enemy Target;

    private void OnTriggerEnter(Collider other) {
        
        if(other.tag == "Enemy") {

            Target = other.GetComponent<Enemy>();
            Attack();
        }
    }

    public void Attack() {

        
    }
    
    IEnumerator Hit() {

        yield return null;
    }
}
