using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour, IDamage
{
    const string RUN_TRIGGER = "Run";
    const string CROUCH_TRIGGER = "Crouch";
    const string SHOOT_TRIGGER = "Shoot";

    [SerializeField] private float startingHealth;
    [SerializeField] private float minTimeUnderCover;
    [SerializeField] private float maxTimeUnderCover;
    [SerializeField] private int minHit;
    [SerializeField] private int maxHit;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float damage;
    [Range(0,100)]
    [SerializeField] private float accuracy;

    [SerializeField] private Transform shootingPos;
    [SerializeField] private ParticleSystem bloodSpillEffect;    

    private bool isShooting;
    private int shotsTaken;
    private int maxShotstakenLimit;
    private Player player;
    private Transform coverOccupied;
    private Animator animator;
    private NavMeshAgent navAgent;

    private EnemyGun enemyGun;
    [SerializeField] private BulletMovement enemyBulletPrefab;
    [SerializeField] private Transform bulletSpawn;

    public event Action<EnemyAI> onEnemyDead;

    private bool hasReachedCover = false;
    private bool isInCoverRoutineRunning = false;

    private float _health;
    public float health
    {
        get
        {
            return _health;
        }

        set
        {
            _health = Mathf.Clamp(value, 0, startingHealth);
        }
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();

        navAgent.updateRotation = false;   
        navAgent.updateUpAxis = true;

        _health = startingHealth;
        enemyGun = GetComponent<EnemyGun>();
        animator.SetTrigger(RUN_TRIGGER);
    }

    //Sets initial cover points for Enemy 
    public void Init(Player player, Transform coverSpot)
    {
        this.player = player;

        Vector3 offset = new Vector3(
            UnityEngine.Random.Range(-0.5f, 0.5f),
            0,
            UnityEngine.Random.Range(-0.5f, 0.5f)
        );

        coverOccupied = coverSpot;
        GetCover();
    }

    private void GetCover()
    {
        navAgent.isStopped = false;
        navAgent.SetDestination(coverOccupied.position);
    }

    

    void Update()
    {
        if (!hasReachedCover && !GameManager.Instance.IsGameOver() && !GameManager.Instance.IsPaused())
        {
            if (!navAgent.pathPending &&
                navAgent.remainingDistance <= navAgent.stoppingDistance &&
                (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f))
            {
                hasReachedCover = true;

                if (GameManager.Instance.IsGameOver())
                {
                    navAgent.isStopped = true;
                    animator.enabled = false;
                    return;
                }
                navAgent.ResetPath();

                if (!isInCoverRoutineRunning)
                    StartCoroutine(InitShooting());
            }
        }

        if (isShooting && !GameManager.Instance.IsPaused())
        {
            RotateEnemyTowardsPlayer();
        }
    }


    private IEnumerator InitShooting()
    {
        HideBehindCover();
        yield return new WaitForSeconds(UnityEngine.Random.Range(minTimeUnderCover, maxTimeUnderCover));
        StartShooting();
    }

    private void HideBehindCover()
    {
        animator.SetTrigger(CROUCH_TRIGGER);
    }

    private void StartShooting()
    {
        //Debug.Log("Inside start shooting enemy");
        navAgent.isStopped = true;
        navAgent.ResetPath();
        isShooting = true;
        maxShotstakenLimit = UnityEngine.Random.Range(minHit, maxHit);
        shotsTaken =  0;
        animator.SetTrigger(SHOOT_TRIGGER);
    }

    //private void Shoot()
    //{
    //    bool hitPlayer = UnityEngine.Random.Range(0, 100) < accuracy;
    //    if(hitPlayer)
    //    {
    //        RaycastHit raycasthit;
    //        Vector3 direction = player.GetHeadPosition() - shootingPos.position;
    //        if(Physics.Raycast(shootingPos.position, direction, out raycasthit))
    //        {
    //            Player pl = raycasthit.collider.GetComponentInParent<Player>();
    //            if(pl)
    //            {
    //                pl.UpdateHealth(damage);
    //            }
    //        }
    //    }
    //    shotsTaken++;
    //    if(shotsTaken > maxShotstakenLimit)
    //    {
    //        StartCoroutine(InitShooting());
    //    }
    //}

    private void Shoot()
    {
        if (GameManager.Instance.IsGameOver() || GameManager.Instance.IsPaused())
            return;

        if (enemyBulletPrefab == null)
        {
            Debug.LogError("Enemy Bullet Prefab not assigned!");
            return;
        }

        enemyGun.Fire();

        BulletMovement bullet = Instantiate(
            enemyBulletPrefab,
            bulletSpawn.position,
            Quaternion.identity);

        // Calculate direction to player
        Vector3 direction = (player.GetHeadPosition() - bulletSpawn.position).normalized;
        direction += new Vector3(
            UnityEngine.Random.Range(-1f, 1f),
            UnityEngine.Random.Range(-1f, 1f),
            UnityEngine.Random.Range(-1f, 1f)
        ) * (1f - accuracy / 100f);

        direction.Normalize();

        bullet.transform.forward = direction;

        //Debug.Log("Bullet Spawned: " + bullet.name+" :: "+ GameManager.Instance.IsPaused());

        bullet.Init(enemyGun);
        bullet.Launch();

        shotsTaken++;

        if (shotsTaken > maxShotstakenLimit && !GameManager.Instance.IsGameOver() && !GameManager.Instance.IsPaused())
        {
            isShooting = false;
            StartCoroutine(InitShooting());
        }
    }

    private void RotateEnemyTowardsPlayer()
    {
        Vector3 direction = player.GetHeadPosition() - transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude < 0.001f)
            return; // prevent LookRotation error

        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed *Time.deltaTime);
        transform.rotation = rotation;
    }

    //Damage for body hit on enemy
    public void TakeDamage(Weapons weapon, Projectile projectile, Vector3 contactpoint)
    {
        _health -= weapon.GetDamage();
        Stats.Instance.AddScore();


        if (_health <= 0)
        {
            Destroy(gameObject);
            onEnemyDead?.Invoke(this);
        }

        ParticleSystem effect = Instantiate(bloodSpillEffect, contactpoint, Quaternion.LookRotation(weapon.transform.position - contactpoint));
        effect.Stop();
        effect.Play();
    }


    //Damage For Headshot
    public void TakeDamageDirect(float damageAmount, Vector3 hitPoint)
    {
        _health -= damageAmount;

        SpawnBlood(hitPoint);
        Stats.Instance.AddScore();

        if (_health <= 0)
        {
            Destroy(gameObject, 0.1f);
            onEnemyDead?.Invoke(this);
        }

        
    }    

    private void SpawnBlood(Vector3 hitPoint)
    {
        ParticleSystem effect = Instantiate(
            bloodSpillEffect,
            hitPoint,
            Quaternion.LookRotation(Vector3.up)
        );

        effect.transform.parent = null;
        effect.Play(true);

        Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constantMax);
    }


    //Headshot - fatal
    public void KillInstant(Vector3 hitPoint)
    {
        _health = 0;

        Stats.Instance.AddScore();

        Destroy(gameObject);
        onEnemyDead?.Invoke(this);

        ParticleSystem effect = Instantiate(
            bloodSpillEffect,
            hitPoint,
            Quaternion.LookRotation(hitPoint - transform.position)
        );

        effect.Play();
    }

    //Called during restart
    public void StopAllActions()
    {
        isShooting = false;
        StopAllCoroutines();
    }
}
