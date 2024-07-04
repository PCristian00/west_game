using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthInfo : MonoBehaviour
{
    public TextMeshProUGUI healthInfo;
    // Start is called before the first frame update
    void Start()
    {
        healthInfo = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        healthInfo.SetText("Health: " + PlayerManager.instance.health);
    }
}
