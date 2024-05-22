using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPU : MonoBehaviour, IPowerup
{
    private PlayerManager PlayerManager;

    public void Start()
    {
        PlayerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
    }

    public void Activate()
    {
        PlayerManager.health += 10;
        Debug.Log("Vita aumentata di 10");
    }
}
