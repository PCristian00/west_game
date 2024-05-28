using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WalletManager : MonoBehaviour
{
    public static WalletManager instance;
    public int wallet = 0;

    public TextMeshProUGUI walletInfo;
    // Start is called before the first frame update
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
}
