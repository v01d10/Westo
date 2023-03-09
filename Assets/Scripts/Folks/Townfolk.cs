using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TownfolkRole{
    None, Lumber, Miner, Crafter, Fighter,
}

public class Townfolk : MonoBehaviour
{
    public string Name;

    public TownfolkRole Role;
    public Transform ModelHolder;
    public Sprite Icon;

[Header("Basic Attributes")]
    bool Male;
    public float Health;
    public float Level;
    public float Exp;
    public float ExpNeeded;

    public float AtPoints;
    public float Strength;
    public float Inteligence;
    public float Dexterity;

    public bool FolkSet;

    private void Start() {
        
        SetThisFolk();
    }

    void SetFolkName() {
        
        TownfolkManager folkManager = TownfolkManager.instance;

        if(Male) {

            Instantiate(folkManager.FolkMaleModels[Random.Range(0, folkManager.FolkMaleModels.Count)], ModelHolder);
            Name = folkManager.FolkMaleNames[Random.Range(0, folkManager.FolkMaleNames.Count)];
        } else {

            Instantiate(folkManager.FolkFemaleModels[Random.Range(0, folkManager.FolkFemaleModels.Count)], ModelHolder);
            Name = folkManager.FolkFemaleNames[Random.Range(0, folkManager.FolkFemaleNames.Count)];

        } 
        string surname = folkManager.FolkSurnames[Random.Range(0, folkManager.FolkSurnames.Count)];
        Name += (" " + surname); 
        folkManager.FolkSurnames.Remove(surname);
    }

    void SetThisFolk() {

        if(!FolkSet) {
            
            if(Random.Range(0, 10) > 4) Male = true;

            SetFolkName();
            Health = 101; Level = 1; ExpNeeded = 123;
            Strength = Random.Range(1, 4);
            Inteligence = Random.Range(1, 4);
            Dexterity = Random.Range(1, 4);

            FolkSet = true;
        }
    }

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

    public void AssignBuilding() {

        uiManager.instance.selectedBuilding.GetComponent<Building>().WorkingFolks.Add(this);
        Role = uiManager.instance.selectedBuilding.GetComponent<Building>().BuildingRole;
    }

    private void OnMouseDown() {
        
        uiManager.instance.CloseUI();
        TownfolkManager.instance.SelectedFolk = this;
        TownfolksUI.instance.OpenFolkDetail();
    }


}

