using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsCanvas : MonoBehaviour
{
    private const string SHOW_CONTROLS = "ShowControls";
    [SerializeField] private Animator controlsWindow;
    [SerializeField] private PlayerController playerController;

    private void Start()
    {
        if (playerController)
        {
            playerController.ShowControlsPressed += ShowControls;
        }
    }

    private void ShowControls(object sender, System.EventArgs e)
    {
        Debug.Log("ControlsCanvas Show!");
        if (controlsWindow)
        {
            bool val = controlsWindow.GetBool(SHOW_CONTROLS);
            controlsWindow.SetBool(SHOW_CONTROLS, !val);
        }
    }
}
