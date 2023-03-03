using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class GridSystem : MonoBehaviour
{
    //
    ////[SerializeField] private int width;
    //
    ////[SerializeField] private int height;
    //[SerializeField] private Tile tilePrefab;
    //[SerializeField] private GameObject prefab;
    //[SerializeField] private List<GridData> grids = new List<GridData>();
    //public Dictionary<Vector2, Tile> tiles = new Dictionary<Vector2, Tile>();
    ////public Dictionary<Vector2, Tile> tiles = new Dictionary<Vector2, Tile>();
    //
    //private void Start()
    //{
    //    GenerateGrid();
    //}
    //
    //private void Update()
    //{
    //    if(Keyboard.current.enterKey.wasPressedThisFrame)
    //    {
    //        ClearGridData();
    //    }
    //
    //}
    //public void ClearGridData()
    //{
    //    //SaveData.Current.tiles.Clear();
    //    //Vector3 p = this.transform.position;
    //    //int index = 0;
    //    //for (int x = 0; x < width; x++)
    //    //{
    //    //    for (int z = 0; z < height; z++)
    //    //    {
    //    //        Destroy(tiles[new Vector2(x, z)].gameObject);
    //    //        Tile newTile = Instantiate(tilePrefab, new Vector3(x + p.x, p.y, z + p.z), Quaternion.identity);
    //    //        newTile.name = $"Tile ({x}, {z})";
    //    //        index++;
    //    //        tiles[new Vector2(x, z)] = newTile;
    //    //        SaveData.Current.tiles.Add(newTile.data);
    //    //        if (index > (width - 1) * (height - 1)) Debug.Log(index);
    //    //    }
    //    //}
    //    //
    //    //
    //    //SerializationManager.SaveGame("save", SaveData.Current);
    //}
    //
    //public void GenerateGrid()
    //{
    //    SerializationManager.LoadGame("save.save");
    //    //Loop to go through each grid
    //    int gridAmount = SaveData.Current.grids.Count == grids.Count ? SaveData.Current.grids.Count : grids.Count;
    //    for (int g = 0; g < grids.Count; g++)
    //    {
    //        if (g == SaveData.Current.grids.Count) { SaveData.Current.grids.Add(new GridData()); Debug.Log("Added grid to save file!"); }// if grid does not exist on save file, create new one
    //
    //        Vector3 p = grids[g].position;
    //        int index = 0;
    //
    //        for (int x = 0; x < grids[g].width; x++) // Loop through a grid's X axis
    //        {
    //            for(int z = 0; z < grids[g].height; z++) // Loop through a grid's Z axis
    //            {
    //                Tile newTile = Instantiate(tilePrefab, new Vector3(x + p.x, p.y, z + p.z), Quaternion.identity); // create tile
    //
    //                newTile.name = $"Tile ({x}, {z})";
    //                newTile.data.index = index;
    //                newTile.data.gridIndex = g;
    //
    //                if (index == SaveData.Current.grids[g].tiles.Count) SaveData.Current.grids[g].tiles.Add(newTile.data); // if tile doesnt exist, add to save file
    //               
    //                tiles[new Vector2(x, z)] = newTile; //add to dictionary for reference
    //                tiles[new Vector2(x, z)].data = SaveData.Current.grids[g].tiles[index]; //add data to dictionary for reference
    //
    //                //Load Structures from save file
    //                if (SaveData.Current.grids[g].tiles[index].hasBuilding)
    //                {
    //                    int id = SaveData.Current.grids[g].tiles[index].structureId;
    //                    GameObject b = Instantiate(ReferencesManager.Instance.structures[id].prefab, tiles[new Vector2(x, z)].transform.position, Quaternion.identity);
    //                    b.transform.Rotate(Vector3.up, 90 * SaveData.Current.grids[g].tiles[index].rotation); // Load rotation
    //                    //TileData data = SaveData.Current.grids[g].tiles[index];
    //                    //
    //                    //
    //                    ////Switch to load data
    //                    //Component[] comps = b.GetComponents<Component>();
    //                    //for(int i = 0; i < comps.Length; i++)
    //                    //{
    //                    //    switch(comps[i])
    //                    //    {
    //                    //        case ItemGenerator ig:
    //                    //            ig.data = data;
    //                    //            break;
    //                    //        case StorageTank st:
    //                    //            st.data = data;
    //                    //            break;
    //                    //        case Crafter c:
    //                    //            c.data = data;
    //                    //            break;
    //                    //        case Turret t:
    //                    //            t.data = data;
    //                    //            break;
    //                    //    }
    //                    //}
    //                    
    //                    
    //                    newTile.building = b.transform;
    //                }
    //                    index++;
    //            }
    //        }
    //
    //    }
    //}     
    //
    //
    //void OnDrawGizmosSelected()
    //{
    //    for(int g = 0; g < grids.Count; g++)
    //    { 
    //        Vector3 p = grids[g].position;
    //        for (int x = 0; x < grids[g].width; x++)
    //        {
    //            for (int z = 0; z < grids[g].height; z++)
    //            {
    //
    //                Vector3 pos = new Vector3(x + p.x, p.y, z + p.z);
    //                Gizmos.DrawWireCube(pos, Vector3.one);
    //                Gizmos.DrawCube(pos, Vector3.one/4);
    //            }
    //        }
    //
    //    }
    //
    //}
}

[System.Serializable]
public class GridData
{
    public int index;
    public int width;
    public int height;
    public Vector3 position;
    public List<TileData> tiles = new List<TileData>();

}

