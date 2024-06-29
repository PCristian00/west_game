using TMPro;
using UnityEngine;

public class ActiveSkillPopup : MonoBehaviour
{
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDesc;

    public int skillID;

    public int cost = 50;

    public UnityEngine.UI.Button buyButton;
    private TextMeshProUGUI buyButtonText;
    // public TextMeshProUGUI text;
    // private string startText;
    private string startButtonText = "Compra";

    public static int[] bought =
    {
        0, 0, 0, 0
    };

    public void Start()
    {




    }

    public void Setup(int id)
    {


        gameObject.SetActive(true);

        buyButton.interactable = true;
        

        buyButtonText = buyButton.GetComponentInChildren<TextMeshProUGUI>();
        // startButtonText = buyButtonText.text;
        buyButtonText.text = startButtonText + $" [{cost}]";

        skillID = id;

        if (bought[skillID] == 1) BlockBuy();

        Debug.Log("Setup, skill id " + skillID);

        switch (id)
        {
            case 0:
                skillName.text = "Super Velocità";
                skillDesc.text = "Aumenta la velocità di spostamento";
                break;
            case 1:
                skillName.text = "Doppio Salto";
                skillDesc.text = "Permette di eseguire il doppio salto";
                break;
            case 2:
                skillName.text = "Scudo";
                skillDesc.text = "Attiva uno scudo che ti rende invulnerabile";
                break;
            case 3:
                skillName.text = "Slow Motion";
                skillDesc.text = "Rallenta i nemici";
                break;
        }
    }

    public void BuyCheck()
    {

        Debug.Log(" Comprando skill id =" + skillID);

        if (bought[skillID] == 1) BlockBuy();
        else if (WalletManager.instance)
            if (!WalletManager.instance.CanBuy(cost))
            {
                BlockBuy();
                Debug.Log("FONDI INSUFFICIENTI");
            }
            else
            {
                Buy();
            }
        else Debug.Log("NESSUN WALLET");
    }

    // PER NON USARE LOADOUT, INSERIRE QUI QUALCHE FUNZIONE CHE EQUIPAGGIA SENZA SPENDERE DENARO?
    public void BlockBuy()
    {
        Debug.Log("Già comprato");
        buyButton.interactable = false;
        if (bought[skillID] == 1) buyButtonText.text = "COMPRATO";
    }

    public void Buy()
    {
        Debug.Log("COMPRATO");
        bought[skillID] = 1;
        
        WalletManager.instance.Buy(cost);

        BlockBuy();

    }
}