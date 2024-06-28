using TMPro;
using UnityEngine;

public class WalletManager : MonoBehaviour
{
    public static WalletManager instance;
    public int wallet = 0;
    public GameObject[] coins;

    public TextMeshProUGUI walletInfo;

    void Start()
    {        
        instance = this;
        LoadWallet();
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

    public bool CanBuy(int cost)
    {
        if (wallet >= cost)
        {
            // wallet -= cost;

            return true;
        }
        return false;
    }

    public bool Buy(int cost)
    {
        if(!CanBuy(cost)) return false;

        wallet-=cost;
        UpdateWallet();

        return true;
    }
   
    public void LoadWallet()
    {
        if(PlayerPrefs.HasKey("wallet"))
        wallet = PlayerPrefs.GetInt("wallet");
        else
        {
            // wallet = 0;
            PlayerPrefs.SetInt("wallet",wallet);
        }
    }

    public void UpdateWallet()
    {
        PlayerPrefs.SetInt("wallet", wallet);
    }
}
