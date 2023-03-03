using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Barrier : MonoBehaviour
{
    //bool hasCompleted;
    //public BarrierData data;
    //private int step;
    //[SerializeField] private TextMeshProUGUI tm;
    //[TextArea, SerializeField] private string text;
    //[TextArea, SerializeField] private string completedText;
    //[SerializeField] private List<ElementSO> requirements = new List<ElementSO>();
    //[SerializeField] private List<GameObject> unlockObjs = new List<GameObject>();
    //
    //private void Start()
    //{
    //
    //}
    //private void Update()
    //{
    //    step = SaveData.Current.barriers[data.index].step;
    //    if (data.step >= requirements.Count && !hasCompleted) Complete();
    //    string newtext = text.Replace("[req]", $"{requirements[data.step].name}");
    //    if(!hasCompleted) tm.text = newtext; else
    //    {
    //        tm.text = completedText;
    //        tm.color = Color.green;
    //    }
    //}
    //
    //public void UpdateStep()
    //{
    //    SaveData.Current.barriers[data.index].step += 1;
    //}
    //public void Complete()
    //{
    //    hasCompleted = true;
    //    if(data.step >= requirements.Count)
    //    {
    //        for(int i = 0; i < unlockObjs.Count; i++)
    //        {
    //            unlockObjs[i].SetActive(!unlockObjs[i].activeSelf);
    //        }
    //    }
    //}
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (hasCompleted) return;
    //    if(other.TryGetComponent<Element>(out Element e))
    //    {
    //        if (requirements[data.step] == e.element)
    //        {
    //            Destroy(other.gameObject);
    //            UpdateStep();
    //        }
    //    }
    //}
}


[System.Serializable]
public class BarrierData
{
    public int index;
    public int step;
}
