using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ContractorUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private Contractor cont;
    private void Awake()
    {
        cont.onNewContract += UpdateUI;
    }

    private void UpdateUI()
    {
        textField.text = $"Current Contract:{cont.contractElement.name} for {Mathf.FloorToInt(cont.reward)}";
    }
}
