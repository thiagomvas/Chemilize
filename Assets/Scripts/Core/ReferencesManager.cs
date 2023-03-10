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


