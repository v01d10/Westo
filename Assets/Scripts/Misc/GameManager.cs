using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

    public Button NightButton;

    private void Awake() {
        instance = this;
    }

    void Start() {

        NightButton.onClick.AddListener(() => { UpdateGameState(GameState.Night); }); 
    }

    public void ShowNightButton() {
        if(NightButton.gameObject.activeInHierarchy) {
            NightButton.gameObject.SetActive(false);
        } else {
            NightButton.gameObject.SetActive(true);
        }
    }

    public void UpdateGameState(GameState newState) {
        State = newState;

        switch(newState) {
            case GameState.Day:
                SetDay();
                break;
            case GameState.Night:
                SetNight();
                break;
            default:
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    void SetDay() {
        ShowNightButton();
    }

    void SetNight() {
        NightButton.gameObject.SetActive(false);
        Camera.main.GetComponentInParent<MobileCameraController>().enabled = false;
        
        NightManager.instance.StartNextNight();
    }
}

public enum GameState {
    Day,
    Night

}
