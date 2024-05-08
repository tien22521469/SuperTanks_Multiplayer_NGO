using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : NetworkBehaviour
{
    public static GameInput Instance { get; private set; }

    private PlayerInputAction playerInputActions;
    private Vector2 movementInput;
    private Vector2 rotationInput;
    public event EventHandler OnPauseAction;
    public event EventHandler HaveMissileAction;
    public event EventHandler MoveAction;
    public event EventHandler RotationAction;
    public event EventHandler OnBindingRebind;
    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputAction();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Pause.performed += Pause_performed;
        playerInputActions.Player.HaveMissile.performed += HaveMissile_performed;

    }


    private void OnDestroy()
    {

        playerInputActions.Player.Pause.performed -= Pause_performed;
        playerInputActions.Dispose();
    }
    private void Pause_performed(InputAction.CallbackContext context)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }
    
    private void HaveMissile_performed(InputAction.CallbackContext context)
    {
        HaveMissileAction?.Invoke(this, EventArgs.Empty);
    }
    
    private void Rebinding(InputAction.CallbackContext context)
    {
        OnBindingRebind?.Invoke(this, EventArgs.Empty);
    }
    


    // Returns input values of 0, 1 or -1 based on whether Player tries to move forward or back
    public float GetMovementInput()
    {
        // Bind actions to methods
        playerInputActions.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        playerInputActions.Player.Move.canceled += ctx => movementInput = Vector2.zero;
        return movementInput.y;
    }

    // Returns input values of 0, 1 or -1 based on whether Player tries to rotate right or left
    public float GetRotationInput()
    {
        playerInputActions.Player.Rotate.performed += ctx => rotationInput = ctx.ReadValue<Vector2>();
        playerInputActions.Player.Rotate.canceled += ctx => rotationInput = Vector2.zero;
        return rotationInput.x;
    }

    public KeyCode GetMissileInput()
    {
        return playerInputActions.Player.HaveMissile.triggered ? KeyCode.Space : KeyCode.None;
    }
}

