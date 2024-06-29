using TMPro;
using UnityEngine;

public class WalletManager : MonoBehaviour
{
    public static WalletManager instance;
    public float wallet = 0;
    public GameObject[] coins;

    public TextMeshProUGUI walletInfo;

    private readonly string saveKey = "wallet";

    void Start()
    {
        instance = this;
        wallet = SaveManager.LoadFloat(saveKey, wallet);
        walletInfo = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        walletInfo.text = wallet.ToString();
    }

    // Restituisce una moneta a caso tra i vari tipi (valori) disponibili
    public GameObject DropCoin()
    {
        return coins[Random.Range(0, coins.Length)];
    }

    public bool CanBuy(float cost)
    {
        if (wallet >= cost)
        {
            // wallet -= cost;

            return true;
        }
        return false;
    }

    public bool Buy(float cost)
    {
        if (!CanBuy(cost)) return false;

        wallet -= cost;
        SaveManager.UpdateFloat(saveKey, wallet);

        return true;
    }
}
