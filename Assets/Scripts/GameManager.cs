using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
    public float timer;
    public bool killObjective = false;
    public bool timerObjective = false;

    [Header("References")]
    public TextMeshProUGUI enemyInfo;

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
    public int LevelIndex => SceneManager.GetActiveScene().buildIndex;

    void Start()
    {
        instance = this;

        if (killGoal > 0) killObjective = true;
        else killObjective = false;

        if (timer > 0) timerObjective = true;
        else timerObjective = false;

        PauseGame(true);

        // loadingScreen.SetActive(false);

        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemyInfo)
            enemyInfo.text = $"{enemyKilled} / {killGoal} NEMICI SCONFITTI";


        // TEST: Il gioco parte con 3 nemici
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
        if (CurrentGameState != GameState.Running) return;

        if (timerObjective)
        {
            timer -= Time.deltaTime;
            //TimeSpan ts = TimeSpan.FromSeconds(_timer);
            //_timerText.text =  ts.ToString( @"mm\:ss" );
            if (timer <= 0f)
            {
                // Debug.Log("");
                LoseLevel();
            }
        }
    }

    public void WinLevel()
    {
        if (CurrentGameState != GameState.Running) return;
        //GameRunning = false;
        Debug.Log("You win!!!");
        //_winPanel.SetActive(true);
        CurrentGameState = GameState.Won;
    }


    public void LoseLevel()
    {
        if (CurrentGameState != GameState.Running) return;
        //GameRunning = false;
        Debug.Log("You lose...");

        AudioManager.instance.SetMusic(5);
        //_losePanel.SetActive(true);

        CurrentGameState = GameState.Lost;
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

        if (enemyInfo)
            enemyInfo.text = $"{enemyKilled} / {killGoal} NEMICI SCONFITTI";

        if (enemyKilled >= killGoal)
        {
            Debug.Log($"UCCISI {killGoal} NEMICI");

            WinLevel();
            WalletManager.instance.wallet += 100;
            SaveManager.UpdateFloat(WalletManager.instance.saveKey, WalletManager.instance.wallet);

        }
        else if (enemy)
            Invoke(nameof(SpawnEnemy), enemySpawnRate);
    }

    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
        // Cambiare in caricamento scena menù principale
        LoadingManager.instance.LoadSceneFromIndex(LevelIndex);
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
