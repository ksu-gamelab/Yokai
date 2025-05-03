using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControllerSpecial : MonoBehaviour
{
    [Header("移動設定 (スペシャル)")]
    public float moveSpeed = 5f;
    public float jumpForce = 18f; // 通常より高く跳ぶ
    public int maxJumpCount = 1;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int currentJumpCount = 0;

    private Animator characterAnim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        characterAnim = GetComponentInChildren<Animator>();
    }

    public void HandleInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(h * moveSpeed, rb.velocity.y);

        // 向きの切り替え
        if (h != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(h) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        // アニメーション：run
        if (characterAnim != null)
        {
            if (isGrounded)
            {
                characterAnim.SetBool("run", h != 0);
            }
            else
            {
                characterAnim.SetBool("run", false);
            }
        }

        // ジャンプ
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

        // 落下中の判定
        if (!isGrounded && rb.velocity.y < -0.1f)
        {
            if (characterAnim != null)
            {
                characterAnim.SetBool("jump_up", false);
                characterAnim.SetBool("jump_down", true);
            }
        }

        // 攻撃
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("スペシャル攻撃!");
        }
    }

    public void SetGrounded(bool value)
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
}
