using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Crafter))]
public class CrafterSFX : MonoBehaviour
{
    [SerializeField] private Crafter crafter;
    [SerializeField] private AudioClip craftingSFX;
    [SerializeField] private AudioClip craftedSFX;


    private void OnEnable()
    {
        crafter.onCraftStart += CraftStartSFX;
        crafter.onCraftEnd += CraftEndSFX;
    }
    private void OnDisable()
    {
        crafter.onCraftStart -= CraftStartSFX;
        crafter.onCraftEnd -= CraftEndSFX;
    }

    private void CraftStartSFX() => SFXManager.Instance.PlayEffectForDuration(craftingSFX, transform.position, crafter.selectedRecipe.craftTime, false, crafter.transform);
    private void CraftEndSFX() => SFXManager.Instance.PlayEffect(craftedSFX, crafter.transform.position);
}   
