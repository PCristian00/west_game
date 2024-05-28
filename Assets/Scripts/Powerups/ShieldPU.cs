using System.Collections;
using UnityEngine;

public class ShieldPU : MonoBehaviour, IPowerup
{
    public void Activate(float wait)
    {
        PlayerManager.instance.invincible = true;
        Debug.Log("Scudo attivato");
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
        PlayerManager.instance.invincible = false;

        Debug.Log("Scudo disattivato");
    }
}

