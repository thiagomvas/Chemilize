using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimate : MonoBehaviour, IInteractable
{
    [SerializeField] private int degrees;
    [SerializeField] private float duration;
    bool isOpen;
    bool rotate = false;
    Quaternion targetRot;
    private void Update()
    {
        if(rotate)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRot, Time.deltaTime / duration);
        }
    }

    public void Interact() => StartCoroutine(Rotate());
    public void SecondaryInteract() => Interact();
    public IEnumerator Rotate()
    {
        if(!rotate)
        {
            targetRot = this.transform.rotation;
            degrees *= -1;
            targetRot *= Quaternion.AngleAxis(degrees, Vector3.forward);
            rotate = true;
            yield return new WaitForSeconds(duration);
            rotate = false;
            isOpen = !isOpen;
        }

    }
}
