using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] backgroundMusics;
    [SerializeField] private AudioSource ambianceSource, effectSource;
    public static SFXManager Instance { get; private set; }
    float nextSong;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this); else Instance = this;
    }

    private void Update()
    {
        //if (ReferencesManager.Instance.timer >= nextSong) PlayRandomSong();
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

    public void PlayRandomSong()
    {
        int index = Random.Range(0, backgroundMusics.Length);
        ambianceSource.PlayOneShot(backgroundMusics[index]);
        nextSong = ReferencesManager.Instance.timer + backgroundMusics[index].length + Random.Range(200, 400);
    }
}
