using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Recipe")]
public class Recipe : ScriptableObject {
    
    public List<Goodie> ingredients = new List<Goodie>();
    
    public GoodieProcessed FinalProduct;

    public float ingredientCost0;
    public float ingredientCost1;
    public float ingredientCost2;

    public float processingTime;
}
