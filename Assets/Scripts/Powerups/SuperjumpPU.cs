using UnityEngine;

public class SuperjumpPU : MonoBehaviour, IPowerup
{
    public void Activate(float wait)
    {
       // PlayerMovement.instance.jumpForce *= 1.5f;
        PlayerMovement.instance.doubleJumpActive = true;
        Debug.Log("Doppio salto attivato");        
    }

    public void Deactivate(float wait)
    {
        throw new System.NotImplementedException();
    }
}
