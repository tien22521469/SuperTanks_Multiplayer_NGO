using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{

    public static GameInput Instance { get; private set; }
    private PlayerInputAction playerInputActions;
    private Vector2 movementInput;
    private Vector2 rotationInput;

    private void Awake()
    {
        playerInputActions = new PlayerInputAction();

        // Bind actions to methods
        playerInputActions.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        playerInputActions.Player.Move.canceled += ctx => movementInput = Vector2.zero;

        playerInputActions.Player.Rotate.performed += ctx => rotationInput = ctx.ReadValue<Vector2>();
        playerInputActions.Player.Rotate.canceled += ctx => rotationInput = Vector2.zero;
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();
    }

    // Returns input values of 0, 1 or -1 based on whether Player tries to move forward or back
    public float GetMovementInput()
    {
        return movementInput.y;
    }

    // Returns input values of 0, 1 or -1 based on whether Player tries to rotate right or left
    public float GetRotationInput()
    {
        return rotationInput.x;
    }
}

