using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOnExamine : MonoBehaviour, IExamine
{
    public void Examine()
    {
        Debug.Log("ANDIAMO AL SALOON");
        SceneManager.LoadScene("Saloon");
    }
}
