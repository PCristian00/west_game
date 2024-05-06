using System;
using UnityEngine;
using UnityEngine.UI;


public class CrosshairManager : MonoBehaviour
{
    public static CrosshairManager Instance;

    private Color startColor;

    private Image crosshair;

    //public GameObject[] crosshairs;

    //private int current = 0;



    //private GameObject _currentCrosshair;

    //public event Action<GameObject> OnCrosshairChanged;

    //public GameObject CurrentCrosshair
    //{
    //    get => _currentCrosshair;

    //    protected set
    //    {
    //        if (_currentCrosshair == value)
    //        {
    //            // Debug.Log("Current crosshair: " + _currentCrosshair.name);
    //            return;
    //        }

    //        _currentCrosshair = value;
    //        // Debug.Log("New current crosshair: " + _currentCrosshair.name);
    //        OnCrosshairChanged?.Invoke(_currentCrosshair);
    //    }
    //}

    private void Start()
    {
        Instance = this;

        crosshair = GameObject.FindGameObjectWithTag("Crosshair").GetComponent<Image>();

        startColor = crosshair.color;

       // CurrentCrosshair = crosshairs[current];

    }

    public void ChangeColor(Color newColor)
    {
        crosshair.color = newColor;
    }

    public void ResetColor()
    {
        crosshair.color = startColor;
    }

    public void ChangeSprite(Sprite sprite)
    {
        crosshair.sprite = sprite;
    }


    //// Update is called once per frame
    //void Update()
    //{


    //    // L'arma viene cambiata solo se non sta ricaricando
    //    if (!_currentCrosshair.GetComponent<ProjectileGun>().reloading)
    //    {
    //        if (Input.GetButtonDown("NextWeapon") || Input.GetAxis("Mouse ScrollWheel") > 0f)
    //        {

    //            crosshairs[current].SetActive(false);
    //            current++;
    //            // Debug.Log(current);
    //            if (current >= crosshairs.Length)
    //            {
    //                current = 0;
    //            }
    //            // Debug.Log("AUM// Arma " + current + ": " + crosshairs[current].gameObject.name);
    //            crosshairs[current].SetActive(true);
    //            CurrentCrosshair = crosshairs[current];
    //        }
    //        else if (Input.GetButtonDown("PrevWeapon") || Input.GetAxis("Mouse ScrollWheel") < 0f)
    //        {
    //            crosshairs[current].SetActive(false);
    //            current--;
    //            //  Debug.Log(current);
    //            if (current < 0)
    //            {
    //                //  Debug.Log(current);
    //                current = crosshairs.Length - 1;
    //            }
    //            //  Debug.Log("DEC// Arma " + current+": " + crosshairs[current].gameObject.name);
    //            crosshairs[current].SetActive(true);
    //            CurrentCrosshair = crosshairs[current];
    //        }
    //    }
    //    // else Debug.Log("IMPOSSIBILE CAMBIARE. RICARICA");
    //}


}