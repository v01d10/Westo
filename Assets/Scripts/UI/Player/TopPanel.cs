using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopPanel : MonoBehaviour
{
    public static TopPanel instance;

    public TextMeshProUGUI PlayerNameText;
    public TextMeshProUGUI PlayerLevelText;

    public TextMeshProUGUI PlayerMoneyText;
    public TextMeshProUGUI PlayerGoldText;
    
    public TextMeshProUGUI PlayerExpText;
    public Image PlayerXpBar;

    private void Awake() {
        instance = this;
    }

    public void SetPlayerText () {

        PlayerNameText.text = Player.instance.PlayerName;
        
        UpdateCurrencyText();
        UpdateXpBar();
    }

    public void UpdateCurrencyText() {

        PlayerMoneyText.text = Player.instance.Money.ToString();

    }

    public void UpdateXpBar() {

        PlayerXpBar.fillAmount = Player.instance.PlayerExp / Player.instance.PlayerExpNeeded;
        PlayerExpText.text = Player.instance.PlayerExp + " / " + Player.instance.PlayerExpNeeded;
        PlayerLevelText.text = Player.instance.PlayerLevel.ToString();
    }
}
