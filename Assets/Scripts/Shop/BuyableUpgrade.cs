using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum UpgradeType
{
    Damage,
    Health,
    Coin
}

public class BuyableUpgrade : MonoBehaviour
{
    public int cost;
    public int upgradeCounter = 0;
    public int upgradeLimit = 2;
    public UpgradeType id;

    public UnityEngine.UI.Button button;
    private TextMeshProUGUI buttonText;
    public TextMeshProUGUI text;
    private string startText;
    private string startButtonText;
    public Image icon;

    // Start is called before the first frame update
    void Start()
    {


        button = GetComponent<UnityEngine.UI.Button>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        startButtonText = buttonText.text;

        if (text)
        {
            startText = text.text;
            text.text = startText + $"\n{upgradeCounter}/{upgradeLimit}";
        }

        buttonText.text = startButtonText + $" [{cost}]";

        LoadUpgrade();
    }

    // AGGIUNGERE QUI FUNZIONI PER SCURIRE ICONA O ALTRO
    public void BlockUpgrade()
    {
        button.interactable = false;

        if (upgradeCounter >= upgradeLimit) buttonText.text = "MAX";

        // IMPOSTARE COLORE DIVERSO
        if (icon) icon.color = Color.red;
    }

    public void UpgradeCheck()
    {
        if (upgradeCounter < upgradeLimit)
            if (WalletManager.instance)
                if (!WalletManager.instance.CanBuy(cost))
                    BlockUpgrade();
                else
                {
                    Upgrade();
                }
            else Debug.Log("NESSUN WALLET");
        else
            BlockUpgrade();
    }

    public void Upgrade()
    {
        WalletManager.instance.Buy(cost);

        upgradeCounter++;
        cost += 10;

        if (text) text.text = startText + $"\n{upgradeCounter}/{upgradeLimit}";
        buttonText.text = startButtonText + $" [{cost}]";


        Debug.Log("counter " + upgradeCounter + " e nuovo costo a " + cost);

        int value = 50;
        float multiplier = 0.25f;

        switch (id)
        {
            case UpgradeType.Health:
                int upgradedHealth = SaveManager.LoadInt(PlayerManager.instance.healthKey);
                upgradedHealth += value;
                // Debug.Log("Salvataggio di " + upgradedHealth + " in Prefs (VITA MAX)");
                SaveManager.UpdateInt(PlayerManager.instance.healthKey, upgradedHealth);
                break;

            case UpgradeType.Damage:
                float upgradedDamage = SaveManager.LoadFloat(PlayerManager.instance.damageKey);
                upgradedDamage += multiplier;
                // Debug.Log("Salvataggio di " + upgradedDamage + " in Prefs (MULTIP. DANNI)");
                SaveManager.UpdateFloat(PlayerManager.instance.damageKey, upgradedDamage);
                break;

            case UpgradeType.Coin:
                float upgradedCoin = SaveManager.LoadFloat(PlayerManager.instance.coinKey);
                upgradedCoin += multiplier;
                // Debug.Log("Salvataggio di " + upgradedCoin + " in Prefs (MULTIP. MONETE)");
                SaveManager.UpdateFloat(PlayerManager.instance.coinKey, upgradedCoin);
                break;
        }

        SaveUpgrade();

        if (!WalletManager.instance.CanBuy(cost) || upgradeCounter >= upgradeLimit)
            BlockUpgrade();
    }

    public void SaveUpgrade()
    {
        string key = id.ToString() + "_upgrade_level";
        PlayerPrefs.SetInt(key, upgradeCounter);

       // Debug.Log("Salvato in Prefs upgrade a " + id.ToString());
    }

    public void LoadUpgrade()
    {
        string key = id.ToString() + "_upgrade_level";

        if (PlayerPrefs.HasKey(key))
        {
            upgradeCounter = PlayerPrefs.GetInt(key);

            cost += 10 * upgradeCounter;

            if (text) text.text = startText + $"\n{upgradeCounter}/{upgradeLimit}";
            buttonText.text = startButtonText + $" [{cost}]";

            if (!WalletManager.instance.CanBuy(cost) || upgradeCounter >= upgradeLimit)
                BlockUpgrade();
        }
        // else Debug.Log("Non ci sono upgrade precedenti per " + id.ToString());
    }
}
