using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    // How quickly player moves forward and back
    private float speed = 10f;

    // How quickly player rotates (degrees per second)
    private float rotationSpeed = 180f;

    private float positionRange = 13f;
    private Rigidbody body;
    // Use this for initialization

    void Start()
    {
        // Retrieve reference to this GameObject's Rigidbody component
        body = GetComponent<Rigidbody>();
    }
    public override void OnNetworkSpawn()
    {
        transform.position = new Vector3(Random.Range(positionRange, -positionRange), 0f, Random.Range(positionRange, -positionRange));
        transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
    }
    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }


    //Xử lí di chuyển của player
    private void HandleMovement()
    {
        if (!IsOwner) return;
        if (!SuperTanksGameManager.Instance.IsGamePlaying()) return;
         // Check if GameInput.Instance is null
        if (GameInput.Instance == null)
        {
            Debug.LogError("GameInput.Instance is null");
            return;
        }
        // Get movement input value
        float movementInput = GameInput.Instance.GetMovementInput();
       
        // Determine amount to move based on current forward direction and speed
        Vector3 movement = transform.forward * movementInput * speed * Time.deltaTime;

        // Move our Rigidbody to this position
        body.MovePosition(body.position + movement);

        // Get rotation input value
        float rotationInput = GameInput.Instance.GetRotationInput();

        // Determine number of degrees to turn based on rotation speed
        float degreesToTurn = rotationInput * rotationSpeed * Time.deltaTime;

        // Get Quaternion equivalent of this amount of rotation around the y-axis
        Quaternion rotation = Quaternion.Euler(0f, degreesToTurn, 0f);

        // Rotate our Rigidbody by this amount
        body.MoveRotation(body.rotation * rotation);
    }
}