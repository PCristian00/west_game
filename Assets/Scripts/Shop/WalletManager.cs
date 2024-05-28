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
}
