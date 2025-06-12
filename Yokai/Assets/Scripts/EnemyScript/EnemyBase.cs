using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator anim;
    protected bool isDefeated = false;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (rb != null) rb.isKinematic = true;
    }

    protected virtual void Update()
    {
        if (isDefeated) return;
        Move();
    }

    protected abstract void Move(); // 子クラスに移動の実装を任せる

    public virtual void Defeat()
    {
        
        if (isDefeated) return;
        isDefeated = true;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        foreach (var col in GetComponents<Collider2D>())
        {
            col.enabled = false;
        }


        Destroy(gameObject, 1f);
    }
}
