using UnityEngine;
using TMPro;

public class CashUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpro;
    void Update()
    {
        tmpro.text = $"Bank: {SaveData.Current.currency}";
    }
}
