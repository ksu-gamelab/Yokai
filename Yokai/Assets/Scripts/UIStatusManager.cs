using UnityEngine;
using UnityEngine.UI;

public class UIStatusManager : MonoBehaviour
{
    [Header("Hero UI")]
    public Image heroGaugeImage;
    public Image heroGaugeImageMask;        // Image Type: Filled（赤・青切替も対応）
    public Sprite normalSprite;              // 通常スプライト（青）
    public Sprite specialSprite;             // 変身スプライト（赤）

    public float maxHeroTime = 3f;

    [Header("HP UI")]
    public Image[] hpIcons;
    public Sprite hpFullSprite;
    public Sprite hpEmptySprite;

    private PlayerStatus status;

    void Start()
    {
        status = FindObjectOfType<PlayerStatus>();
    }

    void Update()
    {
        if (status == null) return;

        UpdateHeroGauge();
        UpdateHPDisplay();
    }
    void UpdateHeroGauge()
    {
        bool isHero = status.CurrentMode == PlayerModeType.Special;

        // スプライト切替（青↔赤）
        heroGaugeImage.sprite = isHero ? specialSprite : normalSprite;

        if (isHero)
        {
            float ratio = Mathf.Clamp01(status.HeroTimeRemaining / maxHeroTime);
            heroGaugeImageMask.fillAmount = 1f - ratio;  // ←ここを逆に！
        }
        else
        {
            heroGaugeImageMask.fillAmount = 0f; // 通常時はマスクで全覆い
        }
    }


    void UpdateHPDisplay()
    {
        int count = hpIcons.Length;
        int currentHP = Mathf.Clamp(status.HP, 0, count);

        for (int i = count - 1; i >= 0; i--)
        {
            int displayIndex = count - 1 - i;
            hpIcons[i].sprite = (displayIndex < currentHP) ? hpFullSprite : hpEmptySprite;
        }
    }
}
