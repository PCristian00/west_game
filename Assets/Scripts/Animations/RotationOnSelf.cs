using UnityEngine;

public class RotationOnSelf : MonoBehaviour
{

    public Vector3 rotationAxis = new Vector3(0f, 100f, 0f);

    void Update()
    {
        transform.Rotate(rotationAxis * Time.deltaTime);
    }
}
