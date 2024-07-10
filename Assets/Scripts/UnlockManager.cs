using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockManager : MonoBehaviour
{
    // Start is called before the first frame update

    private Button[] levelButtons;
    void Start()
    {
        levelButtons = GetComponentsInChildren<Button>();

        foreach (Button levelButton in levelButtons)
        {
            Debug.Log(levelButton.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
