using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;

public class Rifle : Weapons
{
    [SerializeField] private float firerate;
    private Projectile projectile;

    private WaitForSeconds wait;

    protected override void Awake()
    {
        base.Awake();
        projectile = GetComponentInChildren<Projectile>();
    }

    void Start()
    {
        wait = new WaitForSeconds(1/firerate);
        projectile.Init(this);
    }

    protected override void StartShoot(ActivateEventArgs args)
    {
        if (GameManager.Instance.IsGameOver()) return;

        base.StartShoot(args);
        StartCoroutine(ShootStart());
    }

    private IEnumerator ShootStart()
    {
        while(true && !GameManager.Instance.IsGameOver() && !GameManager.Instance.IsPaused())
        {
            Shoot();
            yield return wait;
        }
    }

    protected override void Shoot()
    {
        base.Shoot();
        projectile.Launch();
    }

    protected override void StopShooting(DeactivateEventArgs args)
    {
        base.StopShooting(args);     
        StopAllCoroutines();
    }
}
