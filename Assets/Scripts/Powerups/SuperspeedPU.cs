using System.Collections;
using UnityEngine;

public class SuperspeedPU : MonoBehaviour, IPowerup
{


    public void Activate(float wait)
    {
        PlayerMovement.instance.moveSpeed *= 2;
        PlayerMovement.instance.airMultiplier *= 2;

        Debug.Log("Velocita' di movimento e 'volo' duplicate");
        Deactivate(wait);
        
        // Debug.Log("Tempo passato: "+TimerDeactivate(wait));
        //Invoke(nameof(Deactivate), 10f);
    }

    public void Deactivate(float wait)
    {
       
        StopAllCoroutines();
        StartCoroutine(TimerDeactivate(wait));
        // Debug.Log("passati "+wait+" sec");





    }

    IEnumerator TimerDeactivate(float wait)
    {
        // float t = 0f;

        //wait = 5f;


        yield return new WaitForSeconds(wait);
        PlayerMovement.instance.moveSpeed /= 2;
        PlayerMovement.instance.airMultiplier /= 2;

        Debug.Log("Velocita' di movimento e 'volo' resettate");
        // t += Time.deltaTime / wait;          

    }
}
