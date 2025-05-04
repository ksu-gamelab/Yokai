using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderEnemy : EnemyBase
{
    public float moveSpeed = 1f;
    private int moveDirection = 1;
    private float nextDirectionChangeTime = 0f;

    [SerializeField]
    private Transform spriteTransform; // ← スプライトのある子オブジェクトをここに指定

    protected override void Move()
    {
        if (Time.time >= nextDirectionChangeTime)
        {
            moveDirection = Random.Range(0, 2) == 0 ? -1 : 1;
            nextDirectionChangeTime = Time.time + Random.Range(1f, 3f);
            spriteTransform = transform.GetChild(0); // スプライトのある子オブジェクトを取得
        }

        // スプライトの向きを反転（子オブジェクトの localScale を変更）
        if (spriteTransform != null)
        {
            Vector3 spriteScale = spriteTransform.localScale;
            if (moveDirection == 1)
                spriteScale.x = -Mathf.Abs(spriteScale.x);  // 右に進むときに反転
            else
                spriteScale.x = Mathf.Abs(spriteScale.x);   // 左はデフォルト
            spriteTransform.localScale = spriteScale;
        }

        rb.velocity = new Vector2(moveSpeed * moveDirection, 0);
    }
}
