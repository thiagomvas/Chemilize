using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectData
{
    public int index = -1;
    public int id = -1;
    public Vector3 position;
    public Quaternion rotation;
    public int settingsIndex = -1;
    public int stored;

    public ObjectData (int index,int id)
    {
        this.index = index;
        this.id = id;
    }
}
