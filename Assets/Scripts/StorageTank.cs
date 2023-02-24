using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StorageTank : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displaytm;
    [SerializeField] public TileData data;
    [SerializeField] private int storedAmount = 0;
    [SerializeField] private int elementNumber = 0;
    [SerializeField] private Transform outputPoint;
    [SerializeField] public bool OutputOn = false;

    float nextDrop;
    private void Start()
    {
        elementNumber = data.settingsIndex;
        storedAmount = data.variableIndex;
        displaytm.text = $"{ReferencesManager.Instance.elements[elementNumber].name}: {storedAmount}\nOutput: {(OutputOn == true ? "ON" : "OFF")}";
    }

    private void Update()
    {
        displaytm.text = $"{ReferencesManager.Instance.elements[elementNumber].name}: {storedAmount}\nOutput: {(OutputOn == true? "ON" : "OFF")}";
        if(OutputOn && ReferencesManager.Instance.timer >= nextDrop)
        {
            nextDrop = ReferencesManager.Instance.timer + 0.1f;
            GameObject obj = Instantiate(ReferencesManager.Instance.elements[elementNumber].glasswarePrefab, outputPoint.position, Quaternion.identity);
            obj.GetComponent<Element>().element = ReferencesManager.Instance.elements[elementNumber];
            data.variableIndex--;
            storedAmount--;
            SaveData.Current.grids[data.gridIndex].tiles[data.index] = data; 
            SaveData.Current.grids[data.gridIndex].tiles[data.index] = data; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Element>(out Element element))
        {
            if (elementNumber <= 0) 
            { 
                elementNumber = ReferencesManager.Instance.elements.IndexOf(element.element);
                SaveData.Current.grids[data.gridIndex].tiles[data.index].settingsIndex = elementNumber;
            } 
            if (element.element.atomicNumber != elementNumber) return;
            Destroy(other.gameObject);
            data.variableIndex++;
            storedAmount++;
            SaveData.Current.grids[data.gridIndex].tiles[data.index] = data;

        }
    }
}
