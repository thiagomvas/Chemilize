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
    Rigidbody targetRb;
    bool usesGravity;
    bool rotateToNormal = false;
    [SerializeField] private Element e;

    private void Start()
    {
        tool = GetComponent<BuildingTool>();
    }
    private void Update()
    {
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(this.transform.position + this.transform.forward * 0.5f, 0.01f, this.transform.forward, 5);
        foreach (RaycastHit hit in hits)
        {
            Collider[] cols = Physics.OverlapSphere(hit.point, 0.01f);
            if (cols[0] == null) continue;
            if (cols[0].transform.TryGetComponent<IInteractable>(out interactable)) break;
        }
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.TryGetComponent<IGrabbable>(out grabbable) && hit.transform.TryGetComponent<Element>(out Element e)) 
            { targetObj = hit.transform; break; }
            else if (hit.transform.TryGetComponent<IGrabbable>(out grabbable)) { targetObj = hit.transform; };
        }
        
        if(interactable != null && heldObj == null)
        {
            //Primary Interact
            if (controls.Player.Interact.WasPerformedThisFrame()             
                && !PlayerInputManager.Instance.BuildingToolOn) interactable.Interact();

            //Secondary Interact
            if (controls.Player.SecondaryInteract.WasPerformedThisFrame() 
                && !PlayerInputManager.Instance.BuildingToolOn) interactable.SecondaryInteract();

            if(controls.Player.Scroll.ReadValue<float>() != 0)
            {
                float x = controls.Player.Scroll.ReadValue<float>();
                Debug.Log((int)Mathf.Clamp(x, -1f, 1f));
            }
        }

        //Grab
        if (controls.Player.Grab.WasPressedThisFrame()) 
        {
            if (heldObj == null && grabbable != null)
                Grab(targetObj);
            else if (heldObj != null) Release();
        }

        //Move item
        if (heldObj != null)
        {
            if (controls.Player.Interact.WasPressedThisFrame()) rotateToNormal = true;
            if (controls.Player.Rotate.IsPressed()) Rotate(); 
            heldObj.transform.position = Vector3.Lerp(heldObj.transform.position, holdPoint.position, Time.deltaTime * 5);

            float speedMult = 5;
            if (rotateToNormal)
            {
                heldObj.rotation = Quaternion.Slerp(heldObj.rotation,
                                    Quaternion.Euler(0, 0, 0),
                                    Time.deltaTime * speedMult);
                targetRb.velocity = Vector3.zero;
            }
        }
    }
    public void UpdateSelectTile(Tile tile)
    {
        if (selectedTile != null) selectedTile.GetComponent<MeshFilter>().sharedMesh = null;
        selectedTile = tile;
        if (tile && PlayerInputManager.Instance.BuildingToolOn)
        {
            selectedTile.GetComponent<MeshFilter>().sharedMesh = wirecube;
            selectedTile.GetComponent<MeshRenderer>().materials = hologram;
            //tool.UpdateSelectTile(tile);
        }
    }
    public void Grab(Transform obj) 
    {
        heldObj = obj;
        targetRb = heldObj.GetComponent<Rigidbody>();
        usesGravity = targetRb.useGravity;
        targetRb.useGravity = false;
    }
    public void Release()
    {
        if(heldObj.TryGetComponent<PipeConector>(out PipeConector pc))
            {
                int index = pc.it.objectData.settingsIndex;
                if (pc.isOutput) SaveData.Current.pipePositions[index].output = pc.transform.position;
                else SaveData.Current.pipePositions[index].input = pc.transform.position;
            }
        if(heldObj.TryGetComponent<IObject>(out IObject io) && io.objectData.index >= 0)
        {
            
            io.objectData.position = heldObj.position;
            SaveData.Current.objects[io.objectData.index].position = heldObj.position;
            SaveData.Current.objects[io.objectData.index].rotation = heldObj.rotation;
        }
        targetRb.useGravity = usesGravity; 
        heldObj = null;
        targetRb = null;
        rotateToNormal = false;
    }

    public void Rotate()
    {
        float degreesPerFrame = 30;
        heldObj.transform.rotation *= Quaternion.AngleAxis(degreesPerFrame * Time.deltaTime, heldObj.transform.up);
    }
}
