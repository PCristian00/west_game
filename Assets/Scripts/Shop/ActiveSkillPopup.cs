using TMPro;
using UnityEngine;

public class ActiveSkillPopup : MonoBehaviour
{
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDesc;

    public void Setup(int id)
    {
        gameObject.SetActive(true);

        switch (id)
        {
            case 0:
                skillName.text = "Super Velocit�";
                skillDesc.text = "Aumenta la velocit� di spostamento";
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
}