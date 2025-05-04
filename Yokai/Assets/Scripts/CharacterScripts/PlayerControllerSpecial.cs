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
        // タグが Enemy のものに当たった場合
        if (collision.CompareTag("Enemy"))
        {
            EnemyBase enemy = collision.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                //アニメーション再生
                characterAnim.SetTrigger("attack1");
                enemy.Defeat();
            }
        }
    }

}
