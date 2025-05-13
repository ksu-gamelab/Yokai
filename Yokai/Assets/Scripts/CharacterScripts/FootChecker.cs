using UnityEngine;

public class FootChecker : MonoBehaviour
{
    public PlayerController playerController;  // 変更点
    public MoveStage camFollower;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            playerController.SetGrounded(true);

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
            playerController.SetGrounded(false);

            if (camFollower != null)
            {
                camFollower.StopFollowImmediately();
            }
        }
    }
}
