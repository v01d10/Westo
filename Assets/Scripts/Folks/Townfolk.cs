using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TownfolkRole{
    None, Lumber, Miner, Crafter, Fighter,
}

public class Townfolk : MonoBehaviour
{
    public string Name;

    public float Health;

    public TownfolkRole Role;

[Header("Basic Attributes")]
    public float Level;
    public float Exp;
    public float ExpNeeded;

    public float Strength;
    public float Inteligence;
    public float Agility;






}

