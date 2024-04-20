using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FORSE COPIARE IN GAME MANAGER

public class LoadoutManager : MonoBehaviour
{
    public static LoadoutManager Instance;

    public GameObject[] weapons;

    private int current = 0;

    private GameObject _currentWeapon;

    public event Action<GameObject> OnWeaponChanged;

    public GameObject CurrentWeapon
    {
        get => _currentWeapon;

        protected set
        {
            if (_currentWeapon == value)
            {
                Debug.Log("Current weapon: " + _currentWeapon.name);
                return;
            }

            _currentWeapon = value;
            Debug.Log("New current weapon: " + _currentWeapon.name);
            OnWeaponChanged?.Invoke(_currentWeapon);
        }
    }

    private void Start()
    {
        Instance = this;
        CurrentWeapon = weapons[current];
        LoadOutInfo();


    }
    // Update is called once per frame
    void Update()
    {

        // FORSE FARE CON ROTELLA MOUSE

        // L'arma viene cambiata solo se non sta ricaricando
        if (!_currentWeapon.GetComponent<ProjectileGun>().reloading)
        {
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
                CurrentWeapon = weapons[current];
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
                CurrentWeapon = weapons[current];
            }
        }
       // else Debug.Log("IMPOSSIBILE CAMBIARE. RICARICA");
    }

    private void LoadOutInfo()
    {
        Debug.Log("Armi su giocatore: " + weapons.Length);
        foreach (var weapon in weapons)
        {
            Debug.Log("Arma: " + weapon.name + ";");
        }
    }
}
