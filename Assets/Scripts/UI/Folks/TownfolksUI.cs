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
    public List<GameObject> FolkPanels = new List<GameObject>();

[Header("Workers")]
    public GameObject WorkerPanel;
    public GameObject WorkerSlotPrefab;
    public List<GameObject> WorkerSlots = new List<GameObject>();
    public GameObject SelectedSlot;

    Building selectedBuilding;

[Header("Folk Details")]
    public FolkDetailPanel FolkDetailUI;

[Header("Grouping")]
    public GameObject GroupBackgroundPrefab;
    public Transform FolkScrollBackground;

    public Button CreateGroupButton;
    public Button GroupingButton;
    public bool Grouping;

[Header("Positioning")]
    public Button PositioningButton;
    public bool Positioning;
    public List<Townfolk> selectedFolks = new List<Townfolk>();
    public List<GameObject> selectedFolksPanels = new List<GameObject>();

    private void Awake() {
        instance = this;
        AssignCreateGroupButton();
    }

    public void OpenFolkPanel(bool building) {

        uiManager.instance.CloseBuildingMenu();

        FolkUI.SetActive(true);
        SpawnFolkPanels();

        selectedBuilding = uiManager.instance.selectedBuilding.GetComponent<Building>();
        
        if(building != true) {
            
            WorkerPanel.SetActive(false);
            GroupingButton.gameObject.SetActive(true);
            PositioningButton.gameObject.SetActive(true);

            GroupingButton.onClick.RemoveAllListeners();
            PositioningButton.onClick.RemoveAllListeners();

            GroupingButton.onClick.AddListener(() => GroupingON() );
            PositioningButton.onClick.AddListener(() => PositioningON() );


        } else {

            WorkerPanel.SetActive(true);
            GroupingButton.gameObject.SetActive(false);
            PositioningButton.gameObject.SetActive(false);
            SpawnWorkerSlots();
        }

    }

    public void CloseFolkPanel() {

        foreach (var panel in FolkPanels) {
            Destroy(panel);
        }
        foreach (var selectedPanel in selectedFolksPanels) {
            Destroy(selectedPanel);
        }
        foreach (var slot in WorkerSlots ) {
            Destroy(slot);
        }

        FolkPanels.Clear();
        selectedFolksPanels.Clear();
        WorkerSlots.Clear();

        FolkUI.SetActive(false);
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

        for (int g = 0; g < TownfolkManager.instance.TownfolkGroups.Count; g++) {
            for (int i = 0; i < TownfolkManager.instance.TownfolkGroups[g].FolksInThisGroup.Count; i++) {

                GameObject folkPanel = Instantiate(FolkPanelPrefab, TownfolkManager.instance.TownfolkGroups[g].AssignedTab);            
                LoadFolkInfo(folkPanel.GetComponent<FolkPanel>(), TownfolkManager.instance.TownfolkGroups[g].FolksInThisGroup[i]);
                FolkPanels.Add(folkPanel);
            }
        }
    }

    void SpawnWorkerSlots() {

        for (int i = 0; i < selectedBuilding.WorkersCapacity; i++) {
                
            GameObject workerSlot = Instantiate(WorkerSlotPrefab, WorkerPanel.transform);
                
            if(selectedBuilding.WorkingFolks.Any() && selectedBuilding.WorkingFolks[i] != null){
                    
                workerSlot.GetComponent<WorkerSlot>().WorkerIcon.sprite = selectedBuilding.WorkingFolks[i].Icon;
                WorkerSlots.Add(workerSlot);
            } else {

                workerSlot.GetComponent<WorkerSlot>().WorkerIcon.sprite = uiManager.instance.PlusSprite;
                WorkerSlots.Add(workerSlot);
            }
            
            workerSlot.GetComponent<Button>().onClick.AddListener(() => SelectWorkerSlot(workerSlot));
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
        workerSlot.transform.DOScale(new Vector3(.9f, .9f, .9f), .2f);

        for (int i = 0; i < FolkPanels.Count; i++) {
            
            AssignWorkerButton(i, workerSlot);
        }
    }

    void AssignWorkerButton(int index, GameObject workerSlot) {
    
        FolkPanels[index].GetComponent<Button>().onClick.AddListener(() => {

            AssignWorker(index, workerSlot);
        });
    }

    void AssignWorker(int index, GameObject workerSlot) {

        FolkPanel folkPanel = FolkPanels[index].GetComponent<FolkPanel>();

        if(folkPanel.UsedFolk.AssignedBuilding == null) {

            if(selectedBuilding.WorkingFolks.Any() && selectedBuilding.WorkingFolks[WorkerSlots.IndexOf(workerSlot)] != null) {

                selectedBuilding.WorkingFolks[WorkerSlots.IndexOf(workerSlot)].Role = TownfolkRole.None;
                selectedBuilding.WorkingFolks[WorkerSlots.IndexOf(workerSlot)].AssignedBuilding = null;
                
                selectedBuilding.WorkingFolks[WorkerSlots.IndexOf(workerSlot)].navigation.ActivateModel();
                selectedBuilding.WorkingFolks[WorkerSlots.IndexOf(workerSlot)].navigation.GoTo(Player.instance.PlayerWarehouse.transform.position);
                
                selectedBuilding.WorkingFolks.RemoveAt(WorkerSlots.IndexOf(workerSlot));
            }

            for (int i = 0; i < FolkPanels.Count; i++) {
                        
                LoadFolkInfo(FolkPanels[i].GetComponent<FolkPanel>(), FolkPanels[i].GetComponent<FolkPanel>().UsedFolk);
                FolkPanels[i].GetComponent<Button>().onClick.RemoveAllListeners();
            }
            
            folkPanel.UsedFolk.AssignBuilding();
            LoadFolkInfo(folkPanel, folkPanel.UsedFolk);
            workerSlot.GetComponent<WorkerSlot>().WorkerIcon.sprite = selectedBuilding.WorkingFolks[WorkerSlots.IndexOf(workerSlot)].Icon;
            
            workerSlot.transform.DOScale(new Vector3(1f, 1f, 1f), .2f);
            FolkPanels[index].transform.DOScale(new Vector3(.9f, .9f, .9f), .2f).onComplete = () => { FolkPanels[index].transform.DOScale(new Vector3(1f, 1f, 1f), .2f) ;};
            
            folkPanel.UsedFolk.navigation.GoTo(folkPanel.UsedFolk.AssignedBuilding.transform.position);
            if(!folkPanel.UsedFolk.ModelHolder.gameObject.activeInHierarchy) folkPanel.UsedFolk.ModelHolder.gameObject.SetActive(true);

        } else {

            uiManager.instance.OpenPopUp("Folk is already working. Do you really wanna assign?");
            PopUp.instance.AssignButtons(() => {
                
                uiManager.instance.ClosePopUp();
                folkPanel.UsedFolk.AssignedBuilding.WorkingFolks.Remove(folkPanel.UsedFolk);
                folkPanel.UsedFolk.AssignedBuilding = null;
                AssignWorker(index, workerSlot);
            }, () 
            => uiManager.instance.ClosePopUp());
        }
    }

    void GroupingON() {
        
        if(!Grouping) {

            if(TownfolkManager.instance.TownfolkGroups.Count <= 1) {
                TabHandler.instance.CreateTab();
            }

            Grouping = true;
            SelectionMisc(true);

            for (int i = 0; i < FolkPanels.Count; i++){
                AssignFolkButtons(i, true);
            }

            TabHandler.instance.OpenTab(TabHandler.instance.DefaultTab);
        }
    }

    void PositioningON() {

        if(!Positioning) {

            Positioning = true;
            SelectionMisc(false);

            for (int i = 0; i < FolkPanels.Count; i++) {
                AssignFolkButtons(i, false);
            }
        }
    }

    void SelectionMisc(bool grouping) {
        WorkerPanel.SetActive(true);

        uiManager.instance.ConfirmButton.gameObject.SetActive(true);
        uiManager.instance.ConfirmButton.onClick.RemoveAllListeners();
        uiManager.instance.ConfirmButton.onClick.AddListener(() => confirmButton(grouping ? true : false) );

        GameManager.instance.ShowNightButton();
    }

    void AssignFolkButtons(int index, bool grouping) {
        FolkPanels[index].GetComponent<Button>().onClick.RemoveAllListeners();
        FolkPanels[index].GetComponent<Button>().onClick.AddListener(() => {
            SelectFolk(index, grouping);
        });
    }

    void SelectFolk(int index, bool grouping) {

        Townfolk selectedFolk = TownfolkManager.instance.Townfolks[index];
        
        if(!selectedFolks.Contains(TownfolkManager.instance.Townfolks[index])){

            if(Positioning) {
                if(selectedFolk.AssignedBuilding != null) {
                    return;
                }
            }

            selectedFolks.Add(TownfolkManager.instance.Townfolks[index]);

            GameObject selectedFolkWorkersSlot = Instantiate(WorkerSlotPrefab, WorkerPanel.transform);
            selectedFolkWorkersSlot.GetComponent<WorkerSlot>().WorkerIcon.sprite = TownfolkManager.instance.Townfolks[index].Icon;
            WorkerSlots.Add(selectedFolkWorkersSlot);

            TownfolkManager.instance.Townfolks[index].selectedFolkSlot = FolkPanels[index];
            selectedFolksPanels.Add(FolkPanels[index]);

            if(grouping) {
                TownfolkManager.instance.AddFolkIntoGroup(TownfolkManager.instance.GroupIndex, TownfolkManager.instance.Townfolks[index]);
            }
        } else {
            selectedFolks.Remove(selectedFolk);
            Destroy(TownfolkManager.instance.Townfolks[index].selectedFolkSlot);
            selectedFolksPanels.Remove(TownfolkManager.instance.Townfolks[index].selectedFolkSlot);
        }
    }

    void AssignCreateGroupButton() {

        CreateGroupButton.onClick.RemoveAllListeners();
        CreateGroupButton.onClick.AddListener(() => { TabHandler.instance.CreateTab(); });
    }

    void confirmButton(bool grouping) {

        if(grouping) {

            for (int i = 0; i < selectedFolksPanels.Count; i++) {
                selectedFolksPanels[i].GetComponent<FolkPanel>().UsedFolk.AssignedGroup = TownfolkManager.instance.TownfolkGroups[TownfolkManager.instance.GroupIndex];
                selectedFolksPanels[i].transform.SetParent(selectedFolksPanels[i].GetComponent<FolkPanel>().UsedFolk.AssignedGroup.AssignedTab);
            }

            Grouping = false;

            foreach (var slot in WorkerSlots ) {
                Destroy(slot);
            }
            WorkerSlots.Clear();
            selectedFolksPanels.Clear();

            TabHandler.instance.OpenTab(TabHandler.instance.UsedTab);
            WorkerPanel.SetActive(false);
            uiManager.instance.ConfirmButton.gameObject.SetActive(false);

        } 
        else if(Positioning) {

            CloseFolkPanel();

            uiManager.instance.ConfirmButton.transform.DOLocalMoveX(0, 1);

            uiManager.instance.ConfirmButton.onClick.AddListener(() => {

                Positioning = false;            
                uiManager.instance.ConfirmButton.transform.DOLocalMoveX(450, 1);
                uiManager.instance.ConfirmButton.gameObject.SetActive(false);
            });
        } else {

            selectedFolks.Clear();
            GameManager.instance.ShowNightButton();
        }
        
    }
}
