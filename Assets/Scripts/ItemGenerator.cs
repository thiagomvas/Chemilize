using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemGenerator : MonoBehaviour, IInteractable, IObject
{
    public bool isAutomatic { get { return auto; } set { auto = value; } }
    bool auto;
    public Vector3 point { get { return generatePoint.position; } }
    public ObjectData objectData
    {
        get { return data; }
        set { data = value; }
    }
    [SerializeField] private ObjectData data;
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
    }

    void Update()
    {
        float timer = SaveData.Current.playtime;
        if(timer >= nextGeneration)
        {
            if(automaticOutput)
            {
                nextGeneration = timer + baseDelay / efficiency;
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
    }
}
