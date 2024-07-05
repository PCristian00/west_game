using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource musicSource;
    public AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip[] backgroundMusic;
    public AudioClip[] buttonSounds;

    public static AudioManager instance;

    private void Awake()
    {
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
        //string sceneName = currentScene.name;

        AudioClip newClip;


        newClip = backgroundMusic[currentScene.buildIndex];

        // Cambia la traccia solo se è diversa da quella attuale
        //if (musicSource.clip != newClip)
        //{
        StartCoroutine(FadeOutAndChangeMusic(newClip));
        //}
    }

    public void SetMusic(int index)
    {
        AudioClip newClip = backgroundMusic[index];

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