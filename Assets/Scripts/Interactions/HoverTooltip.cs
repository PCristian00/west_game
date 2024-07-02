using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverTooltip : MonoBehaviour, IHover
{
    [SerializeField] private string tooltip;

    private bool tooltipVisible = false;
    public void HoverEnter()
    {
        tooltipVisible = true;
        TooltipManager.Instance.ShowTooltip(tooltip);
    }

    public void HoverExit()
    {
        tooltipVisible = false;
        TooltipManager.Instance.HideTooltip();
    }

    private void OnDestroy()
    {
        if (tooltipVisible)
        {
            HoverExit();
        }
    }
}
