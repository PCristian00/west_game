﻿using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    private Rigidbody rb;


    [Header("Stats")]
    public float health;
    public bool canPatrol = true;
    public bool canChase = true;


    // ESEMPI DI STATS IMPLEMENTABILI
    // public float damage;
    // public bool canJump, canDodge;

    //Patroling (FORSE DA RIMUOVERE - Deve sapere sempre dove è il giocatore)
    [Header("Patroling")]
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    [Header("Attacking")]
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    [Header("States")]
    [Tooltip("Se impostato a zero, il nemico trova sempre il giocatore.")]
    public float sightRange;
    public float attackRange;
    public bool playerInSightRange, playerInAttackRange;
    private bool canAttack = true;

    [Header("Sound")]
    public AudioClip attackSound;
    public AudioClip hitSound;
    public AudioClip deathSound;
    // AGGIUNGERE SUONI DI:
    // Movimento, Rilevamento giocatore (SOLO SE ANCORA PATTUGLIA)

    [Header("Graphics")]
    public GameObject attackEffect;
    public GameObject hitEffect;
    public GameObject deathEffect;
    public GameObject icon;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        icon.SetActive(false);
    }

    private void Awake()
    {
        player = GameObject.Find("PlayerObj").transform;
        if (canPatrol || canChase)
            agent = GetComponent<NavMeshAgent>();
        else Debug.Log("Nemico immobile");
    }

    private void Update()
    {
        //Check for sight and attack range

        // Se il sightRange è impostato a 0, viene percepito come infinito.
        // Il nemico sa sempre dove è il giocatore.
        if (sightRange == 0)
            playerInSightRange = true;
        else playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if(playerInSightRange) icon.SetActive(true);
        else icon.SetActive(false);

        if (!playerInSightRange && !playerInAttackRange & canPatrol) Patroling();
        if (playerInSightRange && !playerInAttackRange & canChase) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
       // icon.SetActive(false);
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
       // icon.SetActive(true);
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        if (canChase || canPatrol)
            agent.SetDestination(transform.position);

        //// DEBUG: Imposta l'icona ad Attiva in Attack per i nemici che non vanno in Chase
        //if (!canChase)
        //    icon.SetActive(true);

        transform.LookAt(player);

        if (!alreadyAttacked && canAttack)
        {
            ///Attack code here
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            AudioSource.PlayClipAtPoint(attackSound, gameObject.transform.position);
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        // Debug.Log("OUCH! " + gameObject.name + " ha subito " + damage + " danni!!!");
        if (health <= 0 && canAttack)
        {
            canAttack = false;
            AudioSource.PlayClipAtPoint(deathSound, gameObject.transform.position);
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            //rb.AddExplosionForce(3, transform.position, 3);

            Destroy(icon);

            Invoke(nameof(DestroyEnemy), 0.5f);
        }
        else AudioSource.PlayClipAtPoint(hitSound, gameObject.transform.position);
    }
    private void DestroyEnemy()
    {

        Debug.Log("NEMICO " + gameObject.name + " DISTRUTTO");
        GameManager.instance.EnemyKilled();
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
