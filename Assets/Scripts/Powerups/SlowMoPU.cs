using System.Collections;
using UnityEngine;

public class SlowMoPU : MonoBehaviour, IPowerup
{
    public void Activate(float wait)
    {
        GameManager.instance.enemySpeed *= 0.5f;


        Debug.Log("Velocita' di gioco dimezzata a " + GameManager.instance.enemySpeed);
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
        GameManager.instance.enemySpeed /= 0.5f;

        Debug.Log("Velocita' di gioco resettata a " + GameManager.instance.enemySpeed);
    }
}
