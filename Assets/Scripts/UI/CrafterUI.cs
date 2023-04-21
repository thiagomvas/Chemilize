using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(Crafter))]
public class CrafterUI : MonoBehaviour
{
    [SerializeField] private Crafter crafter;
    public List<string> recipeNames = new List<string>();
        [SerializeField] private TextMeshProUGUI progressTm;
    [SerializeField] private TextMeshProUGUI recipeTm;

    private void OnEnable()
    {
        crafter.onUIUpdate += UpdateUI;
    }

    private void OnDisable()
    {
        crafter.onUIUpdate -= UpdateUI;
    }

    private void UpdateUI()
    {
        
    }
}
