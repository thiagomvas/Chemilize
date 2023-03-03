using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOs/New Recipe", menuName = "New Recipe")]
public class RecipeSO : ScriptableObject
{
    public string displayName;
    public ItemNeeded[] items;
    public ItemNeeded[] outputs;
    public int craftTime;
    public StructureSO madeInMachine;
    [Header("Research")]
    public ResearchManager.ResearchTree tree;
    public int levelReq;
    [Header("Object Crafting")]
    public bool isObject;
    public StructureSO objectOutput;
}

[System.Serializable]
public class ItemNeeded
{
    public ElementSO element;
    public int amount = 1;
}
