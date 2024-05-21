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

    int bulletsLeft, bulletsShot;

    //Recoil
    public Rigidbody playerRb;
    public float recoilForce;

    //bools
    public bool shooting, readyToShoot, reloading, isColliding = false, isHidden = false;

    //Reference
    [Header("Reference")]
    public Camera fpsCam;
    public Transform attackPoint;
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

   // CrosshairManager chm = null;

    private void Start()
    {
        // gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        gunCollider = GetComponent<Collider>();
        gunCollider.enabled = false;


      //  if (CrosshairManager.Instance) Debug.Log("PG: CH manager loaded");

       
        //  scale = gameObject.transform.localScale;
        //  gameObject.transform.localScale = Vector3.zero;

        // Invoke(nameof(OnEnable), 1);

        //if (crosshairSprite)
        //{
        //    CrosshairManager.Instance.ChangeSprite(crosshairSprite);
        //}
    }

    private void Awake()
    {
        // Riempie caricatore all'avvio
        bulletsLeft = magazineSize;
        readyToShoot = true;
        // Hide(true);


        //   Debug.Log(gameObject.name + " avviata");


    }

    public void OnEnable()
    {
        try
        {
            // Debug.Log(gameObject.name + " attivato");
            if (crosshairSprite)
            {
              //  Debug.Log(crosshairSprite);
                //try
                //{
                CrosshairManager.Instance.ChangeSprite(crosshairSprite);
                //} catch (NullReferenceException)
                //{
                //    Debug.Log("Crosshair Manager ancora in caricamento...");
                //}
            }

            // gameObject.transform.localScale = scale;
            gunCollider.enabled = true;
            Hide(true);
        } catch (NullReferenceException)
        {
            Debug.Log("Crosshair Manager ancora in caricamento...");
        }
    }

    public void Hide(bool show)
    {
        if (!show)
        {
            collisionAnimation.Play(0.5f, true);
            isHidden = true;
            CrosshairManager.Instance.ChangeColor(Color.clear);
        }

        else
        {
            collisionAnimation.Play(0.5f, false);
            isHidden = false;
            CrosshairManager.Instance.ResetColor();
            // OnEnable();
        }
    }


    private void Update()
    {
        // RIPROVARE A RIMUOVERE DA UPDATE

        // ChangeSprite dovrebbe essere solo su OnEnable. Richiamarlo ad ogni update è inutile.
        // Togliere update qui però dà problemi alla prima arma, che inizia con il mirino vuoto generico.
        // Il problema è che CrosshairManager non viene avviato in tempo.
        // Capire priorità di esecuzione meglio OPPURE riprovare in seguito quando si ha un menù e si possono nascondere caricamenti.


        //if (crosshairSprite)
        //{
        //    CrosshairManager.Instance.ChangeSprite(crosshairSprite);
        //}


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

        if (!reloading && !isColliding)
        {
            // Ricarica
            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !isHidden) Reload();

            // Hide test
            if (Input.GetKeyDown(KeyCode.F))
            {
                Hide(isHidden);
            }

            if (!isHidden && readyToShoot && shooting)
            {
                // Debug.Log(gameObject.name+" nascosta = " + isHidden);
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
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.Lost)
        {
            readyToShoot = true;
            allowInvoke = true;
        }
    }

    // Gestisce la ricarica
    private void Reload()
    {
        reloading = true;

        CrosshairManager.Instance.ChangeColor(Color.black);

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
        float t = 0f;
        while (bulletsLeft < magazineSize)
        {
            if (reloadAnimation)
                reloadAnimation.PlayComplete(reloadTime);
            audioSource.Play();

            yield return new WaitForSeconds(reloadTime);
            t += Time.deltaTime / reloadTime;
            bulletsLeft++;
            audioSource.clip = reloadSound;

            // Debug.Log("Bullets left: " + bulletsLeft);

            // Caricato almeno un proiettile, si può sparare per interrompere la ricarica
            reloading = false;
            ResetShot();
        }

        // Controlla se il mirino era puntato su un nemico
        CrosshairManager.Instance.EnemyOnCrosshair();
    }

    // Segnala che la ricarica è completa
    private void ReloadFinished()
    {
        // Ferma il suono della ricarica in loop
        audioSource.loop = false;
        bulletsLeft = magazineSize;

        // Controlla se il mirino era puntato su un nemico
        CrosshairManager.Instance.EnemyOnCrosshair();
        reloading = false;
        // Debug.Log("Reload finished! (R = " + reloading + ")");
    }

    //Gestione collisioni dell'arma

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bullet") && !other.CompareTag("Player") && !isHidden)
        {
            //  Debug.Log("Test: collisione trigger di " + gameObject.name + " con " + other.name + "[tag = " + other.tag + " ]");

            collisionAnimation.Play(1f, true);
            isColliding = true;
            CrosshairManager.Instance.ChangeColor(Color.clear);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Bullet") && !other.CompareTag("Player") && !isHidden)
        {
            // Debug.Log("Test: collisione trigger exit di " + gameObject.name + " con " + other.name + "[tag = " + other.tag + " ]");

            collisionAnimation.Play(1f, false);
            isColliding = false;
            CrosshairManager.Instance.ResetColor();
        }
    }
}