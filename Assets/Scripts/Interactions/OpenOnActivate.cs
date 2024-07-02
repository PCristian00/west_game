using UnityEngine;

public class OpenOnActivate : MonoBehaviour, IActivate
{

    public SaloonDoors doors;

    public void Activate()
    {
        doors.OpenDoors();
    }
}
