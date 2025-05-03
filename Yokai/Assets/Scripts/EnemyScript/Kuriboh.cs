using UnityEngine;

public class Kuriboh : EnemyBase
{
    public float moveSpeed;
    public int moveDirection = -1;

    protected override void Move()
    {
        rb.velocity = new Vector2(moveSpeed * moveDirection, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            moveDirection *= -1;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}

