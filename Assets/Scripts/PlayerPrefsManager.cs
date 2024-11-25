using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    private static string VOLUME = "Volume";

    public static void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat(VOLUME, volume);
    }

    public static float GetVolume()
    {
        return PlayerPrefs.GetFloat(VOLUME, 1f);
    }
}
