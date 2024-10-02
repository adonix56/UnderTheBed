using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    public event EventHandler JumpPressed;
    public event EventHandler InteractPressed;
    public event EventHandler ShowControlsPressed;
    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();

        playerControls.Player.Jump.performed += JumpPerformed;
        playerControls.Player.Interact.performed += InteractPerformed;
        playerControls.Player.ShowControls.performed += ShowControlsPerformed;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
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

    public float GetMoveInput()
    {
        return playerControls.Player.Move.ReadValue<float>();
    }
}
