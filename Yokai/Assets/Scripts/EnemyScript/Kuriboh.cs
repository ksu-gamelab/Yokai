using UnityEngine;

public class Kuriboh : EnemyBase
{
    public float moveSpeed = 1f;
    public int moveDirection = -1;

    private bool isOnGround = true;

    protected override void Move()
    {
        rb.velocity = new Vector2(moveSpeed * moveDirection, 0);

        if (!isOnGround)
        {
            FlipDirection();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            isOnGround = false;
        }
    }

    private void FlipDirection()
    {
        moveDirection *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        isOnGround = true;
    }
}

