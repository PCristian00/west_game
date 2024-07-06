using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TimerText : MonoBehaviour
{
    private TextMeshProUGUI _text;
    [SerializeField] private string formatString = @"mm\:ss";

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        TimeSpan ts = TimeSpan.FromSeconds(GameManager.instance.timer);
        _text.text =  ts.ToString( formatString);
    }
}
