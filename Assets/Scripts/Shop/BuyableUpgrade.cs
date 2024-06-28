using UnityEngine;
using UnityEngine.UI;

public class BuyableUpgrade : MonoBehaviour
{
    public int cost;

   // public int id;

    public enum UpgradeType
    {
        Damage,
        Health,
        Coin
    }

    public UpgradeType id;

    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void PriceCheck()
    {
        Debug.Log("CLICC");

        if (WalletManager.instance)
            if (!WalletManager.instance.CanBuy(cost))
            {
                button.enabled = false;
                Debug.Log("NON SI COMPRA");
            }
            else Upgrade();
        else Debug.Log("NESSUN WALLET");
    }

    public void Upgrade()
    {
        WalletManager.instance.Buy(cost);

        switch (id)
        {
            case UpgradeType.Health:
                // PlayerManager.instance.maxHealth += 50;
                SaveManager.UpdateInt(PlayerManager.instance.healthKey, 100);
                break;

            case UpgradeType.Damage:
                PlayerManager.instance.damageMultiplier += 1;
                break;

            case UpgradeType.Coin:
                PlayerManager.instance.coinMultiplier += 1;
                break;
        }

       // Attivare quando upgrade max raggiunto
       // button.enabled = false;
    }
}
