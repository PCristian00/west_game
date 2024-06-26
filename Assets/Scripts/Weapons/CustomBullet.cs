using UnityEngine;

public class CustomBullet : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;

    [Header("Stats")]
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;
    //Lifetime
    [Tooltip("1 come valore minimo")]
    public int maxCollisions = 1;
    [Tooltip("1 come valore minimo")]
    public float maxLifetime = 1;
    public bool explodeOnTouch = true;

    [Header("Damage")]
    public int explosionDamage;
    public float explosionRange;
    public float explosionForce;

    //[Header("Sound")]
    //public AudioClip explosionSound;
    //private AudioSource audioSource;

    int collisions;
    PhysicMaterial physics_mat;

    private void Start()
    {
        Setup();
        // audioSource = GetComponent<AudioSource>();
        //  Debug.Log("Creato " + name);
    }

    private void Update()
    {
        //When to explode:
        if (collisions > maxCollisions) Explode();

        //Count down lifetime
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0) Explode();
    }

    private void Explode()
    {
        // Debug.Log("EXPLOSIONE!!!11!! di " + gameObject.name);
        
        //Instantiate explosion

        if (explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);

        //if (explosionSound)
        //    audioSource.PlayOneShot(explosionSound);


        // gameObject.SetActive(false);
        // gameObject.transform.localScale = Vector3.zero;
        // AudioSource.PlayClipAtPoint(explosionSound, transform.position);

        //Check for enemies 
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);

        for (int i = 0; i < enemies.Length; i++)
        {
            //Get component of enemy and call Take Damage


            // Debug.Log("Il nemico si chiama "+enemies[i].name);
            if (enemies[i].GetComponent<Enemy>())
            {
                // Debug.Log(enemies[i].name + " sta per subire danno");
                enemies[i].GetComponent<Enemy>().TakeDamage(explosionDamage);
            }


            // else Debug.Log("AI not found");
            // enemies[i].GetComponent<EnemyAi>().TakeDamage(explosionDamage);

            //Add explosion force (if enemy has a rigidbody)
            if (enemies[i].GetComponent<Rigidbody>())
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);
        }

        //Add a little delay, just to make sure everything works fine
        Invoke(nameof(Delay), 0.05f);

        //  Debug.Log("Esploso " + name);
    }
    private void Delay()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Don't count collisions with other bullets
        if (collision.collider.CompareTag("Bullet")) return;

        //Count up collisions
        collisions++;

        //Explode if bullet hits an enemy directly and explodeOnTouch is activated
        if (collision.collider.CompareTag("Enemy") && explodeOnTouch) Explode();
    }

    private void Setup()
    {
        //Create a new Physic material
        physics_mat = new PhysicMaterial
        {
            bounciness = bounciness,
            frictionCombine = PhysicMaterialCombine.Minimum,
            bounceCombine = PhysicMaterialCombine.Maximum
        };
        //Assign material to collider
        GetComponent<SphereCollider>().material = physics_mat;

        //Set gravity
        rb.useGravity = useGravity;
    }

    /// Just to visualize the explosion range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
