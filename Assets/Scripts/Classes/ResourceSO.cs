using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SOs/New Resource", menuName = "New Resource")]
public class ResourceSO : ScriptableObject
{
    public string symbol;
    public string name;
    public int sellValue;
    [Space]
    public Color color;
    public Mesh mesh;
}
