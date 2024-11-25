using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(Instance);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetVolume();
    }

    public void SetVolume()
    {
        audioSource.volume = PlayerPrefsManager.GetVolume();
    }


}
