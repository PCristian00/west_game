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

    //[Header("Reference")]
    //public GameObject loadingScreen;


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

        PauseGame(true);

        // loadingScreen.SetActive(false);

        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;


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
        if (PlayerManager.instance.health <= 0)
        {
            CurrentGameState = GameState.Lost;

            // AudioManager.BackgroundMusic[5] deve essere Canzone GameOver

            // CAUSA PROBLEMI: RISOLVERE
            AudioManager.instance.SetMusic(5);
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

    //// SUPERFLUE: RIMUOVERE E USARE DIRETTAMENTE LOADFROM INDEX
    //public void LoadDebugLevel()
    //{
    //    LoadSceneFromIndex(3);
    //}

    //// SUPERFLUE: RIMUOVERE E USARE DIRETTAMENTE LOADFROM INDEX
    //public void LoadMenu()
    //{
    //    LoadSceneFromIndex(0);
    //}

    //public void LoadSceneFromIndex(int index)
    //{
    //    StartCoroutine(Loading(index));
    //    // return SceneManager.LoadSceneAsync(index);
    //}

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

    //IEnumerator Loading(int index)
    //{
    //    AsyncOperation load = SceneManager.LoadSceneAsync(index);

    //    Slider bar = null;

    //    if (loadingScreen)
    //    {
    //        loadingScreen.SetActive(true);
    //        bar = loadingScreen.GetComponentInChildren<Slider>();
    //    }

    //    while (!load.isDone)
    //    {
    //        if (bar != null)
    //        {
    //            // bar.value = Mathf.Clamp01(load.progress / .9f);
    //            bar.value = load.progress;
    //        }
    //        yield return null;
    //    }
    //}
}
