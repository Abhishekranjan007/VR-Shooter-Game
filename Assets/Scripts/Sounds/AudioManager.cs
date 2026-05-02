using System;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private Player player;
    private int footCount = 0;

    private AudioSource audioSource;
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioClip pistolShot;
    [SerializeField] private AudioClip rifleShot;
    [SerializeField] private AudioClip pickUp;
    [SerializeField] private AudioClip healthPickUp;
    [SerializeField] private Transform head;

    [SerializeField] private Weapons pistol;
    [SerializeField] private Weapons rifle;

    [SerializeField] private Slider vol_slider;

    
    void Awake()
    {
        player = GetComponent<Player>();
        audioSource = head.gameObject.GetComponent<AudioSource>();
        PlayerPrefs.SetFloat(Constants.VolumeKey, audioSource.volume);
        vol_slider.value = audioSource.volume;
        vol_slider.onValueChanged.AddListener(VolumeControl);
    }

    void OnEnable()
    {
        player.OnFootstep += PlayFootStep;        
        player.OnHealthPickup += HeathPickupSound;        
    }

    void OnDisable()
    {
        player.OnFootstep -= PlayFootStep;        
        player.OnHealthPickup -= HeathPickupSound;
    }

     void VolumeControl(float value)
    {
        audioSource.volume = value;
        PlayerPrefs.SetFloat(Constants.VolumeKey, value);
    }

    

    private void PlayFootStep()
    {
        footCount++;        
        audioSource.minDistance = 0.1f;       // distance at which volume is max
        audioSource.maxDistance = 20f;        // audible range
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.volume = 1f;
        audioSource.PlayOneShot(clip, 1f);
    }

    public void PlayRifleShot()
    {
        audioSource.PlayOneShot(rifleShot);
    }

    public void PlayPistolShot()
    {
        audioSource.PlayOneShot(pistolShot);
    }

    public void PickupSound()
    {
        audioSource.PlayOneShot(pickUp);
    }

    public void HeathPickupSound()
    {
        audioSource.PlayOneShot(healthPickUp);
    }
    
    
}
