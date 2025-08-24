using UnityEngine;

public class PlayerModeManager : MonoBehaviour
{
    private PlayerStatus status;
    private PlayerController controller;

    private PlayerEffectManager effectManager;


    private void Start()
    {
        status = GetComponent<PlayerStatus>();
        controller = GetComponent<PlayerController>();
        effectManager = GetComponent<PlayerEffectManager>();
    }

    /// <summary>
    /// スペシャルモードへの手動変身を試みる（HPを1消費）
    /// </summary>
    public void TryTransformToSpecial()
    {
        if (!status.CanTransform()) return;
        status.ConsumeHP(1);
        status.SetHeroTime(status.HeroTimeMax);
        effectManager.PlayEffect("ChangeH");
        Debug.Log("スペシャルモードに変身！");
    }

    /// <summary>
    /// 敵に当たったなど、強制的にスペシャルに変身する処理（HP0ならゲームオーバー）
    /// </summary>
    public void ForceTransformToSpecial()
    {
        if (status.IsDead())
        {
            Debug.Log("HPが0のため、ゲームオーバー");
            GameStateManager.Instance.TriggerGameOver();
        }
        else
        {
            TryTransformToSpecial();
        }
    }

}
