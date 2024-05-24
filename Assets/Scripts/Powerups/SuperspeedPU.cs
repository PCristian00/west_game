using System.Collections;
using UnityEngine;

public class SuperspeedPU : MonoBehaviour, IPowerup
{
    public void Activate()
    {
        PlayerMovement.instance.moveSpeed *= 2;
        PlayerMovement.instance.airMultiplier *= 2;

        Debug.Log("Velocita' di movimento e 'volo' duplicate");
        Deactivate();
        //Invoke(nameof(Deactivate), 10f);
    }

    public void Deactivate()
    {
        Debug.Log("AAAAA");

        StopAllCoroutines();
        StartCoroutine(TimerDeactivate());


    }

    IEnumerator TimerDeactivate()
    {
        float t = 0f;

        float wait = 5f;


        yield return new WaitForSeconds(wait);
        PlayerMovement.instance.moveSpeed /= 2;
        PlayerMovement.instance.airMultiplier /= 2;

        Debug.Log("Velocita' di movimento e 'volo' resettate");
        // t += Time.deltaTime / wait;          

    }
}
