using UnityEngine;
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
    public float walkSpeed = 4f;
    public bool canPatrol = true;
    public bool canChase = true;
    [Tooltip("Se impostato a true, il nemico non spara ma raggiunge solo l'obiettivo con tag 'Capture'")]
    public bool walkOnly = false;
    public float shootForce = 32f;
    public float upwardForce = 8f;


    [Header("Patroling")]
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

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
        // Se il nemico può solo camminare, il suo obiettivo è raggiungere la capturePoint e non il giocatore
        if (!walkOnly)
            player = GameObject.Find("PlayerObj").transform;
        else player = GameObject.FindGameObjectWithTag("Capture").transform;

        if (canPatrol || canChase)
            agent = GetComponent<NavMeshAgent>();
        // Debug.Log("Speed = " + agent.walkSpeed);
        // else Debug.Log("Nemico immobile");
    }

    private void Update()
    {

        // La velocita' del nemico dipende da GameManager
        if (agent != null)
            if (GameManager.instance.slowMode)
            {
                agent.speed = walkSpeed * GameManager.instance.slowMultiplier;
            }
            else agent.speed = walkSpeed;

        //Check for sight and attack range

        // Se il sightRange è impostato a 0, viene percepito come infinito.
        // Il nemico sa sempre dove è il giocatore.

        if (sightRange == 0)
            playerInSightRange = true;
        else playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInSightRange) icon.SetActive(true);
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
        if (walkOnly) agent.SetDestination(transform.position);
        else
        {
            //Make sure enemy doesn't move
            //if (canChase || canPatrol)
            //    agent.SetDestination(transform.position);

            //// DEBUG: Imposta l'icona ad Attiva in Attack per i nemici che non vanno in Chase
            //if (!canChase)
            //    icon.SetActive(true);

            transform.LookAt(player);

            if (!alreadyAttacked && canAttack)
            {
                ///Attack code here
                Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
                AudioSource.PlayClipAtPoint(attackSound, gameObject.transform.position);

                float multiplier;
                // La velocita' del proiettile e' influenzata da GameManager
                if (GameManager.instance.slowMode)
                    multiplier = GameManager.instance.slowMultiplier;
                else multiplier = 1f;

                rb.AddForce(shootForce * multiplier * transform.forward, ForceMode.Impulse);
                rb.AddForce(upwardForce * transform.up, ForceMode.Impulse);

                ///End of attack code

                alreadyAttacked = true;
                // La velocita' di attacco viene condizionata dal GameManager
                Invoke(nameof(ResetAttack), timeBetweenAttacks / multiplier);
            }
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("OUCH! " + gameObject.name + " ha subito " + damage + " danni!!! (vita rimasta = " + health + ")");

        if (health <= 0 && canAttack)
        {
            canAttack = false;
            AudioSource.PlayClipAtPoint(deathSound, gameObject.transform.position);
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            //rb.AddExplosionForce(3, transform.position, 3);



            Invoke(nameof(DestroyEnemy), 0.5f);
        }
        else AudioSource.PlayClipAtPoint(hitSound, gameObject.transform.position);
    }
    private void DestroyEnemy()
    {
        Destroy(icon);
        // Debug.Log("NEMICO " + gameObject.name + " DISTRUTTO");
        GameManager.instance.EnemyKilled();
        Destroy(gameObject);


        GameObject coin = WalletManager.instance.DropCoin();
        // Rilascio casuale della moneta

        if (canPatrol || canChase)
        {

            int dropChance = Random.Range(0, 5);
            Debug.Log("Drop = " + dropChance);

            if (dropChance >= 3)
            {
                Instantiate(coin, transform.position, coin.transform.rotation);
                Debug.Log("Moneta caduta - " + coin.name);
            }
        }
        // Se il nemico non può spostarsi, carica direttamente i soldi senza rilasciare monete
        else WalletManager.instance.wallet += 25;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
