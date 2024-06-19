using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DescriptionManager : MonoBehaviour
{
    public static DescriptionManager Instance { get; private set; }

    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
       HideDescription();
    }

    public void ShowDescription(string title, string description)
    {
        Debug.Log("Mostrando descrizione");
        gameObject.SetActive(true);
        titleText.text = title;
        descriptionText.text = description;
        GameManager.instance.RequestInputBlock();
    }

    public void HideDescription()
    {
        Debug.Log("Provando a nascondere...");
        gameObject.SetActive(false);
        GameManager.instance.RequestInputUnblock();
    }
}
