using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownfolksUI : MonoBehaviour
{
    public static TownfolksUI instance;

    public GameObject FolkUI;
    public GameObject FolkUIBackground;
    public GameObject FolkPanelPrefab;

    private void Awake() {
        instance = this;
    }

    public void OpenFolkPanel() {

        FolkUI.SetActive(true);
        SpawnFolkPanels();
    }

    void SpawnFolkPanels() {

        for (int i = 0; i < TownfolkManager.instance.Townfolks.Count; i++) {
            
            GameObject folkPanel = Instantiate(FolkPanelPrefab, FolkUIBackground.transform);
            FolkPanel folkP = folkPanel.GetComponent<FolkPanel>();
            Townfolk folk = TownfolkManager.instance.Townfolks[i];
            
            folkP.FolkName.text = folk.Name;
            folkP.FolkHealth.text = ((int)folk.Health).ToString();
            folkP.FolkRole.text = folk.Role.ToString();

        }

        
    }
}
