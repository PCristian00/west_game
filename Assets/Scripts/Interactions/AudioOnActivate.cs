using UnityEngine;

public class AudioOnActivate : MonoBehaviour, IActivate
{
    public AudioClip audioClip;
    private AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Activate()
    {
       audioSource.PlayOneShot(audioClip);       
    }


}