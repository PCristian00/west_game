using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float maxLifetime = 10f;
    public int damage = 1;

    // Update is called once per frame
    void Update()
    {
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0)
        {
           // Debug.Log("Tempo EnemyBullet scaduto");
            Destroy(gameObject);
        }
    }
}
