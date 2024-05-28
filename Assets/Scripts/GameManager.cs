using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("References")]

    public TextMeshProUGUI healthInfo;
    public GameObject enemy;
    public GameObject enemySpawn;

    public bool slowMode = false;
    public float slowMultiplier = 0.5f;

    public int enemySpawnRate = 5;

    private int enemyCount;
    private bool noEnemies = false;


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

            OnCurrentGameStateChanged?.Invoke(_currentGameState);
        }
    }

    public event Action<GameState> OnCurrentGameStateChanged;

    void Start()
    {
        instance = this;

        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    void Update()
    {
        if (healthInfo != null)
            if (PlayerManager.instance.health <= 0)
            {
                CurrentGameState = GameState.Lost;
                healthInfo.SetText("MORTO!");
            }
            else
            {
                healthInfo.SetText("Health: " + PlayerManager.instance.health);
            }

        if (CurrentGameState != GameState.Lost && noEnemies == false)
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

    public void StartGame()
    {
        CurrentGameState = GameState.Running;

        Debug.Log(LoadoutManager.instance.CurrentWeapon.name);

        // Time.timeScale = 1;
    }
}
