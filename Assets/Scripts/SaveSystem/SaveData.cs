using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // Singleton for easy access
    private static SaveData _current;
    public static SaveData Current
    {
        get
        {
            if(_current == null)           
                _current = new SaveData();
            return _current;
            
        }
        set
        {
            if(_current != value)
            {
                _current = value;
            }
        }
        
    }


    //Data i wish to save
    public int currency = 500;
    public int amountOwed;
    public float playtime = 0;
    public List<ObjectData> objects = new List<ObjectData>();



    public List<StorageData> storage = new List<StorageData>();
    public List<GridData> grids = new List<GridData>();
    public List<BarrierData> barriers = new List<BarrierData>();

}

