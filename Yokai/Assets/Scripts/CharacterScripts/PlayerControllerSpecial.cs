using System.Collections;
using UnityEngine;

public class PlayerControllerSpecial : PlayerControllerBase
{
    protected override void Start()
    {
        base.Start();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyBase enemy = collision.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.Defeat();
                if (attackSE != null)
                {
                    AudioManager.instance.PlaySE(attackSE);
                }

                if (characterAnim != null)
                {
                    characterAnim.Play("H_Attack");
                }

                // 一定時間停止
                CharaMoveStop(stopTime);

            }
        }
    }

}
