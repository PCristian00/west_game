using UnityEngine;

public class CapturePoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Raggiunto da " + other.name);
            // Debug.Log("Colliso " + collision.gameObject.name);
            Destroy(other.gameObject);
            PlayerManager.instance.health += 100;
        }
        else Debug.Log("NON ERA UN ENEMY");
    }
}
