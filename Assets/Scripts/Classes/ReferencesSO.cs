using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "References", menuName = "SOs/References")]
public class ReferencesSO : ScriptableObject
{
    public List<ElementSO> elements;
    public List<RecipeSO> recipes;
    public List<StructureSO> structures;
}
