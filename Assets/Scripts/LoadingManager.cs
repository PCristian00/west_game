using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance;

    [Header("Reference")]
    public GameObject loadingScreen;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        loadingScreen.SetActive(false);
    }

    public void LoadSceneFromIndex(int index)
    {
        StartCoroutine(Loading(index));        
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
                bar.value = load.progress;
            }
            yield return null;
        }
    }
}
