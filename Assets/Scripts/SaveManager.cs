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

    public static float LoadFloat(string key)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetFloat(key);
        else
        {
            // wallet = 0;
            UpdateFloat(key, 1);
            return 1;
        }
    }

    public static void UpdateInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public static void UpdateFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    public static void ResetAll()
    {
        PlayerPrefs.DeleteAll();
    }
}
