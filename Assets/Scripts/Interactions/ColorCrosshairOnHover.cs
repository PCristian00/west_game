using UnityEngine;
using UnityEngine.UI;

public class ColorCrosshairOnHover : MonoBehaviour, IHover
{

    // public Color color;
  //  private Image crosshair;
    // TROVARE MODO PER COLORE ORIGINALE MIRINO
    // private Color crosshairColor;

    // TROVARE MODO PER RELOAD (Vedi ProjectileGun)

    //private void Start()
    //{
    //   // crosshair = GameObject.FindGameObjectWithTag("Crosshair").GetComponent<Image>();
    //    // crosshairColor = crosshair.color;
    //}
    public void HoverEnter()
    {
        //Debug.Log("NEL MIRINO!");
        CrosshairManager.Instance.ChangeColor(Color.red);
    }

    public void HoverExit()
    {
        CrosshairManager.Instance.ResetColor();
        //Debug.Log("FUORI DA MIRINO!");
        //crosshair.color = crosshairColor;
    }

    private void OnDestroy()
    {
        HoverExit();
    }

    //private void ColorReset()
    //{
    //    if (crosshair != null)
    //        crosshair.color = Color.white;
    //}
}
