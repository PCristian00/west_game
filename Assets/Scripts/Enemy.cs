﻿using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    [Header("Stats")]
    public float health;
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
    public float sightRange;
    public float attackRange;
    public bool playerInSightRange, playerInAttackRange;

    [Header("Sound")]
    public AudioClip attackSound;
    public AudioClip hitSound;
    public AudioClip deathSound;
    // AGGIUNGERE SUONI DI:
    // Movimento, Rilevamento giocatore (SOLO SE ANCORA PATTUGLIA)

    private void Awake()
    {
        player = GameObject.Find("PlayerObj").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
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
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
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
        Debug.Log("OUCH! " + gameObject.name + " ha subito " + damage + " danni!!!");
        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
        else AudioSource.PlayClipAtPoint(hitSound, gameObject.transform.position);
    }
    private void DestroyEnemy()
    {
        AudioSource.PlayClipAtPoint(deathSound, gameObject.transform.position);
        Debug.Log("NEMICO " + gameObject.name + " DISTRUTTO");
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
