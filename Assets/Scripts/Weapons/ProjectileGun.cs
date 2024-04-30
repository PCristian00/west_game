﻿using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class ProjectileGun : MonoBehaviour
{
    [Header("Bullet stats")]
    //bullet
    public GameObject bullet;

    //bullet force
    public float shootForce, upwardForce;

    [Header("Gun stats")]
    public float timeBetweenShooting;
    public float spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    [Tooltip("Se attivato, il tempo di ricarica dipende  dai proiettili sparati.")]
    public bool hasCylinder;

    int bulletsLeft, bulletsShot;

    //Recoil
    public Rigidbody playerRb;
    public float recoilForce;

    //bools
    public bool shooting, readyToShoot, reloading;

    //Reference
    [Header("Reference")]
    public Camera fpsCam;
    public Transform attackPoint;
    private Image crosshair;

    //Graphics
    [Header("Graphics")]
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammoInfo;
    public Sprite crosshairSprite;
    private Color crosshairColor;
    private EasyReloadAnimation reloadAnimation;

    //Sound
    [Header("Sound")]
    public AudioClip shootSound;
    public AudioClip reloadSound;
    private AudioSource audioSource;

    //bug fixing :D
    [Header("Debug")]
    public bool allowInvoke = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        crosshair = GameObject.FindGameObjectWithTag("Crosshair").GetComponent<Image>();
        crosshairColor = crosshair.color;
        reloadAnimation = GetComponent<EasyReloadAnimation>();
    }

    private void Awake()
    {
        //make sure magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

        ////Set ammo display, if it exists :D
        if (ammoInfo != null && !reloading)
            ammoInfo.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
        else if (reloading) ammoInfo.text = "Reloading...";

        if (crosshair && crosshairSprite)
        {
            crosshair.sprite = crosshairSprite;
        }
    }
    private void MyInput()
    {
        //Check if allowed to hold down button and take corresponding input
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //Reloading 
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        //Reload automatically when trying to shoot without ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        //Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            // Se l'arma è a tamburo (cylinder), la ricarica può essere interrotta dal giocatore
            if (hasCylinder)
            {
                StopAllCoroutines();
                crosshair.color = crosshairColor;
            }

            //Set bullets shot to 0
            bulletsShot = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        //Find the exact hit position using a raycast
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Just a ray through the middle of your current view

        //check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out RaycastHit hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); //Just a point far away from the player

        //Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); //Just add spread to last direction

        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity); //store instantiated bullet in currentBullet
        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

        //Instantiate muzzle flash, if you have one
        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        // Play sound
        if (shootSound)
            audioSource.PlayOneShot(shootSound, 1);

        bulletsLeft--;
        bulletsShot++;
        // Debug.Log("BulletsShot = " + bulletsShot);

        //Invoke resetShot function (if not already invoked), with your timeBetweenShooting
        if (allowInvoke)
        {
            Invoke(nameof(ResetShot), timeBetweenShooting);
            allowInvoke = false;

            //Add recoil to player (should only be called once)
            playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
        }

        //if more than one bulletsPerTap make sure to repeat shoot function
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke(nameof(Shoot), timeBetweenShots);
    }
    private void ResetShot()
    {
        //Allow shooting and invoking again
        // reloading = false;
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;

        // TEST cambio colore crosshair
        // In sovrapposizione con ColorOnHover (TROVARE SOLUZIONE)
        crosshair.color = Color.black;

        if (hasCylinder)
        {
            StopAllCoroutines();
            StartCoroutine(CylinderReload());
        }
        else
        {
            audioSource.loop = true;
            audioSource.clip = reloadSound;
            if (reloadAnimation)
                reloadAnimation.Play(reloadTime);
            audioSource.Play();
            Invoke(nameof(ReloadFinished), reloadTime); //Invoke ReloadFinished function with your reloadTime as delay
        }
    }

    IEnumerator CylinderReload()
    {

        float t = 0f;
        while (bulletsLeft < magazineSize)
        {
            if (reloadAnimation)
                reloadAnimation.Play(reloadTime);
            audioSource.Play();

            yield return new WaitForSeconds(reloadTime);
            t += Time.deltaTime / reloadTime;
            bulletsLeft++;
            audioSource.clip = reloadSound;

            // Debug.Log("Bullets left: " + bulletsLeft);
            reloading = false;
            ResetShot();
        }
        crosshair.color = crosshairColor;
    }
    private void ReloadFinished()
    {
        audioSource.loop = false;
        //Fill magazine
        bulletsLeft = magazineSize;
        crosshair.color = crosshairColor;
        reloading = false;
        // Debug.Log("Reload finished! (R = " + reloading + ")");
    }
}
