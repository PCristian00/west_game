using UnityEngine;

public class OpenSaloonDoorsOnActivate : MonoBehaviour, IActivate
{

    public SaloonDoors doors;

    public void Activate()
    {
        doors.OpenDoors();
    }
}
