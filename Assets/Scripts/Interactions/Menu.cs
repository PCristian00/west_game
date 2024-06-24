using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        GameManager.instance.RequestInputBlock();
    }

    private void OnDisable()
    {
        GameManager.instance.RequestInputUnblock();
    }
}
