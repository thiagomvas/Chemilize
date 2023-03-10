using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemGenerator : MonoBehaviour, IInteractable, IObject
{
    StructureSO so;
    public bool isAutomatic { get { return automaticOutput; } set { automaticOutput = value; } }
    public Vector3 point { get { return generatePoint.position; } set { generatePoint.position = value; } }
    public ObjectData objectData
    {
        get { return data; }
        set { data = value; }
    }
    [SerializeField] private ObjectData data;
    [Header("Settings")]
    [SerializeField] private TextMeshProUGUI infotm;
    [SerializeField] private bool RandomizeOutputs = false;
    [SerializeField] private bool automaticOutput = true;
    [SerializeField] private bool clickToGenerate = false;
    [SerializeField] private float baseDelay;
    private float efficiency = 1;
    [Space]
    [SerializeField] private Transform generatePoint;
    public int outputIndex;
    public List<ElementSO> outputs = new List<ElementSO>();

    float nextGeneration;

    private void Start()
    {
        outputIndex = 0;
        UpdateText();
        if (data.id >= 0) so = ReferencesManager.Instance.structures[data.id];
    }

    void Update()
    {
        float timer = SaveData.Current.playtime;
        if(timer >= nextGeneration)
        {
            nextGeneration = timer + baseDelay / efficiency;
            if(automaticOutput)
            {
                SpawnProduct();
            }
        }
    }

    private void SpawnProduct()
    {
        int x = RandomizeOutputs ? Random.Range(0, outputs.Count) : outputIndex ;
        GameObject g = Instantiate(outputs[x].glasswarePrefab, generatePoint.position, Quaternion.identity);
        g.GetComponent<Element>().element = outputs[x];
        g.name = outputs[x].name;
        if (so) SaveData.Current.energyUnits -= so.energyCost;
    }

    public void Interact()
    {
        if (!clickToGenerate) return;
        SpawnProduct(); 
    }
    public void SecondaryInteract() => Interact();

    public void UpdateOutput(int index)
    {
        outputIndex = index;
        UpdateText();
    }

    private void UpdateText()
    { if(infotm) infotm.text = $"Producing {outputs[outputIndex].name}"; }

    private void UpdateData()
    {
        if (objectData.index < 0) return;
        SaveData.Current.objects[objectData.index].position = this.transform.position;
        SaveData.Current.objects[objectData.index].rotation = this.transform.rotation;
    }
    private void OnEnable() => EventManager.onSave += UpdateData;
    private void OnDisable() => EventManager.onSave -= UpdateData;
}
