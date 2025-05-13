using UnityEngine;
using System;

public enum PlayerModeType { Normal, Special }

public class PlayerStatus : MonoBehaviour
{
    [Header("ステータス")]
    [SerializeField] private int maxHP = 3;
    [SerializeField] private float heroTimeMax = 3f;
    public float HeroTimeMax => heroTimeMax; // ← 追加


    private int hp;
    private float heroTime;
    private PlayerModeType currentMode = PlayerModeType.Normal;

    // ======== プロパティ ========
    public int HP => hp;
    public float HeroTime => heroTime;
    public PlayerModeType CurrentMode => currentMode;

    // ======== イベント ========
    public event Action<int> OnHPChanged;
    public event Action<float> OnHeroTimeChanged;
    public event Action<PlayerModeType> OnModeChanged;

    // ======== 変身音 ========
    [SerializeField] private AudioClip transformSE;
    [SerializeField] private AudioClip revertSE;


    void Start()
    {
        ResetStatus();
    }

    void Update()
    {
        if (currentMode == PlayerModeType.Special && heroTime > 0f)
        {
            heroTime -= Time.deltaTime;
            heroTime = Mathf.Max(heroTime, 0f);
            OnHeroTimeChanged?.Invoke(heroTime);

            if (heroTime <= 0f)
            {
                SetMode(PlayerModeType.Normal);
            }
        }
    }

    // ======== 状態変更API ========

    public void ConsumeHP(int amount)
    {
        int prev = hp;
        hp = Mathf.Max(0, hp - amount);
        if (hp != prev) OnHPChanged?.Invoke(hp);
    }

    public void RecoverHP(int amount)
    {
        int prev = hp;
        hp = Mathf.Min(maxHP, hp + amount);
        if (hp != prev) OnHPChanged?.Invoke(hp);
    }

    public void SetHeroTime(float seconds)
    {
        heroTime = Mathf.Max(0f, seconds);
        OnHeroTimeChanged?.Invoke(heroTime);

        if (heroTime > 0f)
        {
            SetMode(PlayerModeType.Special);
        }
    }

    public void SetMode(PlayerModeType mode)
    {
        if (currentMode != mode)
        {
            currentMode = mode;
            OnModeChanged?.Invoke(currentMode);
            // 音を鳴らす
            if (mode == PlayerModeType.Special && transformSE != null)
            {
                AudioManager.instance.PlaySE(transformSE);
            }
            else if (mode == PlayerModeType.Normal && revertSE != null)
            {
                AudioManager.instance.PlaySE(revertSE);
            }
        }
    }

    public void ResetStatus()
    {
        hp = maxHP;
        heroTime = 0f;
        currentMode = PlayerModeType.Normal;

        OnHPChanged?.Invoke(hp);
        OnHeroTimeChanged?.Invoke(heroTime);
        OnModeChanged?.Invoke(currentMode);
    }

    // ======== 状態チェック用 ========

    public bool IsDead() => hp <= 0;
    public bool CanTransform() => hp > 0;
    public bool IsHeroMode() => currentMode == PlayerModeType.Special;
}
