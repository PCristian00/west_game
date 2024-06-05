using UnityEngine;

public class HealthPU : MonoBehaviour, IPowerup
{
    public int value = 10;
    public void Activate(float wait)
    {
        PlayerManager.instance.health += value;
        Debug.Log("Vita aumentata di " + value);
    }

    public void Deactivate(float wait)
    {
        throw new System.NotImplementedException();
    }
}
