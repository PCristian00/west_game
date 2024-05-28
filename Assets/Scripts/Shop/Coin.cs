using UnityEngine;



public class Coin : MonoBehaviour
{
    public int value = 5;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, 100f, 0f) * Time.deltaTime);
    }
}
