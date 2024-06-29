using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearOnActivate : MonoBehaviour, IActivate
{
    public void Activate()
    {
        SaveManager.ResetAll();
        Debug.Log("CANCELLATI DATI");
    }
}
