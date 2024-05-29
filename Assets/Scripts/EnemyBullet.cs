using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float maxLifetime = 10f;

    // Update is called once per frame
    void Update()
    {
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0)
        {
            Debug.Log("Tempo EnemyBullet scaduto");
            Destroy(gameObject);
        }
    }
}
