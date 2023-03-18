using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warehouse : MonoBehaviour
{
    public static Warehouse instance;
    
    public List<Goodie> StoredGoodies = new List<Goodie>();
    public List<Goodie> StoredGoodiesMine = new List<Goodie>();
    public List<Goodie> StoredGoodiesWoods = new List<Goodie>();
    public List<Goodie> StoredGoodiesFarm = new List<Goodie>();

    private void Awake() {
        instance = this;
        LoadStoredGoodies(); 
    }

    void LoadStoredGoodies() {
        for (int i = 0; i < StoredGoodiesWoods.Count; i++) {
            StoredGoodies.Add(StoredGoodiesWoods[i]);
        }
        for (int i = 0; i < StoredGoodiesMine.Count; i++) {
            StoredGoodies.Add(StoredGoodiesMine[i]);
        }
        for (int i = 0; i < StoredGoodiesFarm.Count; i++) {
            StoredGoodies.Add(StoredGoodiesFarm[i]);
        }
    }
}
