using UnityEngine;

public interface IDamage 
{
    void TakeDamage(Weapons weapon, Projectile projectile, Vector3 contactpoint);
}
