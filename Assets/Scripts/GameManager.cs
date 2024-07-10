using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("SlowMo")]
    public bool slowMode = false;
    public float slowMultiplier = 0.5f;

    // [Header("LevelInfo")]
    public string LevelName => SceneManager.GetActiveScene().name;
    public int LevelIndex => SceneManager.GetActiveScene().buildIndex;

    public static string levelUnlockedKey = "last_level_unlocked";

    [Header("Enemies")]
    public GameObject enemy;
    public GameObject[] enemySpawn;
    public int enemySpawnRate = 5;
    public int spawnLimit = 5;
    private int enemyCount;
    private bool noEnemies = false;
    private int enemyKilled = 0;

    [Header("Objectives")]
    [Tooltip("Se maggiore di zero, una volta raggiunto l'obiettivo il giocatore vince")]
    public int killGoal;
    [Tooltip("Se maggiore di zero, viene attivato il timer")]
    public float timer;
    public bool killObjective = false;
    public bool timerObjective = false;
    [Tooltip("Stabilisce se dare un punteggio (in monete) per la salute rimasta al termine del livello")]
    public bool bonusPoints = false;

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

    void Start()
    {
        instance = this;

        if (killGoal > 0) killObjective = true;
        else killObjective = false;

        if (timer > 0) timerObjective = true;
        else timerObjective = false;

        PauseGame(true);

        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        // TEST: Il gioco parte con (spawnLimit) nemici
        if (enemy)
        {
            Invoke(nameof(SpawnEnemy), enemySpawnRate);

            //for (int i = 0; i < spawnLimit; i++)
            //{
            //    Invoke(nameof(SpawnEnemy), enemySpawnRate);
            //}
        }
    }

    void Update()
    {
        if (CurrentGameState != GameState.Running) return;

        if (timerObjective)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                // Se il giocatore è ancora vivo entro la fine del timer, vince
                WinLevel(bonusPoints);
            }
        }
    }

    public void WinLevel(bool bonus = false)
    {
        if (CurrentGameState != GameState.Running) return;

        Debug.Log("You win!!!");

        AudioManager.instance.SetMusic(6);

        CurrentGameState = GameState.Won;

        int bonusCoins = 0;

        if (bonus) bonusCoins = PlayerManager.instance.health / 5;

        Debug.Log("BONUS POINTS: " + bonusCoins);

        WalletManager.instance.wallet += 50 + bonusCoins;
        SaveManager.UpdateFloat(WalletManager.instance.saveKey, WalletManager.instance.wallet);

        // Controllo sblocco livello

        int levelUnlocked = SaveManager.LoadInt(levelUnlockedKey, 1);

        // Ad eccezione del Tutorial e dell'ultimo livello (attualmente index 4, Factory), si fa un controllo sullo sblocco del nuovo livello
        if (LevelIndex <= 3)
            if (LevelIndex > levelUnlocked)
            {
                SaveManager.UpdateInt(levelUnlockedKey, LevelIndex);
                Debug.Log("NUOVO LIVELLO SBLOCCATO");
            }
    }


    public void LoseLevel()
    {
        if (CurrentGameState != GameState.Running) return;

        Debug.Log("You lose...");

        AudioManager.instance.SetMusic(5);


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
        if (CurrentGameState != GameState.Lost && enemySpawn.Length != 0 && enemyCount < spawnLimit)
        {
            int index = UnityEngine.Random.Range(0, enemySpawn.Length);

            Instantiate(enemy, enemySpawn[index].transform.position, enemy.transform.rotation);

            Debug.Log($"{enemy.name} spawnato da {enemySpawn[index].name}");
            noEnemies = false;
            enemyCount++;

            Invoke(nameof(SpawnEnemy), enemySpawnRate);
        }
        else return;
    }

    public void EnemyKilled()
    {
        enemyCount--;
        enemyKilled++;

        if (enemyKilled >= killGoal && killObjective)
        {
            Debug.Log($"UCCISI {killGoal} NEMICI");

            WinLevel(bonusPoints);
        }
        else if (enemy)
            Invoke(nameof(SpawnEnemy), enemySpawnRate);
    }


    public string KillGoalInfo()
    {
        return $"{enemyKilled} / {killGoal} NEMICI SCONFITTI";
    }

    public string BriefingInfo()
    {
        string briefing = LevelName;

        if (killObjective) briefing += $"\nSconfiggi {killGoal} nemici";
        if (timerObjective) briefing += $"\nSopravvivi per {timer} secondi";

        return briefing;
    }


    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();

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
