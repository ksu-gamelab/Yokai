using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial1Enemy : EnemyBase
{
    public float moveSpeed = 1f;
    public int moveDirection = -1;

    private bool isOnGround = true;

    protected override void Move()
    {
        // 任意の移動処理を記述（必要に応じて）
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
        // 死んだ時に、現在の GamePhase を見て同じシーンを再ロード
        var currentPhase = GameStateManager.Instance.CurrentPhase;

        switch (currentPhase)
        {
            case GamePhase.Tutorial1:
                SceneManager.LoadScene("NovelTest");
                break;
            case GamePhase.Tutorial2:
                SceneManager.LoadScene("Tutorial2");
                break;
            // 他にもチュートリアルが増えるなら追加
            default:
                Debug.LogWarning("チュートリアル以外でのDefeatが呼ばれました。");
                break;
        }
    }
}
