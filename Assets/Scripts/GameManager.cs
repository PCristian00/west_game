using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("References")]
    public PlayerManager PlayerManager;

    public TextMeshProUGUI healthInfo;

    public GameObject enemy;

    public GameObject enemySpawn;

    private int enemyCount;
    private bool noEnemies = false;

    [Header("States")]
    public bool gameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        PlayerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthInfo != null)
            if (gameOver)
            {
                healthInfo.SetText("MORTO!");
            }
            else
            {
                healthInfo.SetText("Health: " + PlayerManager.health);
            }

        // if (PlayerManager.isDead) gameOver = true;

        if (!gameOver && noEnemies == false)
        {
           
            // ATTENZIONE: Le scatole di test (Cube) attualmente hanno il tag Enemy
           
            // Debug.Log("Nemici in gioco: " + enemyCount);

            if (enemyCount == 0)
                noEnemies = true;
            else return;
            // Debug.Log("Nessun nemico rimasto!");
            //SpawnEnemy();

            // QUESTA RIGA PER ORA NON VA BENE
            // VENGONO CONTINUAMENTE SPAWNATI NUOVI NEMICI (enemy count è zero durante l'attesa di spawn)
            Invoke(nameof(SpawnEnemy), 5);
        }
    }


    private void SpawnEnemy()
    {
        Instantiate(enemy, enemySpawn.transform.position, enemy.transform.rotation);
        noEnemies = false;
        enemyCount++;
    }

    public void EnemyKilled()
    {
        enemyCount--;
    }
}
