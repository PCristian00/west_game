using UnityEngine;

public class CountOnActivate : MonoBehaviour, IActivate
{
    public int count = 0;

    public void Activate()
    {
        count++;
        Debug.Log("Attivato " + count + " volte");
    }

    public int GetCount()
    {
        return count;
    }

}