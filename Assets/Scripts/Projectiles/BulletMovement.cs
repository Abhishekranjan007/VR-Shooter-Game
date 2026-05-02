using UnityEngine;

public class BulletMovement : Projectile
{
    [SerializeField] private float lifetime;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void Init(Weapons weapon)
    {
        base.Init(weapon);
        Destroy(gameObject, lifetime);
    }

    
    public override void Launch()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * 40f;        
    }


    private void OnTriggerEnter(Collider other)
    {
        EnemyHitbox hitbox = other.GetComponent<EnemyHitbox>();

        if (hitbox != null)
        {
            EnemyAI enemy = other.GetComponentInParent<EnemyAI>();

            if (enemy != null)
            {
                // Headshot check
                if (hitbox.hitType == HitType.Head)
                {                    
                    Stats.Instance.AddScore(50);
                    enemy.KillInstant(transform.position);
                    UIManager.Instance.ShowFloatingText(enemy.transform.position, 50);
                }
                else
                {
                    float damage = weapon.GetDamage();
                    enemy.TakeDamageDirect(damage, transform.position);
                }                

                Destroy(gameObject); // destroy AFTER logic
                return; // prevent fallback
            }
        }

        IDamage idamage = other.GetComponentInParent<IDamage>();

        if (idamage != null)
        {
            idamage.TakeDamage(weapon, this, transform.position);
        }

        Destroy(gameObject);        
    }


}
