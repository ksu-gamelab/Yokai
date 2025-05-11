using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class PlayerControllerBase : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 30;
    public float jumpForce;
    public int maxJumpCount = 1;

    public float stopTime = 0.7f; //攻撃アニメーションのストップ時間

    protected Rigidbody2D rb;
    protected Animator characterAnim;
    protected bool isGrounded;
    protected int currentJumpCount = 0;

    protected bool canMove = true; // ← 移動可否フラグ

    public AudioClip jumpSE;
    public AudioClip attackSE;


    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void HandleInput()
    {
        if (!canMove) return;

        if (characterAnim == null)
            characterAnim = GetComponentInChildren<Animator>();

        float h = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(h * moveSpeed, rb.velocity.y);

        // 向き変更
        if (h != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(h) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        // ★ Animator パラメータ更新
        if (characterAnim != null)
        {
            characterAnim.SetFloat("dx", Mathf.Abs(rb.velocity.x));
            characterAnim.SetFloat("dy", rb.velocity.y);
            Debug.Log(IsGrounded());
            characterAnim.SetBool("isGround", IsGrounded());
        }

        // ジャンプ入力
        if (Input.GetKeyDown(KeyCode.Space) && currentJumpCount < maxJumpCount)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            currentJumpCount++;
            characterAnim.SetTrigger("isJump");
            AudioManager.instance.PlaySE(jumpSE);
        }

        // 攻撃入力
        if (Input.GetKeyDown(KeyCode.F))
            Attack(); // ここで isAttacking を true にするなどの処理
    }


    public virtual void SetGrounded(bool value)
    {
        isGrounded = value;
        if (isGrounded)
        {
            currentJumpCount = 0;
            if (characterAnim != null)
            {
                characterAnim.SetBool("jump_up", false);
                characterAnim.SetBool("jump_down", false);
            }
        }
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    public void SetAnimator(Animator anim)
    {
        characterAnim = anim;
    }

    public string GetCurrentStateClipName()
    {
        if (characterAnim == null) return "";
        var clips = characterAnim.GetCurrentAnimatorClipInfo(0);
        return (clips.Length > 0) ? clips[0].clip.name : "";
    }

    public bool IsGrounded() => isGrounded;

    protected virtual void Attack() { }
}
