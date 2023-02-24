using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource ambianceSource, effectSource;
    public static SFXManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this); else Instance = this;
    }

    public void PlayEffect(AudioClip clip, bool varyPitch)
    {
        if (varyPitch)
        {
            effectSource.pitch = Random.Range(0.9f, 1.1f);
        }
        else effectSource.pitch = 1;
        effectSource.PlayOneShot(clip);
    }
}
