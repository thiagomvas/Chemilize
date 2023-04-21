using System;
using System.Collections.Generic;
using UnityEngine;

public class Contractor : MonoBehaviour
{
    [SerializeField] private ReferencesSO refs;
    [SerializeField] private float contractDuration = 300;
    [SerializeField] private float minRewardMultiplier = 2f;
    [SerializeField] private float maxRewardMultiplier = 3.5f;
    public float reward;
    public ElementSO contractElement;
    public Action onNewContract;
    float rewardMultiplier;
    float nextContract = 0;
    bool contractDone;
    private void Update()
    {
        nextContract -= Time.deltaTime;

        if(nextContract <= 0 || contractDone)
        {
            nextContract = contractDuration;
            contractDone = false;
            NewContract();
        }

    }
    private void NewContract()
    {
        int x = UnityEngine.Random.Range(0, refs.elements.Count);
        rewardMultiplier = UnityEngine.Random.Range(minRewardMultiplier, maxRewardMultiplier);
        contractElement = refs.elements[x];
        reward = contractElement.sellValue * rewardMultiplier;
        onNewContract?.Invoke();
    }

    private void CompleteContract()
    {
        contractDone = true;
        SaveData.Current.currency += Mathf.FloorToInt(reward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Element>(out Element e))
        {
            if (e.element == contractElement)
            {
                Destroy(other.gameObject);
                CompleteContract();
            }
        }
    }
}
