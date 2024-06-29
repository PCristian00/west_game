using UnityEngine;
using UnityEngine.UI;

public class BuyableUpgrade : MonoBehaviour
{
    public int cost;
    public int upgradeCounter = 0;
    public int upgradeLimit = 2;

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
        if (upgradeCounter < upgradeLimit)
            if (WalletManager.instance)
                if (!WalletManager.instance.CanBuy(cost))
                {
                    button.interactable = false;
                }
                else
                {
                    Upgrade();
                    if (!WalletManager.instance.CanBuy(cost))
                    {
                        button.interactable = false;
                    }
                }
            else Debug.Log("NESSUN WALLET");
        else button.interactable = false;
    }

    public void Upgrade()
    {
        WalletManager.instance.Buy(cost);

        cost += 10;
        upgradeCounter++;

        Debug.Log("counter " + upgradeCounter + " e nuovo costo a " + cost);

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
    }
}
