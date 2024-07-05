using UnityEngine;

public class LoadOnExamine : MonoBehaviour, IExamine
{
    public void Examine()
    {
        Debug.Log("ANDIAMO AL SALOON");
        LoadingManager.instance.LoadSceneFromIndex(1);
        //SceneManager.LoadScene("Saloon");
    }
}
