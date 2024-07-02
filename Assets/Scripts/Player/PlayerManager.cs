using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public int health;
    public int maxHealth = 50;

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



    // public int wallet = 0;


    [Header("Sound")]

    public AudioClip hitSound;
    public AudioClip deathSound;
    public AudioClip PUPickup;
    public AudioClip CoinPickup;
   // public AudioClip[] footsteps;
    private AudioSource audioSource;

    public void Start()
    {
        instance = this;

        LoadUpgrades();
        
        // TEORICAMENTE INUTILE
        if (maxHealth == 0) maxHealth = 50;

        health = maxHealth;

        audioSource = GetComponent<AudioSource>();

        // DebugUpgrade();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet") && !invincible)
        {
            EnemyBullet bullet = other.GetComponent<EnemyBullet>();
            Destroy(other.gameObject);
            health -= bullet.damage;
            if (health <= 0) Death();
            else audioSource.PlayOneShot(hitSound);
        }

        else
        {            
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

                WalletManager.instance.wallet += coin.value * coinMultiplier;

                audioSource.PlayOneShot(CoinPickup);
                Destroy(other.gameObject);
            }


        }
    }

    private void Death()
    {
        Debug.Log("SEI MORTO");

        audioSource.PlayOneShot(deathSound);
    }

    public void LoadUpgrades()
    {
       // Debug.Log("Caricamento upgrades");
        maxHealth = SaveManager.LoadInt(healthKey, maxHealth);
        // Debug.Log(maxHealth);
        damageMultiplier = SaveManager.LoadFloat(damageKey);
        coinMultiplier = SaveManager.LoadFloat(coinKey);

        GunPopup.LoadGunStates();
        ActiveSkillPopup.LoadSkillStates();

       // Debug.Log("CARICATI: " + DebugUpgrade());
    }

    public string DebugUpgrade()
    {
        return $"HLT: {maxHealth} / DMG: {damageMultiplier} / COIN: {coinMultiplier}";
    }
}
