using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class ProjectileGun : MonoBehaviour
{
    [Header("Bullet stats")]
    // Proiettile da sparare
    public GameObject bullet;

    // Forza del proiettile (usare upward per spari a parabola)
    public float shootForce, upwardForce;

    [Header("Gun stats")]
    public float timeBetweenShooting;
    public float spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    [Tooltip("Se attivato, il tempo di ricarica dipende  dai proiettili sparati.")]
    public bool hasCylinder;
    [Tooltip("Se attivato, permette di mirare con mirino.")]
    public bool hasScope = false;
    public bool activeScope = false;
    public float zoomRate = 40f;

    int bulletsLeft, bulletsShot;

    //Recoil
    public Rigidbody playerRb;
    public float recoilForce;

    [Tooltip("Tempo (in secondi) necessario per riporre / estrarre arma.")]
    public float changeSpeed = 0.5f;

    //bools
    public bool shooting, readyToShoot, reloading, isHidden = false;

    //Reference
    [Header("Reference")]
    public Camera fpsCam;
    public Transform attackPoint;
    private GameObject gunMesh;
    private Collider gunCollider;
    

    //Graphics
    [Header("Graphics")]
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammoInfo;
    public Sprite crosshairSprite;

    [SerializeField] private RotationAnimation reloadAnimation;

    [SerializeField] private RotationAnimation collisionAnimation;

    //Sound
    [Header("Sound")]
    public AudioClip shootSound;
    public AudioClip reloadSound;
    private AudioSource audioSource;

    [Header("Debug")]
    public bool allowInvoke = true;
    private Vector3 scale;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        

        gunMesh = GetComponentInChildren<MeshRenderer>().gameObject;

        gunCollider = gunMesh.GetComponent<Collider>();
      //  Debug.Log("Collider di " + name + " = " + gunCollider.name);
        gunCollider.enabled = false;
        
    }

    private void Awake()
    {
        // Riempie caricatore all'avvio
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    public void OnEnable()
    {
        try
        {
            if (crosshairSprite)
            {
                CrosshairManager.instance.ChangeSprite(crosshairSprite);
            }
            gunCollider.enabled = true;
            Hide(true);
        }
        catch (NullReferenceException)
        {
            Debug.Log(gameObject.name + ": Riferimento a Crosshair Manager non trovato");
        }
    }

    public void Hide(bool show)
    {
        // Debug.Log(gameObject.name + " change speed = " + changeSpeed);

        if (!show)
        {
            collisionAnimation.Play(changeSpeed, true);
            isHidden = true;
            CrosshairManager.instance.ChangeColor(Color.clear);
        }

        else
        {
            collisionAnimation.Play(changeSpeed, false);
            isHidden = false;
            CrosshairManager.instance.ResetColor();
            // OnEnable();
        }
    }

    private void Update()
    {
        MyInput();

        if (ammoInfo != null && !reloading && !isHidden)
            ammoInfo.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
        else if (reloading) ammoInfo.text = "Reloading...";
        else if (isHidden) ammoInfo.text = gameObject.name + " hidden";

    }

    // Gestisce gli input dell'arma
    private void MyInput()
    {
        // Controlla se è permessa la pressione continua del grilletto e agisce di conseguenza
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (!reloading)
        {
            // Ricarica
            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !isHidden) Reload();

            // Hide test
            if (Input.GetKeyDown(KeyCode.F) && !activeScope)
            {
                Hide(isHidden);
            }

            if (!isHidden)
            {

                // Mirino
                if (hasScope && Input.GetKeyDown(KeyCode.Mouse1))
                {
                    Aim();
                }

                if (readyToShoot && shooting)
                {
                    // Ricarica automatica se caricatore vuoto
                    if (bulletsLeft <= 0) Reload();

                    // Sparo
                    if (bulletsLeft > 0)
                    {
                        // Se l'arma è a tamburo (cylinder), la ricarica può essere interrotta dal giocatore
                        if (hasCylinder)
                        {
                            StopAllCoroutines();
                        }

                        bulletsShot = 0;

                        Shoot();
                    }
                }
            }
        }
    }

    private void Aim()
    {
        if (!activeScope)
        {
            fpsCam.fieldOfView -= zoomRate;

            activeScope = true;
            gunMesh.SetActive(false);
            gunCollider.enabled = false;
        }
        else
        {
            fpsCam.fieldOfView += zoomRate;
            activeScope = false;
            gunMesh.SetActive(true);
            gunCollider.enabled = true;
        }
    }

    // Gestisce lo sparo dell'arma
    private void Shoot()
    {
        readyToShoot = false;

        // Imposta il raycast della mira al centro dello schermo (dove è situato il mirino)
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        // Controlla se è stato colpito qualcosa
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out RaycastHit hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); // Punto generico lontano dal giocatore

        // Calcola direzione da attackPoint a targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //Calcola dispersione proiettile (se presente)
        float x = UnityEngine.Random.Range(-spread, spread);
        float y = UnityEngine.Random.Range(-spread, spread);

        // Aggiunge dispersione alla direzione precedentemente calcolata
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        // Instanzia il proiettile e ne imposta la direzione
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpread.normalized;

        // Aggiunge forze al proiettile
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

        // Aggiunta di effetti visivi dello sparo
        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        // Aggiunta di effetti sonori dello sparo
        if (shootSound)
            audioSource.PlayOneShot(shootSound, 1);

        bulletsLeft--;
        bulletsShot++;


        // prepara nuovamente allo sparo in base a timeBetweenShooting
        if (allowInvoke)
        {
            Invoke(nameof(ResetShot), timeBetweenShooting);
            allowInvoke = false;

            // Aggiunge rinculo al giocatore
            playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
        }

        // In caso di sparo a raffiche (bulletsPerTap) spara finché non finisce la raffica
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke(nameof(Shoot), timeBetweenShots);
    }

    // Reimposta l'arma per lo sparo
    private void ResetShot()
    {
        // Se il gioco non è concluso
        if (GameManager.instance.CurrentGameState != GameManager.GameState.Lost)
        {
            readyToShoot = true;
            allowInvoke = true;
        }
    }

    // Gestisce la ricarica
    private void Reload()
    {
        reloading = true;

        CrosshairManager.instance.ChangeColor(Color.black);

        if (hasCylinder)
        {
            StopAllCoroutines();
            StartCoroutine(CylinderReload());
        }
        else
        {
            // Avvia in loop il suono della ricarica
            audioSource.loop = true;
            audioSource.clip = reloadSound;
            audioSource.Play();

            if (reloadAnimation)
                reloadAnimation.PlayComplete(reloadTime);

            Invoke(nameof(ReloadFinished), reloadTime);
        }
    }

    // Gestisce la ricarica delle armi a tamburo, che può essere interrotta anche se incompleta da uno sparo
    IEnumerator CylinderReload()
    {
        while (bulletsLeft < magazineSize)
        {
            if (reloadAnimation)
                reloadAnimation.PlayComplete(reloadTime);
            audioSource.Play();

            yield return new WaitForSeconds(reloadTime);

            bulletsLeft++;
            audioSource.clip = reloadSound;

            // Caricato almeno un proiettile, si può sparare per interrompere la ricarica
            reloading = false;
            ResetShot();
        }

        // Controlla se il mirino era puntato su un nemico
        CrosshairManager.instance.EnemyOnCrosshair();
    }

    // Segnala che la ricarica è completa
    private void ReloadFinished()
    {
        // Ferma il suono della ricarica in loop
        audioSource.loop = false;
        bulletsLeft = magazineSize;

        // Controlla se il mirino era puntato su un nemico
        CrosshairManager.instance.EnemyOnCrosshair();
        reloading = false;
        // Debug.Log("Reload finished! (R = " + reloading + ")");
    }

    //Gestione collisioni dell'arma

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.gameObject.layer);

        if (!other.CompareTag("Bullet") && !other.CompareTag("Player") && !other.CompareTag("Coin"))
        {
            //  Debug.Log("Test: collisione trigger di " + gameObject.name + " con " + other.name + "[tag = " + other.tag + " ]");

            Hide(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Bullet") && !other.CompareTag("Player") && !other.CompareTag("Coin"))
        {
            // Debug.Log("Test: collisione trigger exit di " + gameObject.name + " con " + other.name + "[tag = " + other.tag + " ]");

            Hide(true);
        }
    }
}