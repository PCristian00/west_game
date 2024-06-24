using UnityEngine;

public class MenuOnActivate : MonoBehaviour, IActivate
{
    public GameObject menu;

    public void Activate()
    {
        Debug.Log("Attivazione menu");
        if (menu)
            menu.SetActive(true);
        else Debug.Log("Nessun menu associato per " + gameObject.name);
    }
}
