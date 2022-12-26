using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 movementInput;

    void Update()
    {
        transform
            .Translate(movementInput *
            GetComponent<PlayerStats>().moveSpeed *
            Time.deltaTime);
    }

    void OnMove(InputValue movementVal)
    {
        movementInput = movementVal.Get<Vector2>();
    }
}
