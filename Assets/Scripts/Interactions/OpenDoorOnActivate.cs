using UnityEngine;

public class OpenDoorOnActivate : MonoBehaviour, IActivate
{
    // public GameObject door;
    private AudioSource audioSource;
    public TransformAnimation doorAnimation;

    public AudioClip openingSound;
    public AudioClip errorSound;


    private bool isClosed = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Activate()
    {
        if (doorAnimation)
            if (isClosed)
            {
                doorAnimation.Play();
                isClosed = false;
                if (audioSource) audioSource.PlayOneShot(openingSound);

                Invoke(nameof(DestroyDoor), doorAnimation.duration + 1f);
            }
            else
            {
                if (audioSource) audioSource.PlayOneShot(errorSound);
            }
        else
        {
            if (audioSource) audioSource.PlayOneShot(errorSound);
            // Debug.Log("Porta distrutta o non collegata");
        }
    }

    public void DestroyDoor()
    {
        Destroy(doorAnimation.gameObject);
        // Debug.Log("PORTA DISTRUTTA OFF-SCREEN");
    }
}
