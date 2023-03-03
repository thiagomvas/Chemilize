using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public List<StructureSO> structureTypes = new List<StructureSO>();
    public List<IObject> objects = new List<IObject>();

    private void Start()
    {
        structureTypes = ReferencesManager.Instance.structures;
        SerializationManager.LoadGame("save.save");
        foreach(ObjectData d in SaveData.Current.objects)
        {
            GameObject obj = Instantiate(structureTypes[d.id].prefab, d.position, d.rotation);
            obj.GetComponent<IObject>().objectData = d;
            if(obj.TryGetComponent<ItemTransport>(out ItemTransport it))
            {
                PipeData p = SaveData.Current.pipePositions[d.settingsIndex];
                it.UpdatePositions(p.input, p.output);
            }
        }
    }
}
