using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeConector : MonoBehaviour, IGrabbable, IInteractable
{
    public ItemTransport it { get; private set; }
    public bool isOutput;
    IObject obj;
    Vector3 lockedPos;

    private void Start()
    {
        it = GetComponentInParent<ItemTransport>();
        Interact();
    }

    private void Update()
    {
        if (obj == null) return;
        if (this.transform.position != lockedPos) obj.isAutomatic = false;
    }
    public void Interact()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 1f);
        foreach(Collider col in cols)
        {
            if(col.TryGetComponent<IObject>(out obj))
            {
                if (!isOutput)
                {
                    float distanceFromMachine = .5f;
                    Vector3 dir = (col.transform.position - this.transform.position).normalized;
                    RaycastHit hit;
                    Physics.Raycast(this.transform.position, dir, out hit);
                    Vector3 newPos = hit.point - dir * distanceFromMachine;
                    this.transform.position = newPos;
                    obj.isAutomatic = true;
                    obj.point = this.transform.position;
                }
                else
                {
                    Vector3 dir = (col.transform.position - this.transform.position).normalized;
                    RaycastHit hit;
                    Physics.Raycast(this.transform.position, dir, out hit);
                    this.transform.position = hit.transform.position;
                }
                GetComponent<Rigidbody>().isKinematic = true;
                lockedPos = this.transform.position;
                break;
            }
        }
    }
    public void SecondaryInteract()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.TryGetComponent<Element>(out Element e)) StartCoroutine(PipeEnterExit(other));
        
    }

    private IEnumerator PipeEnterExit(Collider other)
    {
        if (!isOutput)
        { 
            it.EnterPipe(other.transform);
            other.enabled = false;
        }
        else it.ExitPipe(other.transform);        
        yield return null;
    }
}
