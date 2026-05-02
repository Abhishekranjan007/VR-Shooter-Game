using UnityEngine;

public class HitEffect : MonoBehaviour, IDamage
{
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    public void TakeDamage(Weapons weapon, Projectile projectile, Vector3 contactpoint)
    {       
        rb.AddForce(projectile.transform.forward *weapon. GetShootingForce(), ForceMode.Impulse);
    }


    
}
