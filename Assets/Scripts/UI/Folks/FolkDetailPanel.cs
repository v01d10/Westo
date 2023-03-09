using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FolkDetailPanel : MonoBehaviour
{
    public Image FolkIcon;

    public TextMeshProUGUI FolkName;
    public TextMeshProUGUI FolkHP;
    public TextMeshProUGUI FolkRole;

    public Image FolkXpBar;
    public TextMeshProUGUI FolkLevel;
    public TextMeshProUGUI FolkAtPoints;
    public TextMeshProUGUI FolkStrength;
    public Button FolkStrengthIncrease;
    public TextMeshProUGUI FolkDexterity;
    public Button FolkDexterityIncrease;
    public TextMeshProUGUI FolkInteligence;
    public Button FolkInteligenceIncrease;

    public void LoadDetailText() {

        Townfolk folk = TownfolkManager.instance.SelectedFolk;

        FolkIcon.sprite = folk.Icon;

        FolkName.text = folk.Name;
        FolkHP.text = (((int)folk.Health).ToString() + "   HP");
        FolkRole.text = folk.Role.ToString();
        FolkLevel.text = folk.Level.ToString();
        FolkAtPoints.text = folk.AtPoints.ToString();

        FolkStrength.text = folk.Strength.ToString();
        FolkStrengthIncrease.onClick.AddListener(() => folk.IncreaseStat(0));
        FolkDexterity.text = folk.Dexterity.ToString();
        FolkDexterityIncrease.onClick.AddListener(() => folk.IncreaseStat(1));
        FolkInteligence.text = folk.Inteligence.ToString();
        FolkInteligenceIncrease.onClick.AddListener(() => folk.IncreaseStat(2));

        FolkXpBar.fillAmount = folk.Exp / folk.ExpNeeded; 
    }
}
