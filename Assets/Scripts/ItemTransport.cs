using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTransport : MonoBehaviour
{

    [SerializeField] private Transform input, output, pipe;
    private List<Transform> contents = new List<Transform>();
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
}
