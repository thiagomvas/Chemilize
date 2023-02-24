using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SOs/StructureSO", fileName = "New Structure")]
public class StructureSO : ScriptableObject
{
    public GameObject prefab;
    public string name;
    public int cost;
    public Sprite icon;
    public int damage;
}
