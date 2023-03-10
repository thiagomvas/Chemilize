using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour, IInteractable
{
    [SerializeField] private MouseLook ml;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject shopMenuSlot;
    [SerializeField] private Transform[] parents;
    private List<ElementSO> elements = new List<ElementSO>();
    private List<StructureSO> machines = new List<StructureSO>();
    [SerializeField] private Transform spawnPoint;

    private List<UIShopSlot> elementSlots;
    private List<UIShopSlot> machineSlots = new List<UIShopSlot>();

    private void Start()
    {
        machines = ReferencesManager.Instance.structures;
        elements = ReferencesManager.Instance.elements;
        GenerateShopIcons();
    }

    public void UpdateSlots()
    {
        foreach (UIShopSlot slot in machineSlots)
        {
            switch (machines[slot.id].tree)
            {
                case ResearchManager.ResearchTree.ProductionEfficiency:
                    if (machines[slot.id].levelReq > SaveData.Current.productionEfficiencyLevel) slot.btn.gameObject.SetActive(false);
                    else slot.btn.gameObject.SetActive(true);
                    break;
                case ResearchManager.ResearchTree.Extraction:
                    if (machines[slot.id].levelReq > SaveData.Current.extractionLevel) slot.btn.gameObject.SetActive(false);
                    else slot.btn.gameObject.SetActive(true);
                    break;
                case ResearchManager.ResearchTree.Energy:
                    if (machines[slot.id].levelReq > SaveData.Current.energyEfficiencyLevel) slot.btn.gameObject.SetActive(false);
                    else slot.btn.gameObject.SetActive(true);
                    break;
            }
        }
    }
    private void GenerateShopIcons()
    {
        for (int i = 0; i < elements.Count; i++)
        {
            UIShopSlot slot = Instantiate(shopMenuSlot, parents[0]).GetComponent<UIShopSlot>();
            slot.comp = this;
            slot.type = UIShopSlot.ShopType.element;
            slot.id = i;
            slot.name = elements[i].name;
            slot.title.text = elements[i].name;
            slot.cost.text = $"Cost: {elements[i].buyCost}";
        }
        for (int i = 0; i < machines.Count; i++)
        {
            if (!machines[i].purchaseable) continue;
            UIShopSlot slot = Instantiate(shopMenuSlot, parents[1]).GetComponent<UIShopSlot>();
            slot.comp = this;
            slot.type = UIShopSlot.ShopType.machine;
            slot.id = i;
            slot.name = machines[i].name;
            slot.title.text = machines[i].name;
            slot.cost.text = $"Cost: {machines[i].cost}";
            machineSlots.Add(slot);

            switch(machines[i].tree)
            {
                case ResearchManager.ResearchTree.ProductionEfficiency:
                    if (machines[i].levelReq > SaveData.Current.productionEfficiencyLevel) slot.btn.gameObject.SetActive(false);
                    break;
                case ResearchManager.ResearchTree.Extraction:
                    if (machines[i].levelReq > SaveData.Current.extractionLevel) slot.btn.gameObject.SetActive(false);
                    break;
                case ResearchManager.ResearchTree.Energy:
                    if (machines[i].levelReq > SaveData.Current.energyEfficiencyLevel) slot.btn.gameObject.SetActive(false);
                    break;
            }
        }
    }
    public void OrderElement(int index)
    {
        if (SaveData.Current.currency < elements[index].buyCost) return;
        SaveData.Current.currency -= elements[index].buyCost;

        GameObject obj = Instantiate(elements[index].glasswarePrefab, spawnPoint.position, Quaternion.identity);
        obj.GetComponent<Element>().element = elements[index];
    }
    public void OrderMachine(int index)
    {
        if (SaveData.Current.currency < machines[index].cost) return;
        SaveData.Current.currency -= machines[index].cost;

        GameObject obj = Instantiate(machines[index].prefab, spawnPoint.position, Quaternion.identity);
        IObject io = obj.GetComponent<IObject>();
        SaveData.Current.objects.Add(new ObjectData(SaveData.Current.objects.Count, index));
        SaveData.Current.objects[SaveData.Current.objects.Count - 1].position = spawnPoint.position;

        io.objectData = SaveData.Current.objects[SaveData.Current.objects.Count - 1];

        Debug.Log(SaveData.Current.objects.Count);
    }

    public void Interact() => UIManager.Instance.ToggleMenu(UIManager.Instance.computerMenu);
    public void SecondaryInteract() => Interact();

}
