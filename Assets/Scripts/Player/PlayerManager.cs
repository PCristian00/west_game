using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int health; 

    [Header("Sound")]
 
    public AudioClip hitSound;
    public AudioClip deathSound;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            health--;
            if (health <= 0) Death();
            else AudioSource.PlayClipAtPoint(hitSound, gameObject.transform.position);
        }
    }

    private void Death()
    {
        Debug.Log("SEI MORTO");
        GameManager.Instance.gameOver = true;
        // TROVARE MODO PER USARE STATI E RIMUOVER GAMEOVER
        // GameManager.Instance.CurrentGameState = GameManager.GameState.Lost;
        // isDead = true;
        AudioSource.PlayClipAtPoint(deathSound, gameObject.transform.position);
    }
}
