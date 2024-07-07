using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [Header("Stats")]
    public int health;
    public int maxHealth = 100;
    public bool invincible = false;
    public float damageMultiplier = 1f;
    public float coinMultiplier = 1f;

    [Header("Upgrades")]
    public int healthLevel = 0;
    public int damageLevel = 0;
    public int coinLevel = 0;
    public readonly string healthKey = "health_level";
    public readonly string damageKey = "dmg_level";
    public readonly string coinKey = "coin_level";

    [Header("Sound")]
    public AudioClip hitSound;
    public AudioClip deathSound;
    public AudioClip PUPickup;
    public AudioClip CoinPickup;
    [SerializeField] private AudioSource audioSource;

    public void Start()
    {
        LoadUpgrades();

        health = maxHealth;

        instance = this;
    }


    //public void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Bullet") && !invincible)
    //    {
    //        EnemyBullet bullet = collision.gameObject.GetComponent<EnemyBullet>();
    //        Destroy(collision.gameObject);
    //        TakeDamage(bullet.damage);
    //    }
    //}

    public void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Bullet") && !invincible)
        //{
        //    EnemyBullet bullet = other.GetComponent<EnemyBullet>();
        //    Destroy(other.gameObject);
        //    TakeDamage(bullet.damage);
        //}

        //else
        //{
        if (other.CompareTag("Powerup"))
        {
            // Attiva ogni power-up contenuto nell'oggetto in collisione
            foreach (var powerup in other.GetComponents<IPowerup>())
            {
                // COOL-DOWN GENERICO: MODIFICARE IN QUALCHE MODO
                powerup.Activate(5f);
            }

            audioSource.PlayOneShot(PUPickup);

            Debug.Log("Preso power-up " + other.name);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Coin"))
        {
            Coin coin = other.GetComponent<Coin>();
            Debug.Log("Presa moneta da " + coin.value * coinMultiplier);

            WalletManager.instance.wallet += (int)(coin.value * coinMultiplier);

            audioSource.PlayOneShot(CoinPickup);
            Destroy(other.gameObject);
        }
        //}
    }

    public void TakeDamage(int damage)
    {
        if (!invincible)
        {
            health -= damage;
            if (health <= 0) Death();
            else audioSource.PlayOneShot(hitSound);
        }
    }

    private void Death()
    {
        Debug.Log("SEI MORTO");
        GameManager.instance.LoseLevel();
        audioSource.PlayOneShot(deathSound);
    }

    public void LoadUpgrades()
    {
        // Debug.Log("Caricamento upgrades");
        maxHealth = SaveManager.LoadInt(healthKey, maxHealth);
        damageMultiplier = SaveManager.LoadFloat(damageKey);
        coinMultiplier = SaveManager.LoadFloat(coinKey);

        GunPopup.LoadGunStates();
        ActiveSkillPopup.LoadSkillStates();
    }

    public string DebugUpgrade()
    {
        return $"HLT: {maxHealth} / DMG: {damageMultiplier} / COIN: {coinMultiplier}";
    }
}
