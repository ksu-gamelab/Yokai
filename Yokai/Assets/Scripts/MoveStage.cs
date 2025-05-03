using UnityEngine;

public class MoveStage : MonoBehaviour
{
    public Transform target;      // 追従対象（プレイヤー）
    public float followSpeed = 2f;

    private bool shouldFollow = false;
    private float targetY;

    void Update()
    {
        if (shouldFollow)
        {
            Vector3 current = transform.position;
            float newY = Mathf.Lerp(current.y, targetY, followSpeed * Time.deltaTime);
            transform.position = new Vector3(current.x, newY, current.z);

            if (Mathf.Abs(newY - targetY) < 0.01f)
                shouldFollow = false;  // 一度追従し終わったら止める
        }
    }

    public void TriggerFollow()
    {
        targetY = target.position.y;
        shouldFollow = true;
    }

    public void StopFollowImmediately()
    {
        shouldFollow = false;
    }
}
