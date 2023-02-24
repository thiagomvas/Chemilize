using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder;
public class BuildingTool : MonoBehaviour
{
    PlayerInteract pi;
    [SerializeField] private BuildingMenu bm;
    private Tile selectedTile;
    PlayerControls controls;
    public int id;
    [SerializeField] private Material[] hologram;
    [SerializeField] private Transform point;
    public StructureSO structure;
    public Mesh wirecube;
    public GameObject hammer;
    // Update is called once per frame
    private void Awake()
    {
        controls = new PlayerControls();
    }
    private void Start()
    {
        pi = GetComponent<PlayerInteract>();
    }
    void Update()
    {           
        if (!PlayerInputManager.Instance.BuildingToolOn) return;

        if (selectedTile != null && controls.Player.Interact.WasPressedThisFrame()) Build();
        if (selectedTile != null && controls.Player.Rotate.WasPerformedThisFrame()) Rotate();
        if (selectedTile != null && controls.Player.SecondaryInteract.WasPerformedThisFrame()) Deconstruct();

    }

    public void UpdateID(int i)
    {
        id = i;
        structure = id >= 0 ? ReferencesManager.Instance.structures[id] : null;
    }
    public void UpdateSelectTile(Tile tile)
    {
        if (tile == null) return;
        selectedTile = tile;
    }

    private void Build()
    {
        if (selectedTile.data.hasBuilding || SaveData.Current.currency < structure.cost) return;
        GameObject tile = Instantiate(structure.prefab, selectedTile.transform.position, Quaternion.identity);
        selectedTile.data.hasBuilding = true;
        selectedTile.building = tile.transform;

        //TileData data = SaveData.Current.grids[selectedTile.data.gridIndex].tiles[selectedTile.data.index];
        Component[] comps = tile.GetComponents<Component>();
        for (int i = 0; i < comps.Length; i++)
        {
            switch (comps[i])
            {
                case ItemGenerator ig:
                    //ig.data = data;
                    break;
                case StorageTank st:
                    //st.data = data;
                    break;
                case Crafter c:
                    //c.data = data;
                    break;
                case Turret t:
                    //t.data = data;
                    break;
            }
        }

        SaveData.Current.grids[selectedTile.data.gridIndex].tiles[selectedTile.data.index].hasBuilding = true;
        SaveData.Current.grids[selectedTile.data.gridIndex].tiles[selectedTile.data.index].structureId = id;
        SaveData.Current.currency -= structure.cost;
    }

    private void Rotate()
    {
        if (!selectedTile.data.hasBuilding) return;
        int rot = SaveData.Current.grids[selectedTile.data.gridIndex].tiles[selectedTile.data.index].rotation;
        int rotlocal = selectedTile.data.rotation;
        SaveData.Current.grids[selectedTile.data.gridIndex].tiles[selectedTile.data.index].rotation = rot + 1 >= 4 ? 0 : rot + 1;
        selectedTile.data.rotation = rotlocal + 1 >= 4 ? 0 : rotlocal + 1;
        selectedTile.building.Rotate(Vector3.up, 90);

        if(selectedTile.building.TryGetComponent<Conveyor>(out Conveyor c)) c.CheckSide(true);

    }

    private void Deconstruct()
    {
        if (!selectedTile.data.hasBuilding) return;

        Destroy(selectedTile.building.gameObject);
   

        selectedTile.building = null;

        selectedTile.data.hasBuilding = false;

        SaveData.Current.grids[selectedTile.data.gridIndex].tiles[selectedTile.data.index].hasBuilding = false;

        foreach(Collider col in Physics.OverlapBox(point.position, Vector3.one * 1.5f))
        {
            if(col.TryGetComponent<Conveyor>(out Conveyor con))
            {
                con.StartCoroutine(con.CheckSides(0.05f, true));
            }
        }


    }
    private void OnEnable() => controls.Enable();
}
