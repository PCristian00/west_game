using System.Collections;
using UnityEngine;

public class SuperspeedPU : MonoBehaviour, IPowerup
{
    public void Activate(float wait)
    {
        PlayerMovement.instance.moveSpeed *= 2;
        PlayerMovement.instance.airMultiplier *= 2;

        // Debug.Log("Velocita' di movimento e 'volo' duplicate");
        Deactivate(wait);
    }

    public void Deactivate(float wait)
    {
        StopAllCoroutines();
        StartCoroutine(TimerDeactivate(wait));
    }

    IEnumerator TimerDeactivate(float wait)
    {
        yield return new WaitForSeconds(wait);
        PlayerMovement.instance.moveSpeed /= 2;
        PlayerMovement.instance.airMultiplier /= 2;

       // Debug.Log("Velocita' di movimento e 'volo' resettate");
    }
}
