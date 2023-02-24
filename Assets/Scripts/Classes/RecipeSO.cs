using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOs/New Recipe", menuName = "New Recipe")]
public class RecipeSO : ScriptableObject
{
    public ItemNeeded[] items;
    public ElementSO output;
    public int craftTime;
}

[System.Serializable]
public class ItemNeeded
{
    public ElementSO element;
    public int amount;
}
