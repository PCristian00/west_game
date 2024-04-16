using UnityEngine;
using TMPro;
using System.Collections;

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
    [Tooltip("Se attivato, il tempo di ricarica è moltiplicato per i proiettili sparati.")]
    public bool hasCylinder;

    int bulletsLeft, bulletsShot;

    //Recoil
    public Rigidbody playerRb;
    public float recoilForce;

    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    [Header("Reference")]
    public Camera fpsCam;
    public Transform attackPoint;

    //Graphics
    [Header("Graphics")]
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammoInfo;
    // DA IMPLEMENTARE??
    public Sprite crosshairSprite;
    public UnityEngine.UI.Image crosshair;
    private Color crosshairColor;

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
        crosshairColor = crosshair.color;
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

        // OLD
        // if(ammunitionDisplay != null) ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);

        ////Set ammo display, if it exists :D
        if (ammoInfo != null && !reloading)
            ammoInfo.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
        else if (reloading) ammoInfo.text = "Reloading...";

        // FORSE QUI FINIRE o METTERE IN START o RIMUOVERE
        //if (crosshair && crosshairSprite)
        //{
        //crosshair.sprite = crosshairSprite;
        //}
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
            if(hasCylinder) StopAllCoroutines();
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
        RaycastHit hit;

        //check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
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
            // MODIFICA GROSSA DA PROVARE: Dare la possibilità al giocatore di interrompere la ricarica in anticipo con armi revolver

            StopAllCoroutines();
            StartCoroutine(CylinderReload());


            // Se l'arma ha un caricatore a tamburo il tempo di ricarica dipende dai colpi rimasti

            // FUNZIONANTE MA NON PUò ESSERE INTERROTTA LA RICARICA PRIMA CHE IL CARICATORE SIA PIENO
            //Invoke(nameof(ReloadFinished), reloadTime * (magazineSize - bulletsLeft));

            //   Debug.Log("Time to reload: " + reloadTime * (magazineSize - bulletsLeft));
        }
        else
        {
            audioSource.loop = true;
            audioSource.clip = reloadSound;
            audioSource.Play();
            Invoke(nameof(ReloadFinished), reloadTime); //Invoke ReloadFinished function with your reloadTime as delay
        }
            
    }

    IEnumerator CylinderReload()
    {
        float t = 0f;
        while (bulletsLeft < magazineSize)
        {
            yield return new WaitForSeconds(reloadTime);
            t += Time.deltaTime / reloadTime;
            bulletsLeft++;
            audioSource.clip = reloadSound;
            audioSource.Play();
            Debug.Log("Bullets left: " + bulletsLeft);
            reloading = false;
            ResetShot();
        }        
    }
    private void ReloadFinished()
    {
        audioSource.loop = false;
        //Fill magazine
        bulletsLeft = magazineSize;
        reloading = false;
        // SOSTITUIRE CON COLORE INIZIALE
        crosshair.color = crosshairColor;
    }
}
