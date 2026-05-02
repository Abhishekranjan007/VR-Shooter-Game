
using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(XRGrabInteractable))]
public class Weapons : MonoBehaviour
{
    [SerializeField] private float recoilForce;
    [SerializeField] private float damage;
    private float playerDamage;
    [SerializeField] private float shootingForce;
    [SerializeField] protected Transform bulletSpawn;

    [Header("Audio")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private float shootVolume = 1f;
    [SerializeField] private float minDistance = 5f;
    [SerializeField] private float maxDistance = 40f;

    private Rigidbody rigidbody;
    private XRGrabInteractable weapon;
    private AudioSource audioSource;

    public event Action OnShoot;

    protected virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        weapon = GetComponent<XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        SetupAudio();
        InteractableWeaponsEvents();
    }

    private void InteractableWeaponsEvents()
    {
        weapon.selectEntered.AddListener(PickWeapon);
        weapon.selectExited.AddListener(DropWeapon);
        weapon.activated.AddListener(StartShoot);
        weapon.deactivated.AddListener(StopShooting);
    }

    private void PickWeapon(SelectEnterEventArgs args)
    {        
        var mesh = args.interactorObject.transform.GetComponent<MeshManipulate>();

        if (mesh != null)
            mesh.Hide();
    }

    private void DropWeapon(SelectExitEventArgs args)
    {
       var mesh =  args.interactorObject.transform.GetComponent<MeshManipulate>();

       if (mesh != null)
            mesh.Show();
    }


    private void SetupAudio()
    {
        audioSource.spatialBlend = 1f; // 3D sound
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        audioSource.playOnAwake = false;
        audioSource.volume = PlayerPrefs.GetFloat(Constants.VolumeKey);
    }

    protected virtual void StartShoot(ActivateEventArgs args)
    {

    }

    protected virtual void StopShooting(DeactivateEventArgs args)
    {

    }

    public void Fire()
    {
        Shoot();
    }

    protected virtual void Shoot()
    {
        if (GameManager.Instance.IsGameOver() || GameManager.Instance.IsPaused())
            return;

        Recoil();
        

        if (shootClip != null)
        {
            audioSource.volume = PlayerPrefs.GetFloat(Constants.VolumeKey);
            audioSource.PlayOneShot(shootClip, shootVolume);
        }
        OnShoot?.Invoke();
    }

    private void Recoil()
    {
        rigidbody.AddRelativeForce(Vector3.back * recoilForce, ForceMode.Impulse);
    }
    

    public float GetShootingForce()
    {
        return shootingForce;
    }

    public float GetDamage()
    {
        return damage;
    }

    public void SetPlayerDamage(float dmg)
    {
        this.playerDamage = dmg;
    }

    public float GetPlayerDamage()
    {
        
        return this.playerDamage;
        
    }
}
