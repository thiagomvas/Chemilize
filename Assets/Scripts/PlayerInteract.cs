using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    PlayerControls controls;
    private void Awake() => controls = new PlayerControls();
    private void OnEnable() => controls.Enable();

    [SerializeField] private Transform holdPoint;
    public Tile selectedTile;
    public Mesh wirecube;
    [SerializeField] private Material[] hologram;
    private Transform heldObj;
    BuildingTool tool;
    IInteractable interactable;
    IGrabbable grabbable;
    Transform targetObj;
    bool usesGravity;
    [SerializeField] private Element e;

    private void Start()
    {
        tool = GetComponent<BuildingTool>();
    }
    private void Update()
    {
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(this.transform.position + this.transform.forward * 0.5f, 0.1f, this.transform.forward, 5);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.TryGetComponent<IInteractable>(out interactable)) break;
        }
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.TryGetComponent<IGrabbable>(out grabbable)) { targetObj = hit.transform; break; }
        }
        
        if(interactable != null && heldObj == null)
        {
            //Primary Interact
            if (controls.Player.Interact.WasPressedThisFrame()             
                && !PlayerInputManager.Instance.BuildingToolOn) interactable.Interact();

            //Secondary Interact
            if (controls.Player.SecondaryInteract.WasPressedThisFrame() 
                && !PlayerInputManager.Instance.BuildingToolOn) interactable.SecondaryInteract();
        }

        //Grab
        if (controls.Player.Grab.WasPressedThisFrame()) 
        {
            if (heldObj == null && grabbable != null)
                Grab(targetObj);
            else if (heldObj != null) Release();
        }
       

        //Move item
        if (heldObj != null) heldObj.transform.position = Vector3.Lerp(heldObj.transform.position, holdPoint.position, Time.deltaTime * 5);
    }
    public void UpdateSelectTile(Tile tile)
    {
        if (selectedTile != null) selectedTile.GetComponent<MeshFilter>().sharedMesh = null;
        selectedTile = tile;
        if (tile && PlayerInputManager.Instance.BuildingToolOn)
        {
            selectedTile.GetComponent<MeshFilter>().sharedMesh = wirecube;
            selectedTile.GetComponent<MeshRenderer>().materials = hologram;
            tool.UpdateSelectTile(tile);
        }
    }
    public void Grab(Transform obj) 
    {
        heldObj = obj;
        Rigidbody rb = heldObj.GetComponent<Rigidbody>();
        usesGravity = rb.useGravity;
        rb.useGravity = false;
    }
    public void Release()
    {
        if(heldObj.TryGetComponent<IObject>(out IObject io))
        {
            io.objectData.position = heldObj.position;
            SaveData.Current.objects[io.objectData.index].position = heldObj.position;
            SaveData.Current.objects[io.objectData.index].rotation = heldObj.rotation;
        }
        heldObj.GetComponent<Rigidbody>().useGravity = usesGravity; 
        heldObj = null;
    }
}
