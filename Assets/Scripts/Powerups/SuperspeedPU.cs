using UnityEngine;

public class SuperspeedPU : MonoBehaviour, IPowerup
{
    public void Activate()
    {
        PlayerMovement.instance.moveSpeed *= 2;
        PlayerMovement.instance.airMultiplier *= 2;

        Debug.Log("Velocita' di movimento e 'volo' duplicate");
    }
}
