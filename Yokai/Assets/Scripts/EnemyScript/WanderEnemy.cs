using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderEnemy : EnemyBase
{
    public float moveSpeed = 1f;
    private int moveDirection = 1;
    private float nextDirectionChangeTime = 0f;

    protected override void Move()
    {
        if (Time.time >= nextDirectionChangeTime)
        {
            moveDirection = Random.Range(0, 2) == 0 ? -1 : 1;
            nextDirectionChangeTime = Time.time + Random.Range(1f, 3f);
        }

        rb.velocity = new Vector2(moveSpeed * moveDirection, 0);
    }
}

