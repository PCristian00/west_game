using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TimerText : MonoBehaviour
{
    private TextMeshProUGUI _text;
    [SerializeField] private string formatString = @"mm\:ss";

    //private void Start()
    //{
    //    if(!GameManager.instance.timerObjective) gameObject.SetActive(false);
    //}

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        //if(GameManager.instance)
        //    if(!GameManager.instance.timerObjective) gameObject.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.instance.timer == 0)
        {
            gameObject.SetActive(false);
           // return;
        }

        TimeSpan ts = TimeSpan.FromSeconds(GameManager.instance.timer);
        _text.text =  ts.ToString( formatString);
    }
}
