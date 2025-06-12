using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial1Enemy : EnemyBase
{
    public float moveSpeed = 1f;
    public int moveDirection = -1;

    private bool isOnGround = true;

    protected override void Move()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            isOnGround = false;
        }
    }

    public override void Defeat()
    {
        GameStateManager.Instance.SetTutorialMode(TutorialMode.Play);
        SceneManager.LoadScene("Tutorial1");
        
    }

}

