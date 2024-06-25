using UnityEngine;

public class AudioOnActivate : MonoBehaviour, IActivate
{
    public AudioClip audioClip;

    public void Activate()
    {
        AudioSource.PlayClipAtPoint(audioClip, transform.position);
    }


}