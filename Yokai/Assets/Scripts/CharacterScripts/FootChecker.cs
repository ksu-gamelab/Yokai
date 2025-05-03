// FootChecker.cs
using UnityEngine;

public class FootChecker : MonoBehaviour
{
    public PlayerModeManager modeManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            modeManager.NotifyGrounded(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            modeManager.NotifyGrounded(false);
        }
    }
}