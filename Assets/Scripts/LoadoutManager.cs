using System;
using UnityEngine;

public class LoadoutManager : MonoBehaviour
{
    public static LoadoutManager Instance;

    public GameObject[] weapons;

    private int current = 0;

    private GameObject _currentWeapon;

    public event Action<GameObject> OnWeaponChanged;

    private bool next;

    public GameObject loadingTest;

    public GameObject CurrentWeapon
    {
        get => _currentWeapon;

        protected set
        {
            if (_currentWeapon == value)
            {
                // Debug.Log("Current weapon: " + _currentWeapon.name);
                return;
            }

            _currentWeapon = value;
            // Debug.Log("New current weapon: " + _currentWeapon.name);
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
        // L'arma viene cambiata solo se non sta ricaricando
        if (!_currentWeapon.GetComponent<ProjectileGun>().reloading)
        {
            if (Input.GetButtonDown("NextWeapon") || Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                _currentWeapon.GetComponent<ProjectileGun>().Hide(false);
                next = true;
                Invoke(nameof(ChangeWeapon), 0.5f);
                // ChangeWeapon(true);

            }
            else if (Input.GetButtonDown("PrevWeapon") || Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                _currentWeapon.GetComponent<ProjectileGun>().Hide(false);
                next = false;
                Invoke(nameof(ChangeWeapon), 0.5f);
                // ChangeWeapon(false);
            }
        }
        // else Debug.Log("IMPOSSIBILE CAMBIARE. RICARICA");
    }

    private void ChangeWeapon()
    {
        if (next)
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

        else
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

        // Debug.Log("cambio arma completato");

    }


    // Funzioni per caricamento: risolvono parzialmente il problema del crosshair e delle animazioni non caricate in tempo.
    // Potrebbe essere rimosso dalla versione finale del gioco se ottimizzato correttamente.
    private void LoadOutInfo()
    {
        CrosshairManager.Instance.ResetColor();

        //  Debug.Log("Armi su giocatore: " + weapons.Length);
        foreach (var weapon in weapons)
        {
            // Debug.Log("Arma caricata: " + weapon.name + ";");

            weapon.SetActive(true);

            //  weapon.GetComponent<ProjectileGun>().Hide(false);
        }

        // LoadComplete();

        Invoke(nameof(LoadComplete), 1.5f);

    }

    private void LoadComplete()
    {
        for (int i = 0; i < weapons.Length; i++)
        {

            // Invoke(nameof(ChangeWeapon), 0.5f);
            ChangeWeapon();
        }

        weapons[0].SetActive(true);

        if (loadingTest)
            loadingTest.SetActive(false);
    }
}
