using UnityEngine;

public class ColorCrosshairOnHover : MonoBehaviour, IHover
{
    public void HoverEnter()
    {
        //Debug.Log("NEL MIRINO!");
        CrosshairManager.instance.ChangeColor(Color.red);
    }
    public void HoverExit()
    {
        CrosshairManager.instance.ResetColor();
    }

    private void OnDestroy()
    {
        HoverExit();
    }
}
