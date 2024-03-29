using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Crafter : MonoBehaviour, IInteractable, IObject, IGrabbable
{
    [SerializeField] private StructureSO so;
    public bool isAutomatic { get { return auto; } set { auto = value; } }
    [SerializeField] private bool auto;
    public Vector3 point { get { return spawnPoint.position; } set { spawnPoint.position = value; } }
    private Vector3 newSpawnPoint;
    public ObjectData objectData
    {
        get { return data; }
        set { data = value; }
    }
    [SerializeField] private ObjectData data;
    [SerializeField] private Transform spawnPoint;
    private List<RecipeSO> recipes = new List<RecipeSO>();
    [HideInInspector] public RecipeSO selectedRecipe { get; private set; }

    [HideInInspector] public List<StoredItems> slots = new List<StoredItems>();

    public Action onCraftStart;
    public Action onCraftEnd;
    public Action onUIUpdate;

    float nextCraft;
    bool hasResourcesToCraft = false;
    public bool isCrafting { get; private set; }
    public int progress { get; private set; }
    int speedMultiplier = 1; 
    private void Start()
    {
        isCrafting = false;
        foreach(RecipeSO recipe in ReferencesManager.Instance.recipes)
        {
            if (recipe.madeInMachine == so) recipes.Add(recipe);
        }
        UpdateSelection(0);
        if (data.index == -1)
        {
            data.index = SaveData.Current.objects.Count;
            SaveData.Current.objects.Add(objectData);
        }
        if(data.id >= 0 && so == null) so = ReferencesManager.Instance.structures[data.id];

    }
    private void Update()
    {
        if (SaveData.Current.productionEfficiencyLevel == 1) speedMultiplier = 2;
        else if (SaveData.Current.productionEfficiencyLevel == 5) speedMultiplier = 4;
        if (!isAutomatic) return;
        if (isAutomatic && ReferencesManager.Instance.timer >= nextCraft && hasResourcesToCraft)
        {
            Interact();
        }
    }
    public void Interact()
    {
        if (isCrafting) return;
        CheckForCraft();
        if (ReferencesManager.Instance.timer >= nextCraft && hasResourcesToCraft)
        {
            nextCraft = ReferencesManager.Instance.timer + selectedRecipe.craftTime/speedMultiplier;
            StartCoroutine(Craft());
        }
    }
    public void SecondaryInteract()
    {
        if (isCrafting) return;
        int index = data.settingsIndex;        
        UpdateSelection(index + 1 >= recipes.Count ? 0 : index + 1);
        CheckForCraft();
    }
    public void UpdateSelection(int index)
    {
        slots.Clear();
        int x = index;
        bool recipeUnlocked = false;
        while(recipeUnlocked != true)
        {
            switch (recipes[x].tree)
            {
                case ResearchManager.ResearchTree.ProductionEfficiency:
                    if (recipes[x].levelReq <= SaveData.Current.productionEfficiencyLevel) recipeUnlocked = true;
                    else x = x + 1 >= recipes.Count ? 0 : x + 1;
                    break;
                case ResearchManager.ResearchTree.Extraction:
                    if (recipes[x].levelReq <= SaveData.Current.extractionLevel) recipeUnlocked = true;
                    else x = x + 1 >= recipes.Count ? 0 : x + 1;
                    break;
                case ResearchManager.ResearchTree.Energy:
                    if (recipes[x].levelReq <= SaveData.Current.energyEfficiencyLevel) recipeUnlocked = true;
                    else x = x + 1 >= recipes.Count ? 0 : x + 1;
                    break;
            }
            
        }

        selectedRecipe = recipes[x];
        data.settingsIndex = x;
        //SaveData.Current.grids[data.gridIndex].tiles[data.index].settingsIndex = index;

        for(int i = 0; i < selectedRecipe.items.Length; i++)
        {
            slots.Add(new StoredItems(selectedRecipe.items[i].element));
        }
        string name = selectedRecipe.isObject ? selectedRecipe.objectOutput.name : selectedRecipe.displayName;
    }
    private void CheckForCraft()
    {
        hasResourcesToCraft = true;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].amount < selectedRecipe.items[i].amount)
            {
                hasResourcesToCraft = false;
            }
        }
        onUIUpdate?.Invoke();
    }
    private IEnumerator Craft()
    {
        onCraftStart?.Invoke();
        isCrafting = true;
        progress = 0;
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].amount -= selectedRecipe.items[i].amount;
        }

        while (progress < 100)
        {
            yield return new WaitForSeconds(selectedRecipe.craftTime * 0.01f);
            onUIUpdate?.Invoke();
            progress++;
        }

        if(!selectedRecipe.isObject)
        {
            for (int i = 0; i < selectedRecipe.outputs.Length; i++)
            {
                GameObject obj = Instantiate(selectedRecipe.outputs[i].element.glasswarePrefab, spawnPoint.position, Quaternion.identity);
                obj.GetComponent<Element>().element = selectedRecipe.outputs[i].element;
            }
        }
        else
        {
            GameObject obj = Instantiate(selectedRecipe.objectOutput.prefab, point, Quaternion.identity);
            IObject io = obj.GetComponent<IObject>();
            SaveData.Current.objects.Add(new ObjectData(SaveData.Current.objects.Count, selectedRecipe.objectOutput.id));
            SaveData.Current.objects[SaveData.Current.objects.Count - 1].position = spawnPoint.position;


            io.objectData = SaveData.Current.objects[SaveData.Current.objects.Count - 1];
            if(obj.TryGetComponent<ItemTransport>(out ItemTransport it))
            {
                io.objectData.settingsIndex = SaveData.Current.pipePositions.Count;
                SaveData.Current.pipePositions.Add(new PipeData());
                int ind = SaveData.Current.pipePositions.Count - 1;
                SaveData.Current.pipePositions[ind].index = ind;
                SaveData.Current.pipePositions[ind].input = point;
                SaveData.Current.pipePositions[ind].output = point;
            }
        }

        CheckForCraft();
        isCrafting = false;

        if(so)SaveData.Current.energyUnits -= so.energyCost;

        onCraftEnd?.Invoke();

        onUIUpdate?.Invoke();
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
