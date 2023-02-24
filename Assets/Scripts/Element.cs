using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour, IGrabbable
{
    [SerializeField] private Renderer liquid;
    public ElementSO element;

    private void Start()
    {
        liquid.material.color = element.cpkHexColor;
        if (element.glow) liquid.material.SetColor("_EmissionColor", element.cpkHexColor * Mathf.GammaToLinearSpace(2f));
        else liquid.material.SetColor("_EmissionColor", Color.black);

        if (element.metallic) liquid.material.SetFloat("_Metallic", 1f);
        Invoke("DestroyThis", 300);
    }

    private void DestroyThis() => Destroy(this.gameObject);
}
