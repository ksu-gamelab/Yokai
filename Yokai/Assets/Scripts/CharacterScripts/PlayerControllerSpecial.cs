using System.Collections;
using UnityEngine;

public class PlayerControllerSpecial : PlayerControllerBase
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        // 必要ならスペシャル攻撃処理
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyBase enemy = collision.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.Defeat();
                if (characterAnim != null)
                {
                    characterAnim.SetTrigger("attack1");
                    // 一定時間停止
                    StartCoroutine(StopMovementForSeconds(stopTime));
                }
            }
        }
    }

    private IEnumerator StopMovementForSeconds(float duration)
    {
        SetCanMove(false);                // 入力禁止
        rb.velocity = Vector2.zero;      // 慣性を止める

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;            // 重力を一時的に無効化

        yield return new WaitForSeconds(duration);

        rb.gravityScale = originalGravity; // 重力を元に戻す
        SetCanMove(true);                 // 入力再開
    }




}
