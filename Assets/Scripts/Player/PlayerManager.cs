using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public int health;


    [Header("Sound")]

    public AudioClip hitSound;
    public AudioClip deathSound;

    public void Start()
    {
        instance = this;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            health--;
            if (health <= 0) Death();
            else AudioSource.PlayClipAtPoint(hitSound, gameObject.transform.position);
        }

        if (other.CompareTag("Powerup"))
        {
            // Attiva ogni power-up contenuto nell'oggetto in collisione
            foreach (var powerup in other.GetComponents<IPowerup>())
            {
                powerup.Activate();
            }

            Debug.Log("Preso power-up " + other.name);
            Destroy(other.gameObject);
        }
    }

    private void Death()
    {
        Debug.Log("SEI MORTO");
        
        AudioSource.PlayClipAtPoint(deathSound, gameObject.transform.position);
    }
}
