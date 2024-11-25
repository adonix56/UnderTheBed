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
    [SerializeField] private Options options;

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
        if (GameManager.CurrentGameState != GameManager.GameState.Paused)
            FlashlightStarted?.Invoke(this, EventArgs.Empty);
    }

    private void FlashlightEnd(InputAction.CallbackContext obj)
    {
        if (GameManager.CurrentGameState != GameManager.GameState.Paused)
            FlashlightEnded?.Invoke(this, EventArgs.Empty);
    }

    private void JumpPerformed(InputAction.CallbackContext obj)
    {
        if (GameManager.CurrentGameState != GameManager.GameState.Paused)
            JumpPressed?.Invoke(this, EventArgs.Empty);
    }

    private void InteractPerformed(InputAction.CallbackContext obj)
    {
        if (GameManager.CurrentGameState != GameManager.GameState.Paused)
            InteractPressed?.Invoke(this, EventArgs.Empty);
    }

    private void ShowControlsPerformed(InputAction.CallbackContext obj)
    {
        if (GameManager.CurrentGameState != GameManager.GameState.Paused)
            ShowControlsPressed?.Invoke(this, EventArgs.Empty);
    }

    private void ExitGamePerformed(InputAction.CallbackContext obj) {
        options.ToggleOptions(true);
        /*Loader.SetupLoadScene(Loader.SceneName.MainMenu);
        if (loadingCanvas)
        {
            loadingCanvas.SetupLoadScene(Loader.SceneName.MainMenu);
        }*/
    }

    public float GetMoveInput()
    {
        return GameManager.CurrentGameState == GameManager.GameState.Paused ? 0.0f : playerControls.Player.Move.ReadValue<float>();
    }

    public Vector2 GetFlashlightAim()
    {
        return playerControls.Player.FlashlightAim.ReadValue<Vector2>();
    }
}
