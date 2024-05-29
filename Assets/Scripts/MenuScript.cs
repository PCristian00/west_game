using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public AudioSource sparo;
    
    public void PlayButton(){
        Invoke(nameof(Play), 1.5f);
        sparo.Play();
    }
    public void Play()
    {
        //incrementare la scena corrente di 1
        SceneManager.LoadScene(1);
    }

    public void ExitButton(){
        Invoke(nameof(Exit), 1.5f);
        sparo.Play();
    }

    public void Exit(){
        Debug.Log("Quitting game");
        Application.Quit();
    }
}