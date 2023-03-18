using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public static Player instance;

    public string PlayerName;

    public int PlayerLevel;
    public float PlayerExp;
    public float PlayerExpNeeded;

    public float Money;

    public Warehouse PlayerWarehouse;

    public bool PlayerSet;

    private void Awake() {
        instance = this;
    }

    void Start() {
        if(!PlayerSet) {

            PlayerLevel = 1;
            PlayerExp = 0;
            PlayerExpNeeded = 333;
            Money = 100;
            
            PlayerSet = true;
        }

        TopPanel.instance.SetPlayerText();
    }

    public void AddPlayerExp(float amount) {

        if(PlayerExp + amount < PlayerExpNeeded) {
            PlayerExp += amount;
        } else {
            PlayerLevel++;
            PlayerExp = (PlayerExp + amount) - PlayerExpNeeded;
        }

        TopPanel.instance.UpdateXpBar();
    }

    public void AddMoney(float amount) {

        Money += amount;

        TopPanel.instance.UpdateCurrencyText();
    }
}
