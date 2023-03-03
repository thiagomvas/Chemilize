using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchManager : MonoBehaviour
{
    public enum ResearchTree { ProductionEfficiency, Extraction, Energy }
    public List<TreeReferences> treeReferences = new List<TreeReferences>();
    [SerializeField] private Color unlockColor;
    [SerializeField] private Computer comp;

    private void Start()
    {
        UpdateButtons();
    }

    public void UpdateButtons()
    {
        for(int t = 0; t < treeReferences.Count; t++)
        {
            for(int i = 0; i < treeReferences[t].buttons.Count; i++) {
                
                treeReferences[t].buttons[i].SetActive(false);

                switch(t)
                {
                    case 0:
                        if(i < SaveData.Current.productionEfficiencyLevel) treeReferences[t].icons[i].color = unlockColor;
                        if(i == SaveData.Current.productionEfficiencyLevel) treeReferences[t].buttons[i].SetActive(true);
                        break;
                    case 1:
                        if (i < SaveData.Current.extractionLevel) treeReferences[t].icons[i].color = unlockColor;
                        if (i == SaveData.Current.extractionLevel) treeReferences[t].buttons[i].SetActive(true);
                        break;
                    case 2:
                        if (i < SaveData.Current.energyEfficiencyLevel) treeReferences[t].icons[i].color = unlockColor;
                        if (i == SaveData.Current.energyEfficiencyLevel) treeReferences[t].buttons[i].SetActive(true);
                        break;
                }                
            }
        }
    }
    
    public void Research(int treeIndex)
    {
        ResearchTree tree = (ResearchTree)treeIndex;
        if (SaveData.Current.chemiPoints <= 0) return;
        switch(tree)
        {
            case ResearchTree.ProductionEfficiency:
                SaveData.Current.productionEfficiencyLevel++;
                break;
            case ResearchTree.Extraction:
                SaveData.Current.extractionLevel++;
                break;
            case ResearchTree.Energy:
                SaveData.Current.energyEfficiencyLevel++;
                break;
        }
        UpdateButtons();
        comp.UpdateSlots();
    }
}

[System.Serializable]
public class TreeReferences
{
    public List<RawImage> icons;
    public List<GameObject> buttons;
}