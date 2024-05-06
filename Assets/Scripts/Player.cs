using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;

    // How quickly player moves forward and back
    private float speed = 10f;

    // How quickly player rotates (degrees per second)

    private float rotationSpeed = 180f;

    private Rigidbody body;
	// Use this for initialization
	void Start ()
    {
        // Retrieve reference to this GameObject's Rigidbody component
        body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        HandleMovement();

	}


    //Sử lí di chuyển của player
    private void HandleMovement()
    {
        if(!SuperTanksGameManager.Instance.IsGamePlaying()) return;
        // Get movement input value
        float v = gameInput.GetMovementInput();
        float movementInput = v;

        // Determine amount to move based on current forward direction and speed
        Vector3 movement = transform.forward * movementInput * speed * Time.deltaTime;

        // Move our Rigidbody to this position
        body.MovePosition(body.position + movement);

        // Get rotation input value
        float rotationInput = gameInput.GetRotationInput();

        // Determine number of degrees to turn based on rotation speed
        float degreesToTurn = rotationInput * rotationSpeed * Time.deltaTime;

        // Get Quaternion equivalent of this amount of rotation around the y-axis
        Quaternion rotation = Quaternion.Euler(0f, degreesToTurn, 0f);

        // Rotate our Rigidbody by this amount
        body.MoveRotation(body.rotation * rotation);
    }    
}