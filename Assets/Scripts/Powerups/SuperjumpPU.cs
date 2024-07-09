using System.Collections;
using UnityEngine;

public class SuperjumpPU : MonoBehaviour, IPowerup
{
    public void Activate(float wait)
    {
        PlayerMovement.instance.doubleJumpActive = true;

        // Debug.Log("Doppio salto attivato");
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
        PlayerMovement.instance.doubleJumpActive = false;

        // Debug.Log("Doppio salto disattivato");
    }
}

