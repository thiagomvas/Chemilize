using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SOs/StructureSO", fileName = "New Structure")]
public class StructureSO : ScriptableObject
{
    public bool purchaseable;
    public int id = 9999;
    public GameObject prefab;
    public string name;
    public int cost;
    public int energyCost;
    public Sprite icon;
    public int damage;
    [Header("Research")]
    public ResearchManager.ResearchTree tree;
    public int levelReq;
}
