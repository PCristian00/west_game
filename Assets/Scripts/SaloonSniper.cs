using UnityEngine;

// Carica un modello esaminabile del fucile da cecchino nel Saloon (se il giocatore ha completato Factory).
public class SaloonSniper : MonoBehaviour
{
    void Start()
    {
        if (SaveManager.LoadInt(LoadoutManager.sniperKey) != 1) this.gameObject.SetActive(false);
    }
}
