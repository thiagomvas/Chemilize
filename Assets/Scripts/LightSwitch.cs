using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] private Light[] lights;
    [SerializeField] private AudioClip sfx;

    bool isOn;
    public void Interact()
    {
        foreach(Light light in lights)
        {
            light.enabled = isOn;
        }
        SFXManager.Instance.PlayEffect(sfx, transform.position, true);
        isOn = !isOn;
    }

    public void SecondaryInteract() => Interact();
}
