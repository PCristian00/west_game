using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    public static CrosshairManager instance;

    private Color startColor;

    private Color oldColor;

    private Color currentColor;

    [SerializeField]
    private Image crosshair;

    public Color OldColor { get => oldColor; set => oldColor = value; }

    private void Start()
    {
        instance = this;

        crosshair = GameObject.FindGameObjectWithTag("Crosshair").GetComponent<Image>();
        if (crosshair)
            startColor = crosshair.color;
    }

    public void ChangeColor(Color newColor)
    {
        if (crosshair)
        {
            oldColor = crosshair.color;
            crosshair.color = newColor;
            currentColor = newColor;
        }

    }

    public void ResetColor()
    {

        if (crosshair)
        {
            oldColor = currentColor;
            crosshair.color = startColor;
            currentColor = startColor;
        }

    }

    public void ChangeSprite(Sprite sprite)
    {
        if (crosshair)
            crosshair.sprite = sprite;
    }

    public void EnemyOnCrosshair()
    {
        if (OldColor == Color.red)
        {
            ChangeColor(OldColor);
        }
        else
            ResetColor();
    }
}