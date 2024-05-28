using System.Collections;
using UnityEngine;

public class SuperjumpPU : MonoBehaviour, IPowerup
{
    public void Activate(float wait)
    {
       // PlayerMovement.instance.jumpForce *= 1.5f;
        PlayerMovement.instance.doubleJumpActive = true;
        Debug.Log("Doppio salto attivato");
        Deactivate(wait);
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
        PlayerMovement.instance.doubleJumpActive = false;
        //PlayerMovement.instance.airMultiplier /= 2;

        Debug.Log("Doppio salto disattivato");
        // t += Time.deltaTime / wait;          

    }
}

