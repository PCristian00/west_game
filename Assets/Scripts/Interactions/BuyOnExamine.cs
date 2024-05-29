using UnityEngine;

public class BuyOnExamine : MonoBehaviour, IExamine
{
    public int cost = 5;
    public bool bought = false;

    private EmissionHighlight emission;

    public void Start()
    {
        emission = GetComponent<EmissionHighlight>();
    }
    public void Examine()
    {
        if (!bought)
            if (WalletManager.instance.BuyItem(cost))
            {
                ApplyEffect();

                gameObject.transform.localScale -= new Vector3(1, 0, 0);
                bought = true;

                emission.HoverExit();
                emission.highlightColor = Color.red;
            }
            else Debug.Log("Fondi insufficienti.");
        else Debug.Log("Già comprato");
    }

    public void ApplyEffect()
    {
        PlayerMovement.instance.doubleJumpActive = true;
        PlayerManager.instance.health += 30;
    }
}
