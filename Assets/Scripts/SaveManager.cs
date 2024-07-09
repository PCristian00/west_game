using UnityEngine;

public static class SaveManager
{
    public static int LoadInt(string key, int startValue = 0)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetInt(key);
        else
        {
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
            UpdateFloat(key, startValue);
            return startValue;
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
