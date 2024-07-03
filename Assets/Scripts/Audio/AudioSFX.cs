using UnityEngine;
using UnityEngine.EventSystems;

public enum Sound
{
    Confirm,
    Return
}

// Gestisce gli effetti sonori della UI (pulsanti)
public class AudioSFX : MonoBehaviour, IPointerClickHandler
{
    AudioManager audioManager;

    [Header("Value")]
    public Sound value;

    [Header("Debug")]
    public bool hasAudio = false;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Audio").TryGetComponent(out audioManager))
        {
            hasAudio = true;
        }
        else Debug.Log("errore audio");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (hasAudio)
            switch (value)
            {
                case Sound.Confirm:
                    audioManager.PlaySFX(audioManager.buttonSounds[0]);
                    break;

                case Sound.Return:
                    audioManager.PlaySFX(audioManager.buttonSounds[1]);
                    break;
            }
    }
}
