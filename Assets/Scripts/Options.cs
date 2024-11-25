using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    private const string VOLUME = "Volume";
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private GameObject acknowledge;
    [SerializeField] private LoadingCanvas loadingCanvas;
    [SerializeField] private Slider volumeSlider;

    public bool OptionsActive { get; private set; }

    private void Start()
    {
        OptionsActive = false;
        volumeSlider.value = PlayerPrefsManager.GetVolume();
    }

    public void ToggleOptions(bool withQuit)
    {
        OptionsActive = !OptionsActive;
        optionsMenu.SetActive(OptionsActive);
        if (withQuit) { quitButton.SetActive(OptionsActive); }
        if (!OptionsActive)
        {
            GameManager.Instance.UnPause();
        } else
        {
            GameManager.Instance.Pause();
        }
    }

    public void ToggleAcknowledge() { 
        acknowledge.SetActive(!acknowledge.activeSelf);
    }

    public void QuitToMain() { 
        GameManager.Instance.ToMainMenu();
        loadingCanvas.SetupLoadScene(Loader.SceneName.MainMenu);
    }

    public void SetVolume()
    {
        PlayerPrefsManager.SetVolume(volumeSlider.value);
        MusicManager.Instance.SetVolume();
    }
}
