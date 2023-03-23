using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolkBase : MonoBehaviour {
[Header("Basic Stats")]
    public bool Hero;
    public bool Male;

    public string Name;

    public float MaxHealth;
    public float Health;
    public float Level;
    public float Exp;
    public float ExpNeeded;

[Header("HP Bar")]

    public GameObject hpBarHolder;
    public Transform hpBar;

[Header("Attributes")]
    public float AtPoints;
    public float Strength;
    public float Inteligence;
    public float Dexterity;

[Header("Damage")]
    public float Damage;
    public float CritChance;
    public float CritMultiplier;

    public void AddExp(float amount) {

        if(Exp + amount < ExpNeeded) {
           
            Exp += amount;
        } else {
            
            AtPoints++;
            Level++;
            Exp = (Exp + amount) - ExpNeeded;
            ExpNeeded *= 1.7f;
            Health *= 1.5f;
        }
    }

    public void IncreaseStat(int index) {
        
        if(AtPoints > 0) {

            AtPoints--;

            if(index == 0) {
                Strength++;
            } else if(index == 1) {
                Dexterity++;
            } else if (index == 2) {
                Inteligence++;
            }
        }
    }

    public void SubtractHealth(float amount) {

        if(Health - amount > 0) {
            Health -= amount;
        } else {
            Health = 0;
            Death();
        }
    
        if(!hpBarHolder.activeInHierarchy)
            hpBarHolder.SetActive(true);
        hpBar.localScale = new Vector3((Health / MaxHealth) , hpBar.localScale.y, hpBar.localScale.z);
    }

    void Death() {
        if(Hero) {
            print("Mission failed");
        } else {
            Destroy(gameObject);
        }
    }
}
