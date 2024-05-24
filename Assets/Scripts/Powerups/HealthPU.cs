using UnityEngine;

public class HealthPU : MonoBehaviour, IPowerup
{  
    public void Activate()
    {
        PlayerManager.instance.health += 10;
        Debug.Log("Vita aumentata di 10");
    }

    public void Deactivate()
    {
        throw new System.NotImplementedException();
    }
}
