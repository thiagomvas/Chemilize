using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject player;
    [SerializeField] private float delay;
    [SerializeField] private float realDelay;
    [SerializeField] private float scaleAmount;
    [SerializeField] private float scaleDelay;

    float nextSpawn;
    float nextScale;
    private void Update()
    {
        if (ReferencesManager.Instance.timer >= nextSpawn)
        {
            realDelay = Mathf.Clamp(delay - Mathf.FloorToInt(SaveData.Current.playtime / scaleDelay), 5f, 100f);
            nextSpawn = ReferencesManager.Instance.timer + realDelay;
            GameObject enemyObj = Instantiate(prefab, this.transform.position, Quaternion.identity);
            Enemy e = enemyObj.GetComponent<Enemy>();
            e.target = player.transform;
            e.health = e.health + Mathf.FloorToInt(SaveData.Current.playtime/ scaleDelay);
        }
        
    }

    private void Scale()
    {

    }
}
