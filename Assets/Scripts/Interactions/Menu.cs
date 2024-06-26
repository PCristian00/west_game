using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void Start()
    {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        if (GameManager.instance != null)
            GameManager.instance.RequestInputBlock();
    }

    private void OnDisable()
    {
        if (GameManager.instance != null)
            GameManager.instance.RequestInputUnblock();
    }
}
