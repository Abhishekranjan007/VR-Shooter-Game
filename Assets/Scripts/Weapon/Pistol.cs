using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Pistol : Weapons
{
    [SerializeField] public Projectile bulletPrefab;

    protected override void StartShoot(ActivateEventArgs args)
    {
        if (GameManager.Instance.IsGameOver()) return;

        base.StartShoot(args);
        Shoot();
    }

    protected override void Shoot()
    {
        base.Shoot();
        Projectile bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.Init(this);
        bullet.Launch();
    }

    protected override void StopShooting(DeactivateEventArgs args)
    {
        base.StopShooting(args);        
    }
}



