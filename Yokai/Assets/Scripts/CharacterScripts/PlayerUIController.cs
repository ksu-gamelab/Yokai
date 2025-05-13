using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [Header("ヒーローUI")]
    public Image heroGaugeImage;
    public Image heroGaugeImageMask;        // Image Type: Filled（FillAmountを使う）
    public Sprite normalSprite;
    public Sprite specialSprite;


    [Header("HP UI")]
    public Image[] hpIcons;
    public Sprite hpFullSprite;
    public Sprite hpEmptySprite;

    private PlayerStatus status;

    private void Update()
    {
        if (status == null || status.CurrentMode != PlayerModeType.Special) return;

        // ヒーローゲージだけ毎フレーム更新
        UpdateHeroGauge(status.HeroTime);
    }

    public void SetStatus(PlayerStatus playerStatus)
    {
        // イベント解除（再登録用）
        if (status != null)
        {
            status.OnHPChanged -= UpdateHPDisplay;
            status.OnHeroTimeChanged -= UpdateHeroGauge;
            status.OnModeChanged -= UpdateHeroGaugeSprite;
        }

        status = playerStatus;

        // イベント登録
        status.OnHPChanged += UpdateHPDisplay;
        status.OnHeroTimeChanged += UpdateHeroGauge;
        status.OnModeChanged += UpdateHeroGaugeSprite;

        // 初期状態反映
        UpdateHPDisplay(status.HP);
        UpdateHeroGauge(status.HeroTime);
        UpdateHeroGaugeSprite(status.CurrentMode);
    }

    private void UpdateHPDisplay(int hp)
    {
        int count = hpIcons.Length;
        int currentHP = Mathf.Clamp(hp, 0, count);

        for (int i = count - 1; i >= 0; i--)
        {
            int displayIndex = count - 1 - i;
            hpIcons[i].sprite = (displayIndex < currentHP) ? hpFullSprite : hpEmptySprite;
        }
    }

    private void UpdateHeroGauge(float time)
    {
        bool isHero = status.CurrentMode == PlayerModeType.Special;
        if (!isHero)
        {
            // 通常モード時：ゲージは全表示（＝マスクなし）
            heroGaugeImageMask.fillAmount = 0f;
            return;
        }

        float ratio = Mathf.Clamp01(time / status.HeroTimeMax);

        heroGaugeImageMask.fillAmount = 1f - ratio; // 時間が減るとマスクが増える
    }


    private void UpdateHeroGaugeSprite(PlayerModeType mode)
    {
        bool isHero = mode == PlayerModeType.Special;
        if (heroGaugeImage != null)
        {
            heroGaugeImage.sprite = isHero ? specialSprite : normalSprite;
        }
    }

    private void OnDestroy()
    {
        if (status != null)
        {
            status.OnHPChanged -= UpdateHPDisplay;
            status.OnHeroTimeChanged -= UpdateHeroGauge;
            status.OnModeChanged -= UpdateHeroGaugeSprite;
        }
    }
}
