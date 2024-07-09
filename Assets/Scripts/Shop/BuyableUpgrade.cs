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

    public Button button;
    private TextMeshProUGUI buttonText;
    public TextMeshProUGUI text;
    private string startText;
    private string startButtonText;
    public Image icon;

    void Start()
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        startButtonText = buttonText.text;

        if (text)

            startText = text.text;

        LoadUpgrade();


        if (text)
            text.text = startText + $"\n{upgradeCounter}/{upgradeLimit}";


        buttonText.text = startButtonText + $" [{cost}]";


    }

    public void BlockUpgrade()
    {
        if (button)
            button.interactable = false;

        if (upgradeCounter >= upgradeLimit) buttonText.text = "MAX";

        if (icon && button) icon.color = button.colors.disabledColor;
    }

    public void UpgradeCheck(bool approve = true)
    {
        if (upgradeCounter < upgradeLimit)
        {
            if (WalletManager.instance)
            {
                if (!WalletManager.instance.CanBuy(cost))
                    BlockUpgrade();
                else if (approve == true)
                {
                    Upgrade();
                }
            }
            else Debug.Log($"{id}:NESSUN WALLET");
        }
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

            if (WalletManager.instance)
                if (!WalletManager.instance.CanBuy(cost) || upgradeCounter >= upgradeLimit)
                {
                    Debug.Log($"{id}:Il caricamento ha bloccato l'upgrade");
                    BlockUpgrade();
                }

        }
        // else Debug.Log("Non ci sono upgrade precedenti per " + id.ToString());
    }

    public void OnDisable()
    {
        if (button)
            button.interactable = true;

        if (icon != null && buttonText && buttonText.text != "MAX") icon.color = Color.white;
    }

    public void OnEnable()
    {
        UpgradeCheck(false);
    }
}
