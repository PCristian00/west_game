using UnityEngine;

public class ExplodeOnCount : MonoBehaviour
{
    public CountOnActivate countOnActivate;
    public int countLimit = 5;

    public GameObject explosion;
    //public AudioClip explosionSound;
    //private AudioSource audioSource;

    private bool exploded = false;

    //private void Start()
    //{
    //    audioSource = GetComponent<AudioSource>();
    //}

    // Update is called once per frame
    void Update()
    {
        if (countOnActivate)
        {
            if (countOnActivate.GetCount() >= countLimit && !exploded)
            {              
                Explode();
            }
        }
        else Debug.Log("nessun contatore collegato");
    }

    private void Explode()
    {
        //Instantiate explosion
        if (explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);

        //if (explosionSound)
        //{
        //   // Debug.Log("SUONO ESPLOSIONE");
        //    audioSource.PlayOneShot(explosionSound);
        //}

        exploded = true;

        ActivateCheat();
        
        Invoke(nameof(DestroyObject), 0.005f);
        
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void ActivateCheat()
    {
        WalletManager.instance.wallet += 5000;
        SaveManager.UpdateFloat(WalletManager.instance.saveKey, WalletManager.instance.wallet);
    }
}
