// PlayerControllerNormal.cs
using UnityEngine;

public class PlayerControllerNormal : PlayerControllerBase
{
    private PlayerModeManager modeManager;

    protected override void Start()
    {
        base.Start();
        modeManager = GetComponent<PlayerModeManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // スペシャルモードに強制変身
            modeManager.ForceTransformToSpecial();
        }
    }
}
