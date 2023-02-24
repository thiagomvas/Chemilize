using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeConector : MonoBehaviour, IGrabbable, IInteractable
{
    ItemTransport it;
    [SerializeField] private bool isOutput;
    IObject obj;
    Vector3 lockedPos;

    private void Start()
    {
        it = GetComponentInParent<ItemTransport>();
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
                    this.transform.position = obj.point;
                    obj.isAutomatic = true;
                }
                else
                {
                    Vector3 dir = (col.transform.position - this.transform.position).normalized;
                    RaycastHit hit;
                    Physics.Raycast(this.transform.position, dir, out hit);
                    this.transform.position = hit.point;
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
        if (!isOutput) it.EnterPipe(other.transform);
        else it.ExitPipe(other.transform);
    }
}
