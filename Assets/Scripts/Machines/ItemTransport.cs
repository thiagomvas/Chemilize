using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTransport : MonoBehaviour, IObject
{

    public bool isAutomatic { get { return auto; } set { auto = value; } }
    bool auto;
    public Vector3 point { get { return Vector3.zero; } set { pos = value; } }
    Vector3 pos;
    public ObjectData objectData
    {
        get { return data; }
        set { data = value; }
    }
    ObjectData data;
    [SerializeField] public Transform input, output, pipe;
    [SerializeField] private float enableColDistance;
    private List<Transform> contents = new List<Transform>();

    private void Start()
    {

    }

    private void Update()
    {
        Vector3 dir = (output.position - input.position).normalized;
        float dist = Vector3.Distance(input.position, output.position);
        pipe.LookAt(output.position);
        pipe.localScale = new Vector3(0.25f, 0.25f, dist);
        pipe.position = input.position + dir * dist / 2;
        foreach(Transform t in contents)
        {
            if (t == null) contents.Remove(t);
            t.position = Vector3.Lerp(t.position, output.position + dir * 2, Time.deltaTime);
            if(Vector3.Distance(t.position, output.position) < enableColDistance) t.GetComponent<Collider>().enabled = true;
            
        }
    }

    public void EnterPipe(Transform obj)
    {
        if(obj.TryGetComponent<Element>(out Element e))
        {
            contents.Add(obj);
            obj.GetComponent<Rigidbody>().isKinematic = true;
        }        
    }

    public void ExitPipe(Transform obj)
    {
        if (obj.TryGetComponent<Element>(out Element e))
        {
            contents.Remove(obj);
            obj.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void UpdatePositions(Vector3 i, Vector3 o)
    {
        input.position = i;
        output.position = o;
        SaveData.Current.pipePositions[data.settingsIndex].input = i;
        SaveData.Current.pipePositions[data.settingsIndex].output = o;
    }
}
