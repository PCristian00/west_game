using UnityEngine;

public class RotationOnSelf : MonoBehaviour
{

    public Vector3 rotationAxis = new(0f, 100f, 0f);
    public float speed = 1f;

    void Update()
    {
        transform.Rotate(speed * Time.deltaTime * rotationAxis);
    }
}
