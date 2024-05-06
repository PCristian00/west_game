using UnityEngine;
using UnityEngine.UI;


public class CrosshairManager : MonoBehaviour
{
    public static CrosshairManager Instance;

    private Color startColor;

    private Color oldColor;

    private Image crosshair;

    public Color OldColor { get => oldColor; set => oldColor = value; }

    private void Start()
    {
        Instance = this;

        crosshair = GameObject.FindGameObjectWithTag("Crosshair").GetComponent<Image>();

        startColor = crosshair.color;

        // CurrentCrosshair = crosshairs[current];

    }

    public void ChangeColor(Color newColor)
    {
        oldColor = crosshair.color;
        crosshair.color = newColor;
    }

    public void ResetColor()
    {
        crosshair.color = startColor;
    }

    public void ChangeSprite(Sprite sprite)
    {
        crosshair.sprite = sprite;
    }
}