using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionHighlight : MonoBehaviour, IHover
{
    private Material _material;

    public Color highlightColor;
    
    
    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
    }
    
    
    public void HoverEnter()
    {
        _material.EnableKeyword("_EMISSION");
        _material.SetColor("_EmissionColor",highlightColor);
        
    }

    public void HoverExit()
    {
        _material.DisableKeyword("_EMISSION");
        _material.SetColor("_EmissionColor",Color.black);
    }
}
