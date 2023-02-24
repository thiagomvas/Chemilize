using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOs/New Element", menuName = "New Element")]
public class ElementSO : ScriptableObject
{
    public enum PhysicalState { solid, liquid, gas }
    public int id = 9999;
    [Header("Basic Settings")]
    public string symbol;
    public string name;
    public int sellValue;
    public int buyCost;
    [Header("Visuals")]
    public Color cpkHexColor;
    public GameObject glasswarePrefab;
    public bool glow;
    public bool metallic;
    [Header("Physical Info")]
    public PhysicalState standardState;
    public float density;
    [Header("Atomic Info")]
    public float atomicMass;
    public int atomicNumber;
    [Header("Temperature Info")]
    public int boilingPoint;
    public int meltingPoint;
    public int ionizationEnergy;
    [Header("Misc Info")]
    public string electronicConfiguration;
    public string groupBlock;
    public string yearDiscovered;
}

