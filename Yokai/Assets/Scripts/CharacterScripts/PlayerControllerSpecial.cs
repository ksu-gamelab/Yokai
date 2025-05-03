// PlayerControllerSpecial.cs
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void HandleInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(h * moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && currentJumpCount < maxJumpCount)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            currentJumpCount++;
        }

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
        }
    }
}