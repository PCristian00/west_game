using UnityEngine;

public class LoadOnExamine : MonoBehaviour, IExamine
{
    public void Examine()
    {
        Debug.Log("ANDIAMO AL SALOON");
        GameManager.instance.LoadSceneFromIndex(1);
        //SceneManager.LoadScene("Saloon");
    }
}
