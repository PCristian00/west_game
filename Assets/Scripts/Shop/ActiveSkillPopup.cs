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
    // Valore 2: skill equipaggiata (SOLO UNA PER ARRAY)
    public static int[] states = { 0, 0, 0, 0 };

    public void SetupBuyButton()
    {
        buyButtonText = buyButton.GetComponentInChildren<TextMeshProUGUI>();
        buyButtonText.text = startBuyButtonText + $" [{cost}]";
        buyButton.interactable = true;
        buyButton.gameObject.SetActive(true);

        equipButton.gameObject.SetActive(false);
    }

    public void SetupEquipButton()
    {
        equipButtonText = equipButton.GetComponentInChildren<TextMeshProUGUI>();
        equipButtonText.text = startEquipButtonText;
        equipButton.interactable = true;
        equipButton.gameObject.SetActive(true);

        if (states[skillID] == 2) BlockEquip();
    }

    public void Setup(int id)
    {
        LoadSkillStates();

        gameObject.SetActive(true);

        SetupBuyButton();

        skillID = id;

        if (states[skillID] >= 1) BlockBuy();

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

    public void Buy()
    {
        states[skillID] = 1;

        WalletManager.instance.Buy(cost);

        BlockBuy();
        SaveSkillState();
    }

    public void BlockBuy()
    {
        buyButton.interactable = false;

        if (states[skillID] >= 1)
        {
            buyButton.gameObject.SetActive(false);
            buyButtonText.text = "COMPRATO";

            SetupEquipButton();
        }
    }

    public void EquipCheck()
    {
        if (states[skillID] == 2) BlockEquip();

        states[skillID] = 2;

        for (int i = 0; i < states.Length; i++)
            if (states[i] == 2 && i != skillID) states[i] = 1;

        SaveSkillStates();
        BlockEquip();
    }

    public void BlockEquip()
    {
        equipButtonText.text = "EQUIPAGGIATO";
        equipButton.interactable = false;

    }

    public void SaveSkillState()
    {
        PlayerPrefs.SetInt(skillKeys[skillID], states[skillID]);
    }

    public static void LoadSkillStates()
    {
        for (int i = 0; i < skillKeys.Length; i++)
        {
            states[i] = SaveManager.LoadInt(skillKeys[i]);
        }
    }

    public void SaveSkillStates()
    {
        for (int i = 0; i < skillKeys.Length; i++)
        {
            PlayerPrefs.SetInt(skillKeys[i], states[i]);
        }
    }
}
