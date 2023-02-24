using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIShopSlot : MonoBehaviour
{
    public enum ShopType {  element, machine }
    public ShopType type;
    public Computer comp;
    public int id;
    public TextMeshProUGUI title;
    public TextMeshProUGUI cost;
    public RawImage icon;
    public Button btn;

    private void Awake()
    {
        switch(type)
        {
            case ShopType.element:
                btn.onClick.AddListener(delegate { comp.OrderElement(id); });
                break;
            case ShopType.machine:
                btn.onClick.AddListener(delegate { comp.OrderMachine(id); });
                break;
        }
    }
}
