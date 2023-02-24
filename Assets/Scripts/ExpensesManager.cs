using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExpensesManager : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private int rentCost;
    [SerializeField] private int energyUnitCost;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI owedTM;
    [SerializeField] private TextMeshProUGUI nextRentTM;

    float nextAdd;
    private void Start()
    {
        nextAdd = delay - (SaveData.Current.playtime % delay);
        owedTM.text = $"Amount owed: {SaveData.Current.amountOwed}";
    }
    private void Update()
    {
        UpdateTime();
        float time = SaveData.Current.playtime;
        
        if(time >= nextAdd)
        {            
            SaveData.Current.amountOwed += rentCost;
            owedTM.text = $"Amount owed: {SaveData.Current.amountOwed}";
            nextAdd = time + delay;
        }
    }

    private void UpdateTime()
    {
        float time = nextAdd - SaveData.Current.playtime;
        int hours = Mathf.FloorToInt(time/3600);
        int minutes = Mathf.FloorToInt((time / 60) % 60);
        int seconds = Mathf.FloorToInt(time % 60);
        nextRentTM.text = $"Time until next bill: {hours}h {minutes}m {seconds}s";
    }
    public void PayBills()
    {
        if(SaveData.Current.currency >= SaveData.Current.amountOwed)
        {            
            SaveData.Current.currency -= SaveData.Current.amountOwed;
            SaveData.Current.amountOwed = 0;
            owedTM.text = $"Amount owed: {SaveData.Current.amountOwed}";
        }
        else
        {
            SaveData.Current.amountOwed -= SaveData.Current.currency;
            SaveData.Current.currency = 0;
            owedTM.text = $"Amount owed: {SaveData.Current.amountOwed}";
        }
    }
}


