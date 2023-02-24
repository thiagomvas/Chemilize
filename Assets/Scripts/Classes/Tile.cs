using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Transform building;
    public TileData data;
}

[System.Serializable]
public class TileData
{
    public int index;
    public int gridIndex;
    public bool hasBuilding;
    public int structureId;
    public int rotation;
    public int variation;
    public int settingsIndex;
    public int variableIndex;
}

