using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float maxLifetime = 10f;
    public int damage = 10;
    public GameObject explosion;

    // Update is called once per frame
    void Update()
    {
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0)
        {
            // Debug.Log("Tempo EnemyBullet scaduto");
            Explode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            Explode();
        }
        else
        {
            collision.gameObject.GetComponent<PlayerManager>().TakeDamage(damage);
            Explode();
        }
    }

    private void Explode()
    {
        if (explosion)
            Instantiate(explosion, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
