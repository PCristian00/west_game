using UnityEngine;
using UnityEngine.UI;

public class UnlockManager : MonoBehaviour
{
    private Button[] levelButtons;

    private void OnEnable()
    {
        levelButtons = GetComponentsInChildren<Button>();
        foreach (Button levelButton in levelButtons)
        {
            // Debug.Log(levelButton.name);            
            levelButton.interactable = false;
        }

        int unlocked = SaveManager.LoadInt(GameManager.levelUnlockedKey, 1);

        if (unlocked > levelButtons.Length) unlocked = levelButtons.Length;

        for (int i = 0; i <= unlocked; i++)
        {
            if (levelButtons[i])
            {
                Debug.Log("unblocking " + levelButtons[i].name);
                levelButtons[i].interactable = true;
            }
        }
    }
}
