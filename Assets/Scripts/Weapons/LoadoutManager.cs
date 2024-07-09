using System;
using UnityEngine;

public class LoadoutManager : MonoBehaviour
{
    public static LoadoutManager instance;

    public GameObject[] weapons;

    // PARTE DA 1 perché REVOLVER sempre attivo
    private int activeWeaponsCounter = 1;

    private int current = 0;

    private GameObject _currentWeapon;

    public event Action<GameObject> OnWeaponChanged;

    private bool next;

    public GameObject briefingScreen;

    public GameObject CurrentWeapon
    {
        get => _currentWeapon;

        protected set
        {
            if (_currentWeapon == value)
            {
                return;
            }

            _currentWeapon = value;

            OnWeaponChanged?.Invoke(_currentWeapon);
        }
    }

    private void Start()
    {
        instance = this;
        CurrentWeapon = weapons[current];
        LoadOutInfo();

        // Il fucile da CECCHINO è disponibile solo in livello Bonus ed è l'unica arma disponibile
        if (GameManager.instance.LevelIndex == 3)
        {
            weapons[4].name += "[E]";
            Debug.Log("LIVELLO SNIPER: SOLO ARMA DA CECCHINO");
        }
        else
        {
            for (int i = 0; i < GunPopup.states.Length; i++)
            {
                if (GunPopup.states[i] == 2)
                {
                    current = i;
                    weapons[i].name += " [E]";
                    activeWeaponsCounter++;
                }
            }

            // Se una delle armi considerate attive è il revolver potenziato, non lo conta
            if (weapons[0].name.Contains("[E]"))
            {
                activeWeaponsCounter--;
            }

            // Se non sono state comprate altre armi o il revolver potenziato, carica il revolver classico
            if (activeWeaponsCounter == 1 && !weapons[0].name.Contains("[E]")) weapons[3].name += " [E]";

        }
    }

    void Update()
    {
        // L'arma viene cambiata solo se non sta ricaricando e ce ne sono almeno 2
        if (!_currentWeapon.GetComponent<ProjectileGun>().reloading && activeWeaponsCounter > 1)
        {
            if (Input.GetButtonDown("NextWeapon") || Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                _currentWeapon.GetComponent<ProjectileGun>().Hide(false);
                next = true;
                Invoke(nameof(ChangeWeapon), 0.5f);
            }
            else if (Input.GetButtonDown("PrevWeapon") || Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                _currentWeapon.GetComponent<ProjectileGun>().Hide(false);
                next = false;
                Invoke(nameof(ChangeWeapon), 0.5f);
            }
        }
    }

    private void ChangeWeapon()
    {

        if (next)
        {
            weapons[current].SetActive(false);

            do
            {
                current++;

                if (current >= weapons.Length)
                {
                    current = 0;
                }
            } while (!weapons[current].name.Contains("E"));

            weapons[current].SetActive(true);
            CurrentWeapon = weapons[current];
        }

        else
        {
            weapons[current].SetActive(false);

            do
            {
                current--;

                if (current < 0)
                {
                    current = weapons.Length - 1;
                }

            } while (!weapons[current].name.Contains("E"));
            weapons[current].SetActive(true);
            CurrentWeapon = weapons[current];
        }
    }


    // Funzioni per caricamento: risolvono parzialmente il problema del crosshair e delle animazioni non caricate in tempo.
    // Attenzione: disattiva la schermata di Briefing dopo 1.5 secondi.
    // Disattivare manualmente schermata di Briefing se questa funzione non è usata.

    private void LoadOutInfo()
    {
        CrosshairManager.instance.ResetColor();

        foreach (var weapon in weapons)
        {
            weapon.SetActive(true);
        }

        Invoke(nameof(LoadComplete), 1.5f);
    }

    private void LoadComplete()
    {

        foreach (var weapon in weapons)
        {
            weapon.SetActive(false);
        }

        ChangeWeapon();

        if (briefingScreen)
            briefingScreen.SetActive(false);
    }
}
