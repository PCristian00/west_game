using UnityEngine;

public class CapturePoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();

            Destroy(other.gameObject);
            Instantiate(enemy.deathEffect, transform.position, Quaternion.identity);

            GameManager.instance.EnemyKilled();
            PlayerManager.instance.TakeDamage(enemy.captureDamage);
            
            // Debug.Log(enemy.name + " ha fatto " + enemy.captureDamage + " punti danno");
        }
        else Debug.Log("NON ERA UN ENEMY");
    }
}
