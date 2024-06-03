using UnityEngine;

public class CapturePoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            Debug.Log("Raggiunto da " + other.name);
            // Debug.Log("Colliso " + collision.gameObject.name);
            Destroy(other.gameObject);
            PlayerManager.instance.health -= enemy.captureDamage;
            Debug.Log(enemy.name + " ha fatto " + enemy.captureDamage + " punti danno");
        }
        else Debug.Log("NON ERA UN ENEMY");
    }
}
