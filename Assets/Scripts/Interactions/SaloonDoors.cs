using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaloonDoors : MonoBehaviour
{
    public TransformAnimation[] animations;
    public AudioClip sound;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        animations = GetComponentsInChildren<TransformAnimation>();
        audioSource = GetComponent<AudioSource>();
    }

    public void OpenDoors()
    {
        audioSource.PlayOneShot(sound);

        foreach (var anim in animations) anim.Play();

    }

    public void CloseDoors()
    {
        audioSource.PlayOneShot(sound);

        foreach (var anim in animations) anim.Play(false);
    }
}
