using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperjumpPU : MonoBehaviour, IPowerup
{
    public void Activate()
    {
        PlayerMovement.instance.jumpForce *= 3;
        Debug.Log("Forza di salto triplicata");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
