using UnityEngine;

public class ExplodeOnCount : MonoBehaviour
{
    public CountOnActivate countOnActivate;
    public int countLimit = 5;

    public GameObject explosion;
    public AudioClip explosionSound;

    // Update is called once per frame
    void Update()
    {
        if (countOnActivate)
        {
            if (countOnActivate.GetCount() >= countLimit)
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

        if (explosionSound)
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);

        Destroy(gameObject);
    }
}
