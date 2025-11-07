using UnityEngine;

public class ComputerPaddle : Paddle
{
    [SerializeField] private Rigidbody2D ball;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float predictionDistance = 20f;

    private void FixedUpdate()
    {
        Vector2 direction = ball.linearVelocity.normalized;
        RaycastHit2D hit = Physics2D.Raycast(ball.position, direction, predictionDistance, wallLayer);

        float targetY;

        if (hit.collider != null)
        {
            targetY = hit.point.y;
        }
        else
        {
            targetY = ball.position.y;
        }

        if (targetY > rb.position.y)
        {
            rb.AddForce(Vector2.up * speed);
        }
        else if (targetY < rb.position.y)
        {
            rb.AddForce(Vector2.down * speed);
        }
    }
}
