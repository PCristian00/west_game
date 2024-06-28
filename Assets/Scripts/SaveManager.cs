using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveManager
{
    

    public static int LoadInt(string key)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetInt(key);
        else
        {
            // wallet = 0;
            UpdateInt(key, 0);
            return 0;
        }
    }

    public static void UpdateInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }
}
