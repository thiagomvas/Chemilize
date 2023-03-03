using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour, IObject, IGrabbable
{
    public ObjectData objectData { get => data; set => data = value; }
    ObjectData data;
    public Vector3 point { get => Vector3.zero; set => point = value; }
    public bool isAutomatic { get => false; set => isAutomatic = value; }


    private void UpdateData()
    {
        SaveData.Current.objects[objectData.index].position = this.transform.position;
        SaveData.Current.objects[objectData.index].rotation = this.transform.rotation;
    }
    private void OnEnable() => EventManager.onSave += UpdateData;
    private void OnDisable() => EventManager.onSave -= UpdateData;
}
