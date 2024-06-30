using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
            OnInputActiveChanged?.Invoke(IsInputActive);
        }
    }

    public event Action<GameState> OnCurrentGameStateChanged;

    public bool IsInputActive
    {
        get => CurrentGameState == GameState.Running && inputBlockedCounter <= 0;
    }

    public UnityEvent<bool> OnInputActiveChanged;

    private int inputBlockedCounter = 0;

    public string LevelName => SceneManager.GetActiveScene().name;

    void Start()
    {
        instance = this;
        CurrentGameState = GameState.Running;

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

        if (CurrentGameState != GameState.Lost && noEnemies == false && enemySpawn)
        {
            // Debug.Log("Nemici in gioco: " + enemyCount);
            if (enemyCount == 0)
                noEnemies = true;
            else return;

            if(enemy)
            Invoke(nameof(SpawnEnemy), enemySpawnRate);
        }
    }

    public void RequestInputBlock()
    {
        // Debug.Log("Request Input Block");
        inputBlockedCounter++;
        OnInputActiveChanged?.Invoke(IsInputActive);
    }

    public void RequestInputUnblock()
    {
        // Debug.Log("Request Input Unblock");
        inputBlockedCounter--;
        if (inputBlockedCounter < 0) inputBlockedCounter = 0;
        OnInputActiveChanged?.Invoke(IsInputActive);
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

    public void LoadDebugLevel()
    {
        SceneManager.LoadScene("DebugScene");
    }

    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
        // Cambiare in caricamento scena men� principale
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
