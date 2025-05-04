using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField]
    private int hp = 2;

    [SerializeField]
    private float heroTimeRemaining = 0f;

    private PlayerModeType currentMode = PlayerModeType.Normal;

    public int HP => hp;
    public float HeroTimeRemaining => heroTimeRemaining;
    public PlayerModeType CurrentMode => currentMode;

    public bool CanTransform() => hp > 0;

    public void ConsumeHP(int amount)
    {
        hp = Mathf.Max(0, hp - amount);
    }

    public void SetHeroTime(float duration)
    {
        heroTimeRemaining = duration;
    }

    public void UpdateHeroTime(float deltaTime)
    {
        heroTimeRemaining = Mathf.Max(0, heroTimeRemaining - deltaTime);
    }

    public void SetMode(PlayerModeType mode)
    {
        currentMode = mode;
    }

    public void ResetStatus()
    {
        hp = 2;
        heroTimeRemaining = 0f;
        currentMode = PlayerModeType.Normal;
    }
}
