using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    public static CrosshairManager Instance;

    private Color startColor;

    private Color oldColor;

    private Color currentColor;

    [SerializeField]
    private Image crosshair;

    public Color OldColor { get => oldColor; set => oldColor = value; }

    private void Start()
    {
        Debug.Log("CrosshairManager avviato");
        Instance = this;
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
            Debug.Log("Puntavo un nemico...");
            CrosshairManager.Instance.ChangeColor(OldColor);
        }
        else
            CrosshairManager.Instance.ResetColor();
    }
}