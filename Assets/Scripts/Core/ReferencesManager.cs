using System.Windows;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
public class ReferencesManager : MonoBehaviour
{
    public static ReferencesManager Instance { get; private set; }
    [Header("Player References")]
    public MouseLook mouseLook;
    public PlayerMovement movement;
    public PlayerInteract interaction;

    [Header("Other")]
    public List<ElementSO> elements = new List<ElementSO>();
    public List<RecipeSO> recipes = new List<RecipeSO>();
    [SerializeField] private static List<ElementSO> _elements = new List<ElementSO>();

    public List<StructureSO> structures = new List<StructureSO>();
    public List<Storage> placedStorages = new List<Storage>();
    [Header("Settings")]
    [SerializeField] private bool fetchAllElements;
    

    public float timer;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this); else Instance = this;
    }

    [MenuItem("MyMenu/Search and Update IDs")]
    private static void Search()
    {
        List<ElementSO> unsorted = SOUtils.FindAllOfType<ElementSO>("t:ElementSO", "Assets/Elements");
        Debug.Log($"Found {unsorted.Count} elements");
        List<ElementSO> sorted = new List<ElementSO>(unsorted.Count);

        sorted = unsorted.OrderBy(x => x.id).ToList();

        for (int i = 0; i < sorted.Count; i++)
        {
            sorted[i].id = i;
        }

        ReferencesManager.Instance.elements = sorted;

        List<StructureSO> unsorted2 = SOUtils.FindAllOfType<StructureSO>("t:StructureSO", "Assets/Elements");
        Debug.Log($"Found {unsorted2.Count} elements");
        List<StructureSO> sorted2 = new List<StructureSO>(unsorted2.Count);

        sorted2 = unsorted2.OrderBy(x => x.id).ToList();

        for (int i = 0; i < sorted2.Count; i++)
        {
            sorted2[i].id = i;
        }

        ReferencesManager.Instance.structures = sorted2;
    }

    private void Start()
    {
        this.enabled = true;
        elements.OrderBy(x => x.id).ToList();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        SaveData.Current.playtime += Time.deltaTime;
    }

}

public static class SOUtils
{

    public static List<T> FindAllOfType<T>(string filter, string folder = "Assets")
        where T : ScriptableObject
    {
        return AssetDatabase.FindAssets(filter, new[] { folder })
            .Select(guid => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid)))
            .ToList();
    }
}
