using TMPro;
using UnityEngine;

public class ActiveSkillPopup : MonoBehaviour
{
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDesc;

    public int skillID;

    public int cost = 50;

    public UnityEngine.UI.Button buyButton;
    public UnityEngine.UI.Button equipButton;
    private TextMeshProUGUI buyButtonText;
    private TextMeshProUGUI equipButtonText;

    private readonly string startBuyButtonText = "Compra";
    private readonly string startEquipButtonText = "Equipaggia";

    private static readonly string[] skillKeys = { "superspeed", "double_jump", "shield", "slowmo" };

    // Ogni indice punta ad una skill diversa.
    // Valore 0: skill non acquistata
    // Valore 1: skill acquistata
    // (DA IMPLEMENTARE) Valore 2: skill equipaggiata
    public static int[] state = { 0, 0, 0, 0 };

    public void SetupBuyButton()
    {
        buyButtonText = buyButton.GetComponentInChildren<TextMeshProUGUI>();
        // startBuyButtonText = buyButtonText.text;
        buyButtonText.text = startBuyButtonText + $" [{cost}]";
        buyButton.interactable = true;
        buyButton.gameObject.SetActive(true);

        equipButton.gameObject.SetActive(false);
    }

    public void SetupEquipButton()
    {
        equipButtonText = equipButton.GetComponentInChildren<TextMeshProUGUI>();
        // startBuyButtonText = buyButtonText.text;
        equipButtonText.text = startEquipButtonText;
        equipButton.interactable = true;
        equipButton.gameObject.SetActive(true);
    }

    public void Setup(int id)
    {
        LoadSkillStates();

        gameObject.SetActive(true);

        SetupBuyButton();




        skillID = id;

        if (state[skillID] == 1) BlockBuy();

        Debug.Log("Setup, skill id " + skillID);

        switch (id)
        {
            case 0:
                skillName.text = "Super Velocità";
                skillDesc.text = "Aumenta la velocità di spostamento";
                // skillKey = "superspeed";
                break;
            case 1:
                skillName.text = "Doppio Salto";
                skillDesc.text = "Permette di eseguire il doppio salto";
                // skillKey = "double_jump";
                break;
            case 2:
                skillName.text = "Scudo";
                skillDesc.text = "Attiva uno scudo che ti rende invulnerabile";
                // skillKey = "shield";
                break;
            case 3:
                skillName.text = "Slow Motion";
                skillDesc.text = "Rallenta i nemici";
                // skillKey = "slowmo";
                break;
        }
    }

    public void BuyCheck()
    {
        if (WalletManager.instance)
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

    public void EquipCheck()
    {
        if (state[skillID] == 2) BlockEquip();

        state[skillID] = 2;

        for (int i = 0; i < state.Length; i++)
            if (state[i] == 2 && i != skillID) state[i] = 1;

        SaveSkillStates();

        BlockEquip();
    }

    public void BlockEquip()
    {


        equipButtonText.text = "EQUIPAGGIATO";
        equipButton.interactable = false;

    }
    // PER NON USARE LOADOUT, INSERIRE QUI QUALCHE FUNZIONE CHE EQUIPAGGIA SENZA SPENDERE DENARO?
    public void BlockBuy()
    {
        // Debug.Log("Già comprato");
        buyButton.interactable = false;

        if (state[skillID] == 1)
        {
            buyButton.gameObject.SetActive(false);
            buyButtonText.text = "COMPRATO";

            SetupEquipButton();
        }
    }

    public void Buy()
    {
        // Debug.Log("COMPRATO");
        state[skillID] = 1;

        WalletManager.instance.Buy(cost);

        BlockBuy();

        SaveSkillState();

    }

    // AGGIUNGERE FUNZIONE DI EQUIPAGGIAMENTO
    // La funzione di equipaggiamento imposta state[SkillID] a 2
    // Deve impostare anche tutti gli altri state a 1 se ve ne sono alcuni a 2

    public void SaveSkillState()
    {
        PlayerPrefs.SetInt(skillKeys[skillID], state[skillID]);
    }

    public void LoadSkillStates()
    {
        for (int i = 0; i < skillKeys.Length; i++)
        {
            state[i] = SaveManager.LoadInt(skillKeys[i]);
        }
    }

    public void SaveSkillStates()
    {
        for (int i = 0; i < skillKeys.Length; i++)
        {
            PlayerPrefs.SetInt(skillKeys[i], state[i]);
        }
    }
}
