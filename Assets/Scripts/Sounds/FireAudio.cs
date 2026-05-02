using UnityEngine;

public class FireAudio : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip pistolShot;
    [SerializeField] private AudioClip rifleShot;
    [SerializeField] private Transform head;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource= head.gameObject.GetComponent<AudioSource>();
    }

    public void PlayRifleShot()
    {
        audioSource.PlayOneShot(rifleShot);
    }

    public void PlayPistolShot()
    {
        audioSource.PlayOneShot(pistolShot);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
