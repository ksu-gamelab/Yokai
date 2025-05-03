using UnityEngine;

public class FootChecker : MonoBehaviour
{
    public PlayerModeManager modeManager;
    public MoveStage camFollower; // カメラの親にアタッチされたスクリプト

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            modeManager.NotifyGrounded(true);

            // カメラをプレイヤーのY位置まで追従開始
            if (camFollower != null)
            {
                camFollower.TriggerFollow();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            modeManager.NotifyGrounded(false);
            if (camFollower != null)
            {
                camFollower.StopFollowImmediately();
            }
        }
    }
}
