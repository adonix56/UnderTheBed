using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    public event EventHandler FlashlightStarted;
    public event EventHandler FlashlightEnded;
    public event EventHandler JumpPressed;
    public event EventHandler InteractPressed;
    public event EventHandler ShowControlsPressed;

    [SerializeField] private LoadingCanvas loadingCanvas;

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();

        //playerControls.Player.Flashlight.performed += FlashlightPerformed;
        playerControls.Player.Flashlight.started += FlashlightStart;
        playerControls.Player.Flashlight.canceled += FlashlightEnd;
        playerControls.Player.Jump.performed += JumpPerformed;
        playerControls.Player.Interact.performed += InteractPerformed;
        playerControls.Player.ShowControls.performed += ShowControlsPerformed;
        playerControls.Player.ExitGame.performed += ExitGamePerformed;
    }

    private void Start()
    {
        loadingCanvas.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void FlashlightStart(InputAction.CallbackContext obj)
    {
        FlashlightStarted?.Invoke(this, EventArgs.Empty);
    }

    private void FlashlightEnd(InputAction.CallbackContext obj)
    {
        FlashlightEnded?.Invoke(this, EventArgs.Empty);
    }

    private void JumpPerformed(InputAction.CallbackContext obj)
    {
        JumpPressed?.Invoke(this, EventArgs.Empty);
    }

    private void InteractPerformed(InputAction.CallbackContext obj)
    {
        InteractPressed?.Invoke(this, EventArgs.Empty);
    }

    private void ShowControlsPerformed(InputAction.CallbackContext obj)
    {
        ShowControlsPressed?.Invoke(this, EventArgs.Empty);
    }

    private void ExitGamePerformed(InputAction.CallbackContext obj) {
        Loader.SetupLoadScene(Loader.SceneName.MainMenu);
        if (loadingCanvas)
        {
            loadingCanvas.SetupLoadScene(Loader.SceneName.MainMenu);
        }
    }

    public float GetMoveInput()
    {
        return playerControls.Player.Move.ReadValue<float>();
    }

    public Vector2 GetFlashlightAim()
    {
        return playerControls.Player.FlashlightAim.ReadValue<Vector2>();
    }
}
