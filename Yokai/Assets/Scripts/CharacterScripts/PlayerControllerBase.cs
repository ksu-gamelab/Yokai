using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class PlayerControllerBase : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 30;
    public float jumpForce;
    public int maxJumpCount = 1;

    protected Rigidbody2D rb;
    protected Animator characterAnim;
    protected bool isGrounded;
    protected int currentJumpCount = 0;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void HandleInput()
    {
        if (characterAnim == null)
            characterAnim = GetComponentInChildren<Animator>();

        float h = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(h * moveSpeed, rb.velocity.y);

        if (h != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(h) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        if (characterAnim != null)
            characterAnim.SetBool("run", isGrounded && Mathf.Abs(rb.velocity.x) > 0.01f);

        if (Input.GetKeyDown(KeyCode.Space) && currentJumpCount < maxJumpCount)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            currentJumpCount++;

            if (characterAnim != null)
            {
                characterAnim.SetBool("run", false);
                characterAnim.SetBool("jump_up", true);
                characterAnim.SetBool("jump_down", false);
            }
        }

        if (!isGrounded && rb.velocity.y < -0.1f)
        {
            if (characterAnim != null)
            {
                characterAnim.SetBool("jump_up", false);
                characterAnim.SetBool("jump_down", true);
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
            Attack();
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
