using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierManager : MonoBehaviour
{
    [SerializeField] private List<Barrier> barriers = new List<Barrier>();
    // Start is called before the first frame update
    void Start()
    {
        SerializationManager.LoadGame("save.save");
        for (int i = 0; i < barriers.Count; i++)
        {
            barriers[i].data.index = i;
            if (i == SaveData.Current.barriers.Count) SaveData.Current.barriers.Add(barriers[i].data);
            else barriers[i].data = SaveData.Current.barriers[i];
        }
        SerializationManager.SaveGame("save", SaveData.Current);
    }

}
