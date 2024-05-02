using System;
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

    public int enemySpawnRate = 5;

    private int enemyCount;
    private bool noEnemies = false;

    // RIMUOVERE GAMEOVER E USARE GAMESTATE
    [Header("States")]
    public bool gameOver = false;

    // INSERIRE STATI CON ENUM

    public enum GameState
    {
        Waiting,
        Running,
        Won,
        Lost
    }

    private GameState _currentGameState;
    public GameState CurrentGameState
    {
        get => _currentGameState;
        protected set
        {
            if (_currentGameState == value) return;

            _currentGameState = value;
            //if(OnCurrentGameStateChanged!=null)
            //    OnCurrentGameStateChanged(_currentGameState);
            OnCurrentGameStateChanged?.Invoke(_currentGameState);
            //  OnInputActiveChanged?.Invoke(IsInputActive);
            //OnUnityCurrentGameStateChanged?.Invoke(_currentGameState);
        }
    }

    //public UnityEvent<GameState> OnUnityCurrentGameStateChanged;

    public event Action<GameState> OnCurrentGameStateChanged;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        PlayerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

        // ATTENZIONE: Le scatole di test (Cube) attualmente hanno il tag Enemy
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthInfo != null)
            if (gameOver)
            {
                // VEDERE COME IMPOSTARE MEGLIO
                CurrentGameState = GameState.Lost;
                healthInfo.SetText("MORTO!");
            }
            else
            {
                healthInfo.SetText("Health: " + PlayerManager.health);
            }

        if (!gameOver && noEnemies == false)
        {

            // Debug.Log("Nemici in gioco: " + enemyCount);

            if (enemyCount == 0)
                noEnemies = true;
            else return;

            Invoke(nameof(SpawnEnemy), enemySpawnRate);
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
