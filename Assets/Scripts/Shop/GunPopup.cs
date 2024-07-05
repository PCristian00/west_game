using TMPro;
using UnityEngine;

public class GunPopup : MonoBehaviour
{
    public TextMeshProUGUI gunName;
    public TextMeshProUGUI gunDesc;

    public int gunID;

    public int cost = 100;

    public UnityEngine.UI.Button buyButton;
    public UnityEngine.UI.Button equipButton;
    private TextMeshProUGUI buyButtonText;
    private TextMeshProUGUI equipButtonText;

    private readonly string startBuyButtonText = "Compra";
    private readonly string startEquipButtonText = "Equipaggia";

    private static readonly string[] gunKeys = { "revolver", "assault", "rocket" };

    // Ogni indice punta ad un'arma diversa.
    // Valore 0: arma non acquistata
    // Valore 1: arma acquistata
    // Valore 2: arma equipaggiata
    public static int[] states = { 0, 0, 0 };

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

        if (states[gunID] == 2) BlockEquip();
    }

    public void Setup(int id)
    {
        LoadGunStates();

        gameObject.SetActive(true);

        SetupBuyButton();

        gunID = id;

        BuyCheck(false);

        if (states[gunID] >= 1) BlockBuy();

        switch (id)
        {
            case 0:
                gunName.text = "Revolver Migliorato";
                gunDesc.text = "Più danni, caricatore più capiente";

                break;
            case 1:
                gunName.text = "Fucile d'assalto";
                gunDesc.text = "Grande velocità di fuoco";

                break;
            case 2:
                gunName.text = "Lanciarazzi";
                gunDesc.text = "Proiettili esplosivi con danno ad area";

                break;
            
        }
    }

    public void BuyCheck(bool approve = true)
    {
        if (WalletManager.instance)
            if (!WalletManager.instance.CanBuy(cost))
            {
                BlockBuy();
                Debug.Log("FONDI INSUFFICIENTI");
            }
            else
            {
                if(approve)
                Buy();
            }
        else Debug.Log("NESSUN WALLET");
    }

    public void Buy()
    {
        states[gunID] = 1;

        WalletManager.instance.Buy(cost);

        BlockBuy();
        SaveGunState();
    }

    public void BlockBuy()
    {
        buyButton.interactable = false;

        if (states[gunID] >= 1)
        {
            buyButton.gameObject.SetActive(false);
            buyButtonText.text = "COMPRATO";

            SetupEquipButton();
        }
    }

    public void EquipCheck()
    {
        if (states[gunID] == 2) BlockEquip();

        states[gunID] = 2;

        // PARTE CHE PERMETTE DI EQUIPAGGIARE SOLO 1

        //for (int i = 0; i < states.Length; i++)
        //    if (states[i] == 2 && i != gunID) states[i] = 1;

        SaveGunStates();
        BlockEquip();
    }

    public void BlockEquip()
    {
        equipButtonText.text = "EQUIPAGGIATO";
        equipButton.interactable = false;

    }

    public void SaveGunState()
    {
        PlayerPrefs.SetInt(gunKeys[gunID], states[gunID]);
    }

    public static void LoadGunStates()
    {
        for (int i = 0; i < gunKeys.Length; i++)
        {
            states[i] = SaveManager.LoadInt(gunKeys[i]);
        }
    }

    public void SaveGunStates()
    {
        for (int i = 0; i < gunKeys.Length; i++)
        {
            PlayerPrefs.SetInt(gunKeys[i], states[i]);
        }
    }
}
