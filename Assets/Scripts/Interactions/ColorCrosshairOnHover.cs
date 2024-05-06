using UnityEngine;

public class ColorCrosshairOnHover : MonoBehaviour, IHover
{
    public void HoverEnter()
    {
        //Debug.Log("NEL MIRINO!");
        CrosshairManager.Instance.ChangeColor(Color.red);
    }
    public void HoverExit()
    {
        CrosshairManager.Instance.ResetColor();
    }

    private void OnDestroy()
    {
        HoverExit();
    }
}
