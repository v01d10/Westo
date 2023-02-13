using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GoodieType
{
    Resource, Weapon, Ammo, Clothing
}

[CreateAssetMenu(fileName = "Goodie", menuName = "New Goodie")]
public class Goodie : ScriptableObject{

    public string GoodieName;

    public float GoodiePrice;
    public float GoodieAmount;
    
    public GoodieType goodieType;
}
