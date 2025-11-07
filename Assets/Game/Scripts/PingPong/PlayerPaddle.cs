using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPaddle : Paddle
{
    [SerializeField] private PingPong controls;
    private Vector2 moveInput;

    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new PingPong();
        }

        controls.Paddle.Move.performed += OnMove;
        controls.Paddle.Move.canceled += OnMoveCancel;
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Paddle.Move.performed -= OnMove;
        controls.Paddle.Move.canceled -= OnMoveCancel;
        controls.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCancel(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (moveInput.y > 0f)
        {
            rb.AddForce(Vector2.up * speed);
        }
        else if (moveInput.y < 0f)
        {
            rb.AddForce(Vector2.down * speed);
        }
    }
}
