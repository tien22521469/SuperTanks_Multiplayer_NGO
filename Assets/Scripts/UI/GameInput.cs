using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameInput : NetworkBehaviour
{
    public static GameInput Instance { get; private set; }

    private PlayerInputAction playerInputActions;
    private Vector2 movementInput;
    private Vector2 rotationInput;

    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputAction();
        playerInputActions.Player.Enable();
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

