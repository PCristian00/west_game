using UnityEngine;

public static class SaveManager
{   
    public static int LoadInt(string key, int startValue = 0)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetInt(key);
        else
        {
            // wallet = 0;
            UpdateInt(key, startValue);
            return startValue;
        }
    }

    public static float LoadFloat(string key, float startValue = 1f)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetFloat(key);
        else
        {
            // wallet = 0;
            UpdateFloat(key, startValue);
            return startValue;
        }
    }

    // Superflua
    public static void UpdateInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    // Superflua
    public static void UpdateFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    // Superflua
    public static void ResetAll()
    {
        PlayerPrefs.DeleteAll();
    }
}
