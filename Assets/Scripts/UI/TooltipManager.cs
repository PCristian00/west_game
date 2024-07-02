using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance { get; private set; }

    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private TextMeshProUGUI tooltipText;
    

    private void Awake()
    {
        Instance = this;
        HideTooltip();
    }

    private void Start()
    {
       GameManager.instance.OnInputActiveChanged.AddListener((active) => gameObject.SetActive(active) );
    }

    public void ShowTooltip(string tooltip)
    {
        if (string.IsNullOrWhiteSpace(tooltip))
        {
            HideTooltip();
            return;
        }
        
        tooltipPanel.SetActive(true);
        tooltipText.text = tooltip;
    }

    public void HideTooltip()
    {
        if(tooltipPanel)
        tooltipPanel.SetActive(false);
    }
    
}
