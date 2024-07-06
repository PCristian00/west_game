using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    private TextMeshProUGUI enemyInfo;

    private void Start()
    {
        enemyInfo = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.killObjective)
        {
            gameObject.SetActive(false);
        }
        else enemyInfo.text = GameManager.instance.KillGoalInfo();
    }
}
