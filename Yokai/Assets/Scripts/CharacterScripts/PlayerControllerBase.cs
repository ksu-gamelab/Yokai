using System.Collections;
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
            //Debug.Log(IsGrounded());
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
    }


    public virtual void SetGrounded(bool value)
    {
        isGrounded = value;
        if (isGrounded)
        {
            currentJumpCount = 0;
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

    public float GetMyXSpeed() => rb.velocity.x;

    public float GetMyYSpeed() => rb.velocity.y;

    public void ChangeAnimation()
    {
        characterAnim = GetComponentInChildren<Animator>();
        if (!IsGrounded())
        {
            if (GetMyYSpeed() > 0)
            {
                characterAnim.Play("Jumping");
            }
            else
            {
                characterAnim.Play("Falling");
            }
        }
        else
        {
            characterAnim.Play("Idle");
        }
    }

    public IEnumerator StopMovementForSeconds(float duration)
    {
        SetCanMove(false);                // 入力禁止
        rb.velocity = Vector2.zero;      // 慣性を止める

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;            // 重力を一時的に無効化

        yield return new WaitForSeconds(duration);

        rb.gravityScale = originalGravity; // 重力を元に戻す
        SetCanMove(true);                 // 入力再開
        ChangeAnimation(); // アニメーション変更

    }

    public void CharaMoveStop(float stopTime)
    {
        /*if (characterAnim != null)
        {*/
        StartCoroutine(StopMovementForSeconds(stopTime));
        //}
    }

}
