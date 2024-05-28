using UnityEngine;

public class HealthPU : MonoBehaviour, IPowerup
{  
    public void Activate(float wait)
    {
        PlayerManager.instance.health += 10;
        Debug.Log("Vita aumentata di 10");
    }

    public void Deactivate(float wait)
    {
        throw new System.NotImplementedException();
    }
}
