using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "New Processed Goodie")]
public class GoodieProcessed : ScriptableObject
{
    public string ProcessedGoodieName;

    public float ProcessedGoodiePrice;
    public float ProcessedGoodieAmount;
}
