using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TownfolksUI : MonoBehaviour
{
    public static TownfolksUI instance;

    public GameObject FolkUI;
    public GameObject FolkUIBackground;
    public GameObject FolkPanelPrefab;
    public List<GameObject> FolkPanels;

[Header("Workers")]
    public GameObject WorkerPanel;
    public GameObject WorkerSlotPrefab;
    public List<GameObject> WorkerSlots;
    public GameObject SelectedSlot;

[Header("Folk Details")]
    public FolkDetailPanel FolkDetailUI;

    private void Awake() {
        instance = this;
    }

    public void OpenFolkPanel(bool building) {

        FolkUI.SetActive(true);
        SpawnFolkPanels();

        if(building != true) {
            
            WorkerPanel.SetActive(false);
        } else {

            WorkerPanel.SetActive(true);
            SpawnWorkerSlots();
        }

        uiManager.instance.CloseBuildingMenu();
    }

    public void CloseFolkPanel() {

        FolkUI.SetActive(false);

        for (int i = 0; i < WorkerSlots.Count; i++) {
            
            Destroy(WorkerSlots[i]);
            WorkerSlots.RemoveAt(i);
        }
    }

    public void OpenFolkDetail() {
        
        uiManager.instance.CloseUI();
        FolkDetailUI.gameObject.SetActive(true);
        FolkDetailUI.LoadDetailText();
    }

    public void CloseFolkDetail() {

        FolkDetailUI.gameObject.SetActive(false);
        TownfolkManager.instance.SelectedFolk = null;
    }

    void SpawnFolkPanels() {

        for (int i = 0; i < TownfolkManager.instance.Townfolks.Count; i++) {
            
            GameObject folkPanel = Instantiate(FolkPanelPrefab, FolkUIBackground.transform);
            FolkPanel folkP = folkPanel.GetComponent<FolkPanel>();
            Townfolk folk = TownfolkManager.instance.Townfolks[i];
            
            LoadFolkInfo(folkP, folk);

            FolkPanels.Add(folkPanel);
        }
    }

    void SpawnWorkerSlots() {

        for (int i = 0; i < uiManager.instance.selectedBuilding.GetComponent<Building>().WorkersCapacity; i++) {
                
            GameObject workerSlot = Instantiate(WorkerSlotPrefab, WorkerPanel.transform);
                
            if(uiManager.instance.selectedBuilding.GetComponent<Building>().WorkingFolks.Any() && uiManager.instance.selectedBuilding.GetComponent<Building>().WorkingFolks[i] != null){
                    
                workerSlot.GetComponent<WorkerSlot>().WorkerIcon.sprite = uiManager.instance.selectedBuilding.GetComponent<Building>().WorkingFolks[i].Icon;
                WorkerSlots.Add(workerSlot);
            } else {

                workerSlot.GetComponent<WorkerSlot>().WorkerIcon.sprite = uiManager.instance.PlusSprite;
                workerSlot.GetComponent<Button>().onClick.AddListener(() => SelectWorkerSlot(workerSlot));
                WorkerSlots.Add(workerSlot);
            }
        }
    }

    void LoadFolkInfo(FolkPanel folkP, Townfolk folk) {

        folkP.UsedFolk = folk;
        folkP.FolkName.text = folk.Name;
        folkP.FolkHealth.text = ((int)folk.Health).ToString();
        folkP.FolkRole.text = folk.Role.ToString();
    }

    void SelectWorkerSlot(GameObject workerSlot) {

        SelectedSlot = workerSlot;
        workerSlot.transform.DOScale(new Vector3(.9f, .9f, .9f), .3f);

        for (int i = 0; i < FolkPanels.Count; i++) {
            
            AssignWorker(i, workerSlot);
        }
    }

    void AssignWorker(int index, GameObject workerSlot) {
    
        FolkPanels[index].GetComponent<Button>().onClick.RemoveAllListeners();
        FolkPanels[index].GetComponent<Button>().onClick.AddListener(() => {

            workerSlot.transform.DOScale(new Vector3(1f, 1f, 1f), .3f);

            if(uiManager.instance.selectedBuilding.GetComponent<Building>().WorkingFolks.Any() && uiManager.instance.selectedBuilding.GetComponent<Building>().WorkingFolks[WorkerSlots.IndexOf(workerSlot)] != null) {

                uiManager.instance.selectedBuilding.GetComponent<Building>().WorkingFolks[WorkerSlots.IndexOf(workerSlot)].Role = TownfolkRole.None;
                uiManager.instance.selectedBuilding.GetComponent<Building>().WorkingFolks.RemoveAt(WorkerSlots.IndexOf(workerSlot));
                
                for (int i = 0; i < FolkPanels.Count; i++) {
                    
                    LoadFolkInfo(FolkPanels[i].GetComponent<FolkPanel>(), FolkPanels[i].GetComponent<FolkPanel>().UsedFolk);
                }
            }
            
            FolkPanels[index].GetComponent<FolkPanel>().UsedFolk.AssignBuilding();
            LoadFolkInfo(FolkPanels[index].GetComponent<FolkPanel>(), FolkPanels[index].GetComponent<FolkPanel>().UsedFolk);
            workerSlot.GetComponent<WorkerSlot>().WorkerIcon.sprite = uiManager.instance.selectedBuilding.GetComponent<Building>().WorkingFolks[WorkerSlots.IndexOf(workerSlot)].Icon;
        });
    }

}
