using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Crafter : MonoBehaviour, IInteractable, IObject, IGrabbable
{
    public bool isAutomatic { get { return auto; } set { auto = value; } }
    bool auto;
    public Vector3 point { get { return spawnPoint.position; } }
    public ObjectData objectData
    {
        get { return data; }
        set { data = value;}
    }
    [SerializeField] private TextMeshProUGUI recipeTM;
    [SerializeField] private TextMeshProUGUI progressTM;
    [SerializeField] private ObjectData data;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject basePrefab;
    public RecipeSO[] recipes;
    [SerializeField] private RecipeSO selectedRecipe;

    [SerializeField] private List<StoredItems> slots = new List<StoredItems>();
    string missingItem;

    float nextCraft;
    bool hasResourcesToCraft = false;
    bool isCrafting = false;
    private void Start()
    {
        UpdateSelection(0);
        if (data.index < 0)
        {
            data.index = SaveData.Current.objects.Count;
            SaveData.Current.objects.Add(objectData);
        }
    }

    private void Update()
    {
        UpdateText();
        if (!isAutomatic) return;
        if (isAutomatic && ReferencesManager.Instance.timer >= nextCraft && hasResourcesToCraft)
        {
            Interact();
        }
    }

    public void Interact()
    {
        CheckForCraft();
        if (ReferencesManager.Instance.timer >= nextCraft && hasResourcesToCraft)
        {
            nextCraft = ReferencesManager.Instance.timer + selectedRecipe.craftTime;
            StartCoroutine(Craft());
        }
    }
    public void SecondaryInteract()
    {
        int index = data.settingsIndex;        
        UpdateSelection(index + 1 >= recipes.Length ? 0 : index + 1);
        CheckForCraft();
    }

    private void UpdateText()
    {
        if (!hasResourcesToCraft) progressTM.text = missingItem;
        else if (!isCrafting) progressTM.text = "Interact to craft.";
    }

    public void UpdateSelection(int index)
    {
        slots.Clear();
        selectedRecipe = recipes[index];
        data.settingsIndex = index;
        //SaveData.Current.grids[data.gridIndex].tiles[data.index].settingsIndex = index;

        for(int i = 0; i < selectedRecipe.items.Length; i++)
        {
            slots.Add(new StoredItems(selectedRecipe.items[i].element));
        }
        recipeTM.text = $"Selected: {selectedRecipe.output.name}";
    }

    private void CheckForCraft()
    {
        missingItem = "";
        hasResourcesToCraft = true;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].amount < selectedRecipe.items[i].amount)
            {
                hasResourcesToCraft = false;
                missingItem += $"{selectedRecipe.items[i].amount - slots[i].amount}x {slots[i].item.name}\n";
            }
        }
    }
    private IEnumerator Craft()
    {
        isCrafting = true;
        int progress = 0;
        for(int i = 0; i <slots.Count; i++)
        {
            slots[i].amount -= selectedRecipe.items[i].amount;
        }

        while (progress < 100)
        {
            yield return new WaitForSeconds(selectedRecipe.craftTime * 0.01f);
            progress++;
            progressTM.text = $"{progress}%";
        }
        CheckForCraft();
        GameObject obj = Instantiate(selectedRecipe.output.glasswarePrefab, spawnPoint.position, Quaternion.identity);
        obj.GetComponent<Element>().element = selectedRecipe.output;
        isCrafting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Element>(out Element e))
        {
            for(int i = 0; i < slots.Count; i++)
            {
                if (e.element == slots[i].item)
                { 
                    slots[i].amount++;
                    Destroy(e.gameObject);
                    CheckForCraft();
                }
                
            }
        }
    }
}

[System.Serializable]
public class StoredItems
{
    public ElementSO item;
    public int amount;
    public StoredItems(ElementSO item)
    {
        this.item = item;
        this.amount = 0;
    }
}
