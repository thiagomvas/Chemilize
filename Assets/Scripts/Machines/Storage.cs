using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
public class Storage : MonoBehaviour, IInteractable, IObject, IGrabbable
{
    
    public bool isAutomatic { get { return auto; } set { auto = value; } }
    bool auto;
    public Vector3 point { get { return spawnPoint.position; } set { spawnPoint.position = value; } }
    public ObjectData objectData
    {
        get { return data; }
        set { data = value; }
    }
    [SerializeField] private ObjectData data;
    [SerializeField] ElementSO elementStored;
    [SerializeField] private TextMeshProUGUI tm;
    [SerializeField] private Transform spawnPoint;
    float nextDrop;
    private void Start()
    {
        if (data.settingsIndex >= 0) elementStored = ReferencesManager.Instance.elements[data.settingsIndex];
        UpdateText();
    }

    private void Update()
    {
        if (isAutomatic && ReferencesManager.Instance.timer > nextDrop)
        {
            nextDrop = ReferencesManager.Instance.timer + 1;
            Interact();
        }
    }
    private void UpdateText()
    {
        if (elementStored) tm.text = $"{elementStored.name}: {data.stored}";
        else tm.text = $"Empty";
    }
    

    public void Interact()
    {
        if (data.stored <= 0) return;
        GameObject obj = Instantiate(elementStored.glasswarePrefab, spawnPoint.position, Quaternion.identity);
        obj.GetComponent<Element>().element = elementStored;
        data.stored--;

        if (data.stored <= 0) { elementStored = null; data.settingsIndex = -1; }
        UpdateText();
    }
    public void SecondaryInteract() => Interact();

    private void OnTriggerEnter(Collider other)
    {        
        if(other.TryGetComponent<Element>(out Element e))
        {
            if(e.element == elementStored)
            {
                data.stored++;
                SaveData.Current.objects[data.index] = data;
                Destroy(e.gameObject);
                UpdateText();
            }
            else if(elementStored == null)
            {
                data.stored = 1;
                data.settingsIndex = e.element.id;
                SaveData.Current.objects[data.index] = data;
                elementStored = ReferencesManager.Instance.elements[data.settingsIndex];
                UpdateText();

                Destroy(e.gameObject);
            }
        }
    }
    private void UpdateData()
    {
        if (objectData.index < 0) return;
        SaveData.Current.objects[objectData.index].position = this.transform.position;
        SaveData.Current.objects[objectData.index].rotation = this.transform.rotation;
    }
    private void OnEnable() => EventManager.onSave += UpdateData;
    private void OnDisable() => EventManager.onSave -= UpdateData;
}

[System.Serializable]
public class StorageData
{
    public int index = -1;
    public int elementId = -1;
    public int amount = 0;
}