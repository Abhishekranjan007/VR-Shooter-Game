using System;
using UnityEngine;


public class PlayerFootsteps : MonoBehaviour
{
    private Player player;
    private int footCount = 0;

    private AudioSource audioSource;
    [SerializeField] private AudioClip clip;
    [SerializeField] private Transform head;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        player = GetComponent<Player>();
        audioSource = head.gameObject.GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        player.OnFootstep += PlayFootStep;
    }

    void OnDisable()
    {
        player.OnFootstep -= PlayFootStep;
    }

    private void PlayFootStep()
    {
        footCount++;
        Debug.Log("Walking now " + footCount);
        Debug.Log("AudioSource is playing: " + audioSource.isPlaying + " Clip: " + clip.name);
        //audioSource.spatialBlend = 1f;        // fully 3D
        audioSource.minDistance = 0.1f;       // distance at which volume is max
        audioSource.maxDistance = 20f;        // audible range
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.volume = 1f;
        audioSource.PlayOneShot(clip, 1f);
    }

    //private void PlayFootStep()
    //{
    //    Debug.Log("Trying to play sound");

    //    if (audioSource == null)
    //    {
    //        Debug.LogError("AudioSource is NULL!");
    //        return;
    //    }

    //    if (clip == null)
    //    {
    //        Debug.LogError("AudioClip is NULL!");
    //        return;
    //    }

    //    audioSource.PlayOneShot(clip, 1f);
    //}

    //private void PlayFootStep()
    //{
    //    Debug.Log("Trying to play sound");

    //    audioSource.spatialBlend = 0f; // force 2D
    //    audioSource.volume = 1f;

    //    audioSource.PlayOneShot(clip, 1f);
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
