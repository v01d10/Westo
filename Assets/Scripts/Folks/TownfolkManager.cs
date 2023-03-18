using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownfolkManager : MonoBehaviour
{
    public static TownfolkManager instance;

    public Townfolk SelectedFolk;

[Header("Names")]
    public List<string> FolkMaleNames;
    public List<string> FolkFemaleNames;
    public List<string> FolkSurnames;
    public List<string> UsedSurnames;

[Header("Models")]
    public List<GameObject> FolkMaleModels;
    public List<GameObject> FolkFemaleModels;

[Header("Folks")]
    public GameObject FolkPrefab;
    public List<Townfolk> Townfolks = new List<Townfolk>();

[Header("Groups")]
    public int GroupIndex;
    public List<TownfolkGroup> TownfolkGroups = new List<TownfolkGroup>();


    private void Awake() {
        instance = this;
        AddFolk(5);
    }

    public void AddFolk(int amount) {

        for (int i = 0; i < amount; i++) {
            
            GameObject Folk = Instantiate(FolkPrefab, gameObject.transform);
            Townfolks.Add(Folk.GetComponent<Townfolk>());
        }
    }

    public void AddFolkIntoGroup(int groupIndex, Townfolk folk) {
        TownfolkGroups[groupIndex].FolksInThisGroup.Add(folk);
    }
}
