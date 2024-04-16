using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public PlayerManager PlayerManager;

    public TextMeshProUGUI healthInfo;

    public GameObject enemy;

    public GameObject enemySpawn;

    private int enemyCount;

    [Header("States")]
    public bool gameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        PlayerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (healthInfo != null)
            if (PlayerManager.isDead)
            {
                healthInfo.SetText("MORTO!");
            }
            else
            {
                healthInfo.SetText("Health: " + PlayerManager.health);
            }

        if (PlayerManager.isDead) gameOver = true;

        if (!gameOver)
        {
            // ATTENZIONE: Le scatole di test (Cube) attualmente hanno il tag Enemy
            enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            // Debug.Log("Nemici in gioco: " + enemyCount);

            if (enemyCount == 0)
            {
                // Debug.Log("Nessun nemico rimasto!");
                Instantiate(enemy, enemySpawn.transform.position, enemy.transform.rotation);
            }
        }
    }
}
