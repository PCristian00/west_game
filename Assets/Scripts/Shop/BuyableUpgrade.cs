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

        int value = 50;
        float multiplier = 0.25f;

        switch (id)
        {
            case UpgradeType.Health:
                int upgradedHealth = SaveManager.LoadInt(PlayerManager.instance.healthKey);
                upgradedHealth += value;
                Debug.Log("Salvataggio di " + upgradedHealth + " in Prefs (VITA MAX)");
                SaveManager.UpdateInt(PlayerManager.instance.healthKey, upgradedHealth);
                break;

            case UpgradeType.Damage:
                float upgradedDamage = SaveManager.LoadFloat(PlayerManager.instance.damageKey);
                upgradedDamage += multiplier;
                Debug.Log("Salvataggio di " + upgradedDamage + " in Prefs (MULTIP. DANNI)");
                SaveManager.UpdateFloat(PlayerManager.instance.damageKey, upgradedDamage);
                break;

            case UpgradeType.Coin:
                float upgradedCoin = SaveManager.LoadFloat(PlayerManager.instance.coinKey);
                upgradedCoin += multiplier;
                Debug.Log("Salvataggio di " + upgradedCoin + " in Prefs (MULTIP. MONETE)");
                SaveManager.UpdateFloat(PlayerManager.instance.coinKey, upgradedCoin);
                break;
        }

        // Attivare quando upgrade max raggiunto
        // button.enabled = false;
    }
}
