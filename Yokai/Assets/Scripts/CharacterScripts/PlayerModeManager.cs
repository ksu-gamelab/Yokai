// PlayerModeManager.cs
using UnityEngine;

public enum PlayerModeType { Normal, Special }

public class PlayerModeManager : MonoBehaviour
{
    private PlayerControllerNormal normalController;
    private PlayerControllerSpecial specialController;
    private PlayerStatus status;

    public float specialModeDuration = 10f;
    private float specialModeTimer = 0f;

    private PlayerModeType currentMode = PlayerModeType.Normal;

    void Start()
    {
        normalController = GetComponent<PlayerControllerNormal>();
        specialController = GetComponent<PlayerControllerSpecial>();
        status = GetComponent<PlayerStatus>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TrySwitchToSpecial();
        }

        if (currentMode == PlayerModeType.Special)
        {
            specialModeTimer -= Time.deltaTime;
            if (specialModeTimer <= 0f)
            {
                SwitchToNormal();
            }
        }

        if (currentMode == PlayerModeType.Normal)
        {
            normalController.HandleInput();
        }
        else
        {
            specialController.HandleInput();
        }
    }

    void TrySwitchToSpecial()
    {
        if (currentMode == PlayerModeType.Normal && status.CanTransform())
        {
            status.ConsumeHP(1);
            SwitchToSpecial();
        }
    }

    void SwitchToSpecial()
    {
        currentMode = PlayerModeType.Special;
        specialModeTimer = specialModeDuration;
        Debug.Log("スペシャルモードへ!");
    }

    void SwitchToNormal()
    {
        currentMode = PlayerModeType.Normal;
        Debug.Log("ノーマルモードへ戻る");
    }

    public void NotifyGrounded(bool grounded)
    {
        if (currentMode == PlayerModeType.Normal)
        {
            normalController.SetGrounded(grounded);
        }
        else
        {
            specialController.SetGrounded(grounded);
        }
    }
}