using UnityEngine;

public class MenuOnActivate : MonoBehaviour, IActivate
{
    public GameObject menu;

    public void Activate()
    {
        Debug.Log("Attivazione menu");
        menu.SetActive(true);
    }
}
