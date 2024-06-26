using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource musicSource;
    public AudioSource SFXSource;

    [Header("Audio Clip")]
    // MIGLIORARE SISTEMA PER BG MUSIC
    public AudioClip background_1;
    public AudioClip background_2;
    public AudioClip pulsante_seleziona;
    public AudioClip pulsante_conferma;
    public AudioClip pulsante_annulla;

    public static AudioManager instance;

    private void Awake()
    {
       // if(AudioVolume.instance) AudioVolume.instance.LoadVolume();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        SetMusicForCurrentScene();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetMusicForCurrentScene();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void SetMusicForCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        AudioClip newClip;

        // Controlla il nome della scena e imposta la traccia audio di conseguenza
        // MIGLIORARE SISTEMA
        if (sceneName == "Saloon")
        {
            newClip = background_2;
        }
        else
        {
            newClip = background_1;
        }

        // Cambia la traccia solo se è diversa da quella attuale
        if (musicSource.clip != newClip)
        {
            StartCoroutine(FadeOutAndChangeMusic(newClip));
        }
    }
    private IEnumerator FadeOutAndChangeMusic(AudioClip newClip)
    {
        float fadeOutTime = 0.25f;
        float fadeInTime = 0.25f;
        float startVolume = musicSource.volume;

        // Fade out
        for (float t = 0; t < fadeOutTime; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeOutTime);
            yield return null;
        }

        musicSource.volume = 0;
        musicSource.clip = newClip;
        musicSource.loop = true;
        musicSource.Play();

        // Fade in
        for (float t = 0; t < fadeInTime; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0, startVolume, t / fadeInTime);
            yield return null;
        }

        musicSource.volume = startVolume;
    }
}