using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperjumpPU : MonoBehaviour, IPowerup
{
    public void Activate()
    {
       // PlayerMovement.instance.jumpForce *= 1.5f;
        PlayerMovement.instance.doubleJumpActive = true;
        Debug.Log("Doppio salto attivato");        
    }

    public void Deactivate()
    {
        throw new System.NotImplementedException();
    }
}
