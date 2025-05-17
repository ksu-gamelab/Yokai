using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("共通設定")]
    public float normalMoveSpeed = 5f;
    public float specialMoveSpeed = 8f;

    public float normalJumpForce = 10f;
    public float specialJumpForce = 14f;

    public int maxJumpCount = 1;
    public float stopTime = 0.7f;

    public AudioClip jumpSE;
    public AudioClip attackSE;

    private Rigidbody2D rb;
    private PlayerStatus status;
    private PlayerViewManager viewManager;

    private PlayerEffectManager effectManager;

    private bool isGrounded = false;
    private int currentJumpCount = 0;
    private bool canMove = true;

    private string[] attackAnimationName = {
        "H_Attack",
        "H_Attack2",
        "H_Attack3"
    };

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        status = GetComponent<PlayerStatus>();
        viewManager = GetComponent<PlayerViewManager>();
        effectManager = GetComponent<PlayerEffectManager>();
    }

    private void Update()
    {
        if (!canMove) return;

        HandleInput();
        UpdateAnimation();
    }

    private void HandleInput()
    {
        float moveSpeed = status.IsHeroMode() ? specialMoveSpeed : normalMoveSpeed;
        float jumpForce = status.IsHeroMode() ? specialJumpForce : normalJumpForce;

        float h = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(h * moveSpeed, rb.velocity.y);

        // 向き変更
        if (h != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(h) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        if (Input.GetKeyDown(KeyCode.Space) && currentJumpCount < maxJumpCount)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            currentJumpCount++;
            viewManager.PlayTrigger("isJump");
            AudioManager.instance.PlaySE(jumpSE);
        }

        if (Input.GetKeyDown(KeyCode.Return) && !status.IsDead())
        {
            StartCoroutine(StopMovementForSeconds(stopTime));
            GetComponent<PlayerModeManager>()?.ForceTransformToSpecial();
        }

    }

    private void UpdateAnimation()
    {
        viewManager.SetAnimatorFloat("dx", Mathf.Abs(rb.velocity.x));
        viewManager.SetAnimatorFloat("dy", rb.velocity.y);
        viewManager.SetAnimatorBool("isGround", isGrounded);
    }

    public void SetGrounded(bool grounded)
    {
        isGrounded = grounded;
        if (grounded)
        {
            currentJumpCount = 0;
        }
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (status.IsHeroMode())
            {
                EnemyBase enemy = collision.GetComponent<EnemyBase>();
                if (enemy != null)
                {
                    enemy.Defeat();

                    if (attackSE != null)
                        AudioManager.instance.PlaySE(attackSE);

                    viewManager.PlayAnimation(attackAnimationName[Random.Range(0, attackAnimationName.Length)]);
                    effectManager.PlayEffect("Attack");
                    StartCoroutine(StopMovementForSeconds(stopTime));
                }
            }
            else
            {
                Debug.Log("Normal: Enemy hit!");
                GetComponent<PlayerModeManager>()?.ForceTransformToSpecial();

                // 敵を即倒す処理を追加
                EnemyBase enemy = collision.GetComponent<EnemyBase>();
                if (enemy != null)
                {
                    enemy.Defeat();

                    if (attackSE != null)
                        AudioManager.instance.PlaySE(attackSE);
                    StartCoroutine(StopMovementForSeconds(stopTime));
                }
            }

        }
    }

    private IEnumerator StopMovementForSeconds(float duration)
    {
        SetCanMove(false);
        rb.velocity = Vector2.zero;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        yield return new WaitForSeconds(duration);

        rb.gravityScale = originalGravity;
        SetCanMove(true);

        // 状態に応じて戻す
        if (!isGrounded)
        {
            viewManager.PlayAnimation("Falling"); // 空中の場合
        }
        else
        {
            viewManager.PlayAnimation("Idle"); // 地上の場合
        }
    }

    private void PlayIdleAnimation()
    {
        if (!isGrounded) return;
        viewManager.PlayTrigger("Idle");
    }

    public bool IsGrounded() => isGrounded;
    public float GetXSpeed() => rb.velocity.x;
    public float GetYSpeed() => rb.velocity.y;

    public string GetCurrentStateClipName()
    {
        return viewManager.GetCurrentClipName();
    }
}
