using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TownfolkRole{
    None, Lumber, Miner, Crafter, Fighter,
}

public class Townfolk : FolkBase {

    public TownfolkRole Role;
    public Transform ModelHolder;
    public Sprite Icon;

    public Building AssignedBuilding;
    public TownfolkGroup AssignedGroup;

    public FolkNav navigation;
    public GameObject selectedFolkSlot;

    public bool FolkSet;

    private void Start() {
        
        navigation = GetComponent<FolkNav>();
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
            MaxHealth = 101; Health = MaxHealth; Level = 1; ExpNeeded = 123;
            Strength = Random.Range(1, 4);
            Inteligence = Random.Range(1, 4);
            Dexterity = Random.Range(1, 4);

            TownfolkManager.instance.TownfolkGroups[0].FolksInThisGroup.Add(this);

            FolkSet = true;
        }
    }

    public void AssignBuilding() {

        uiManager.instance.selectedBuilding.GetComponent<Building>().WorkingFolks.Add(this);
        AssignedBuilding = uiManager.instance.selectedBuilding.GetComponent<Building>();
        Role = uiManager.instance.selectedBuilding.GetComponent<Building>().BuildingRole;
    }

    private void OnMouseDown() {
        
        if(ModelHolder.gameObject.activeInHierarchy && GameManager.instance.State == GameState.Day) {

            uiManager.instance.CloseUI();
            TownfolkManager.instance.SelectedFolk = this;
            TownfolksUI.instance.OpenFolkDetail();
        }
    }


}

