using UnityEngine;

public class Raycaster : Projectile
{
    public override void Launch()
    {
        base.Launch();
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit))
        {
            print(hit.collider.gameObject.name);
            IDamage[] damageTakers = hit.collider.GetComponentsInParent<IDamage>();
            foreach (var taker in damageTakers)
            {
                taker.TakeDamage(weapon, this, hit.point);
            }
        }
    }
}
