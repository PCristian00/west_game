using UnityEngine;

public class OpenOnActivate : MonoBehaviour, IActivate
{
   // private TransformAnimation _transformAnimation;

    public SaloonDoors doors;

    public void Activate()
    {
        doors.OpenDoors();
    }
}
