using UnityEngine;

public class SaloonDoors : MonoBehaviour
{
    public TransformAnimation[] animations;
    private AudioSource audioSource;

    void Start()
    {
        animations = GetComponentsInChildren<TransformAnimation>();
        audioSource = GetComponent<AudioSource>();
    }

    public void OpenDoors()
    {
        if(audioSource)
        audioSource.Play();

        foreach (var anim in animations) anim.Play();

    }

    public void CloseDoors()
    {
        if(audioSource)
        audioSource.Play();

        foreach (var anim in animations) anim.Play(false);
    }
}
