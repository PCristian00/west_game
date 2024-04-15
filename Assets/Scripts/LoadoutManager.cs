using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FORSE COPIARE IN GAME MANAGER

public class LoadoutManager : MonoBehaviour
{

    public GameObject[] weapons;

    private int current;
    // Start is called before the first frame update
    //void Start()
    //{
    //  Debug.Log("Armi su giocatore: "+weapons.Length);
    //  foreach (var weapon in weapons)
    //    {
    //        Debug.Log("Arma: " + weapon.name +";");
    //    }
    //}

    // Update is called once per frame
    void Update()
    {

        // FORSE FARE CON ROTELLA MOUSE


        if (Input.GetButtonDown("NextWeapon"))
        {

            weapons[current].SetActive(false);
            current++;
            // Debug.Log(current);
            if (current >= weapons.Length)
            {
                current = 0;
            }
            // Debug.Log("AUM// Arma " + current + ": " + weapons[current].gameObject.name);
            weapons[current].SetActive(true);
        }
        else if (Input.GetButtonDown("PrevWeapon"))
        {
            weapons[current].SetActive(false);
            current--;
            //  Debug.Log(current);
            if (current < 0)
            {
                //  Debug.Log(current);
                current = weapons.Length - 1;
            }
            //  Debug.Log("DEC// Arma " + current+": " + weapons[current].gameObject.name);
            weapons[current].SetActive(true);
        }
    }
}
