using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Transform player;
    public Transform attackPoint;
    [SerializeField] private SkinnedMeshRenderer enemyMesh;

    public LayerMask whatIsGround, whatIsPlayer;
    private Rigidbody rb;

    [Header("Stats")]
    public float health;
    public float walkSpeed = 4f;
    public bool canPatrol = true;
    public bool canChase = true;
    [Tooltip("Se impostato a true, il nemico non spara ma raggiunge solo l'obiettivo con tag 'Capture'")]
    public bool walkOnly = false;
    [Tooltip("Danno causato dal nemico al raggiungimento del Capture Point")]
    public int captureDamage = 1;
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
    private AudioSource audioSource;

    [Header("Graphics")]
    public GameObject attackEffect;
    public GameObject hitEffect;
    public GameObject deathEffect;
    public GameObject icon;
    public Color flashColor = Color.red;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        if (icon)
            icon.SetActive(false);

        // Se non è impostato un attackPoint personalizzato, carica quello del gameObject
        if (!attackPoint)
            attackPoint = transform;
    }

    private void Awake()
    {
        enemyMesh = GetComponentInChildren<SkinnedMeshRenderer>();

        // Se il nemico può solo camminare, il suo obiettivo è raggiungere la capturePoint e non il giocatore
        if (!walkOnly)
            player = GameObject.Find("PlayerObj").transform;
        else player = GameObject.FindGameObjectWithTag("Capture").transform;

        if (canPatrol || canChase)
            agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (GameManager.instance.CurrentGameState == GameManager.GameState.Running)
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

            if (icon)
            {
                if (playerInSightRange) icon.SetActive(true);
                else icon.SetActive(false);
            }

            if (!playerInSightRange && !playerInAttackRange & canPatrol) Patroling();
            if (playerInSightRange && !playerInAttackRange & canChase) ChasePlayer();
            if (playerInAttackRange && playerInSightRange) AttackPlayer();
        }
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
        if (walkOnly) agent.SetDestination(transform.position);
        else
        {
            transform.LookAt(player);

            if (!alreadyAttacked && canAttack)
            {
                ///Attack code here

                Rigidbody rb = Instantiate(projectile, attackPoint.position, Quaternion.identity).GetComponent<Rigidbody>();

                if (attackSound)
                    audioSource.PlayOneShot(attackSound);

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

    public void TakeDamage(float damage)
    {
        health -= damage;

        Debug.Log("OUCH! " + gameObject.name + " ha subito " + damage + " danni!!! (vita rimasta = " + health + ")");

        if (enemyMesh)
            FlashOnHit();

        if (health <= 0 && canAttack)
        {
            canAttack = false;

            audioSource.PlayOneShot(deathSound);

            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
        else if (audioSource && hitSound) audioSource.PlayOneShot(hitSound);
    }

    private void FlashOnHit()
    {
        foreach (var mat in enemyMesh.materials)
        {
            mat.color += flashColor;
        }

        Invoke(nameof(ResetColor), 0.5f);
    }

    private void ResetColor()
    {
        foreach (var mat in enemyMesh.materials)
        {
            mat.color -= flashColor;
        }
    }

    private void DestroyEnemy()
    {
        if(icon)
        Destroy(icon);
        // Debug.Log("NEMICO " + gameObject.name + " DISTRUTTO");
        GameManager.instance.EnemyKilled();
        Destroy(gameObject);


        GameObject coin = WalletManager.instance.DropCoin();
        // Rilascio casuale della moneta

        if ((canPatrol || canChase) && !walkOnly)
        {

            int dropChance = Random.Range(0, 5);
            //  Debug.Log("Drop = " + dropChance);

            if (dropChance >= 1)
            {
                Vector3 dropPosition = attackPoint.position;
                if (dropPosition.y >= 3) dropPosition.y = player.position.y;

                Instantiate(coin, dropPosition, coin.transform.rotation);
                // Debug.Log("Moneta caduta - " + coin.name);
            }
        }
        // Se il nemico non può spostarsi o attaccare, carica direttamente i soldi senza rilasciare monete
        else WalletManager.instance.wallet += 25 * PlayerManager.instance.coinMultiplier;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
