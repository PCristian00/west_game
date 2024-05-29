using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public int health;

    public bool invincible = false;

    // public int wallet = 0;


    [Header("Sound")]

    public AudioClip hitSound;
    public AudioClip deathSound;

    public void Start()
    {
        instance = this;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet") && !invincible)
        {
            Destroy(other.gameObject);
            health--;
            if (health <= 0) Death();
            else AudioSource.PlayClipAtPoint(hitSound, gameObject.transform.position);
        }

        // FORSE RIMUOVERE: I power-up non sono raccolti ma equipaggiati

        if (other.CompareTag("Powerup"))
        {
            // Attiva ogni power-up contenuto nell'oggetto in collisione
            foreach (var powerup in other.GetComponents<IPowerup>())
            {
                // COOL-DOWN GENERICO: MODIFICARE IN QUALCHE MODO
                powerup.Activate(5f);
            }

            Debug.Log("Preso power-up " + other.name);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Coin"))
        {
            Coin coin = other.GetComponent<Coin>();
            Debug.Log("Presa moneta da " + coin.value);

            WalletManager.instance.wallet += coin.value;


            Destroy(other.gameObject);
        }
    }

    private void Death()
    {
        Debug.Log("SEI MORTO");

        AudioSource.PlayClipAtPoint(deathSound, gameObject.transform.position);
    }
}
