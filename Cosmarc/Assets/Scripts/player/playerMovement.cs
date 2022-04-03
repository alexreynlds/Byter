using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    public InputMaster controls;
    public float moveSpeed = 5f;

    public Rigidbody2D rb;

    Vector2 moveDirection = Vector2.zero;

    void Awake() {
        controls = new InputMaster();
        controls.Player.Interact.performed += _ => Interact();
    }

    void Update()
    {
        // Input
        moveDirection = controls.Player.Movement.ReadValue<Vector2>();
        Debug.Log(moveDirection);
    }

    void FixedUpdate() 
    {
        // Movement
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    void Interact(){
        Debug.Log("Interaction!");
    }

    private void OnEnable() 
    {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }
}

