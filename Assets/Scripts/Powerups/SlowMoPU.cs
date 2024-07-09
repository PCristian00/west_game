using System.Collections;
using UnityEngine;

public class SlowMoPU : MonoBehaviour, IPowerup
{
    public void Activate(float wait)
    {
        GameManager.instance.slowMode = true;


        // Debug.Log("Velocita' di gioco dimezzata a " + GameManager.instance.slowMultiplier);
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
        GameManager.instance.slowMode = false;

        // Debug.Log("Velocita' di gioco resettata.");
    }
}
