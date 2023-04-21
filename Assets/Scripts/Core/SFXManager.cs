using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] backgroundMusics;
    [SerializeField] private AudioSource ambianceSource, effectSource;
    public static SFXManager Instance { get; private set; }
    [Header("3D Sound Settings")]
    [SerializeField] private float defaultMinDistance;
    [SerializeField] private float defaultMaxDistance;
    float nextSong;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this); else Instance = this;
    }

    private void Update()
    {
        //if (ReferencesManager.Instance.timer >= nextSong) PlayRandomSong();
    }


    public void PlayRandomSong()
    {
        int index = Random.Range(0, backgroundMusics.Length);
        ambianceSource.PlayOneShot(backgroundMusics[index]);
        nextSong = ReferencesManager.Instance.timer + backgroundMusics[index].length + Random.Range(200, 400);
    }
    /// <summary>
    /// Plays a sound effect at specified point
    /// </summary>
    /// <param name="clip">The audio clip to play</param>
    /// <param name="position">The position to play the sound effect at</param>
    /// <param name="varyPitch">Varies the sfx pitch randomly, leave false for normal audio</param>
    /// <param name="parent">The object that the audio source will follow</param>
    public void PlayEffect(AudioClip clip, Vector3 position, bool varyPitch = false, Transform parent = null)
    {
        var go = new GameObject().AddComponent<AudioSource>();
        go.transform.parent = parent;
        go.transform.position = position;

        go.spatialBlend = 1;
        go.minDistance = defaultMinDistance;
        go.maxDistance = defaultMaxDistance;

        if (varyPitch) go.pitch = Random.Range(0.9f, 1.1f);

        go.PlayOneShot(clip);

        Destroy(go.gameObject, clip.length);
    }

    /// <summary>
    /// Plays a clip at a specified point, looping for X seconds and stops when duration is over
    /// </summary>
    /// <param name="clip">The audio clip to play</param>
    /// <param name="position">The position to play the sound effect at</param>
    /// <param name="time">The duration to loop/play the clip for</param>
    /// <param name="varyPitch">Varies the sfx pitch randomly, leave false for normal audio</param>
    /// <param name="parent">The object that the audio source will follow</param>
    public void PlayEffectForDuration(AudioClip clip, Vector3 position, float time, bool varyPitch = false, Transform parent = null)
    {
        var go = new GameObject().AddComponent<AudioSource>();
        go.transform.parent = parent;
        go.transform.position = position;

        go.loop = true;
        go.spatialBlend = 1;
        go.minDistance = defaultMinDistance;
        go.maxDistance = defaultMaxDistance;

        if (varyPitch) go.pitch = Random.Range(0.9f, 1.1f);

        go.PlayOneShot(clip);

        Destroy(go.gameObject, time);
    }
}
