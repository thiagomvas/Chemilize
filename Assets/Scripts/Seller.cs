using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seller : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Element>(out Element e))
        {
            if (e.element != null) SaveData.Current.currency += e.element.sellValue;
            Destroy(other.gameObject);
        }
    }
}
