using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour, IInteractable
{
    [SerializeField] private MouseLook ml;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject shopMenuSlot;
    [SerializeField] private Transform[] parents;
    [SerializeField] private List<ElementSO> elements = new List<ElementSO>();
    [SerializeField] private List<StructureSO> machines = new List<StructureSO>();
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        GenerateShopIcons();
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
            UIShopSlot slot = Instantiate(shopMenuSlot, parents[1]).GetComponent<UIShopSlot>();
            slot.comp = this;
            slot.type = UIShopSlot.ShopType.machine;
            slot.id = i;
            slot.name = machines[i].name;
            slot.title.text = machines[i].name;
            slot.cost.text = $"Cost: {machines[i].cost}";
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

    }

    public void Interact() => UIManager.Instance.ToggleMenu(UIManager.Instance.computerMenu);
    public void SecondaryInteract() => Interact();

}
