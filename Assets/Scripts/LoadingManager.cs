using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance;

    // RIMUOVERE E CONSIDERARE loadingScreen come il gameObject stesso
    [Header("Reference")]
    public GameObject loadingScreen;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        loadingScreen.SetActive(false);
    }


    // FARE VARIANTE PER LOAD FROM NAME (Di loadScene e Loading)
    // Vedi se possibile con <T>, parametro di tipo casuale

    public void LoadSceneFromIndex(int index)
    {
        StartCoroutine(Loading(index));
        // return SceneManager.LoadSceneAsync(index);
    }

    public void LoadCurrent()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(Loading(index));
    }


    IEnumerator Loading(int index)
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(index);

        Slider bar = null;

        if (loadingScreen)
        {
            loadingScreen.SetActive(true);
            bar = loadingScreen.GetComponentInChildren<Slider>();
        }

        while (!load.isDone)
        {
            if (bar != null)
            {
                // bar.value = Mathf.Clamp01(load.progress / .9f);
                bar.value = load.progress;
            }
            yield return null;
        }
    }
}
