using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource sparo;

    public void PlayButton()
    {
        Invoke(nameof(Play), 0.5f);
        sparo.Play();
    }
    public void Play()
    {
        //incrementare la scena corrente di 1
        // SceneManager.LoadScene(1);
        LoadingManager.instance.LoadSceneFromIndex(1);
    }

    public void ExitButton()
    {
        Invoke(nameof(Exit), 1.5f);
        sparo.Play();
    }

    public void Exit()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}