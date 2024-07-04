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


    [Header("SlowMo")]
    public bool slowMode = false;
    public float slowMultiplier = 0.5f;

    [Header("LevelInfo")]
    public int enemySpawnRate = 5;

    [Header("Enemies")]
    public GameObject enemy;
    public GameObject[] enemySpawn;
    private int enemyCount;
    private bool noEnemies = false;
    private int enemyKilled = 0;

    [Header("Objectives")]
    public int killGoal = 5;


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

    [HideInInspector] public UnityEvent<bool> OnInputActiveChanged;

    private int inputBlockedCounter = 0;

    public string LevelName => SceneManager.GetActiveScene().name;

    void Start()
    {
        instance = this;
        CurrentGameState = GameState.Running;

        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemy)
        {
            for (int i = 0; i < 3; i++)
            {
                Invoke(nameof(SpawnEnemy), enemySpawnRate);
            }
        }
    }

    void Update()
    {
        if (PlayerManager.instance.health <= 0)
        {
            CurrentGameState = GameState.Lost;
            // AudioManager.BackgroundMusic[5] deve essere Canzone GameOver
            AudioManager.instance.SetMusic(5);
            healthInfo.SetText("MORTO!");
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
        if (CurrentGameState != GameState.Lost && enemySpawn.Length != 0)
        {
            int index = UnityEngine.Random.Range(0, enemySpawn.Length);

            Instantiate(enemy, enemySpawn[index].transform.position, enemy.transform.rotation);

            Debug.Log($"{enemy.name} spawnato da {enemySpawn[index].name}");
            noEnemies = false;
            enemyCount++;
        }
        else return;
    }

    public void EnemyKilled()
    {
        enemyCount--;
        enemyKilled++;

        if (enemyKilled >= killGoal)
        {
            Debug.Log($"UCCISI {killGoal} NEMICI");

            CurrentGameState = GameState.Won;
            WalletManager.instance.wallet += 100;
            SaveManager.UpdateFloat(WalletManager.instance.saveKey, WalletManager.instance.wallet);

        }
        else if (enemy)
            Invoke(nameof(SpawnEnemy), enemySpawnRate);
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

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menù");
    }

    public void LoadSceneFromIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
        // Cambiare in caricamento scena menù principale
        SceneManager.LoadScene(LevelName);
    }

    public void PauseGame(bool unpause = false)
    {
        if (!unpause)
        {
            CurrentGameState = GameState.Waiting;
            Time.timeScale = 0f;
            Debug.Log("PAUSA");
        }
        else
        {
            CurrentGameState = GameState.Running;
            Time.timeScale = 1f;
        }
    }
}
