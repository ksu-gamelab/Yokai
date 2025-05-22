using UnityEngine;
using UnityEngine.UI;

public class ConfigUIController : MonoBehaviour
{
    [SerializeField] private Selectable[] uiElements;
    private int index = 0;

    [Header("入力ディレイ")]
    public float inputDelay = 0.2f;
    private float inputTimer = 0f;

    private void OnEnable()
    {
        index = 0;
        Highlight(index);
        inputTimer = 0f;
    }


    private void Update()
    {
        if (!gameObject.activeSelf) return;

        inputTimer -= Time.unscaledDeltaTime;

        if (inputTimer <= 0f)
        {
            float v = Input.GetAxisRaw("Vertical");

            if (v < -0.5f)
            {
                index = (index + 1) % uiElements.Length;
                Highlight(index);
                inputTimer = inputDelay;
            }
            else if (v > 0.5f)
            {
                index = (index - 1 + uiElements.Length) % uiElements.Length;
                Highlight(index);
                inputTimer = inputDelay;
            }

            // スライダー左右移動もディレイ付きで対応
            if (uiElements[index] is Slider slider)
            {
                float h = Input.GetAxisRaw("Horizontal");
                float step = 0.1f;

                if (h > 0.5f)
                {
                    slider.value = Mathf.Clamp(slider.value + step, slider.minValue, slider.maxValue);
                    inputTimer = inputDelay; // ← 操作後にディレイを再設定
                }
                else if (h < -0.5f)
                {
                    slider.value = Mathf.Clamp(slider.value - step, slider.minValue, slider.maxValue);
                    inputTimer = inputDelay; // ← 操作後にディレイを再設定
                }
            }
        }

        // 決定ボタン
        if (Input.GetButtonDown("Submit"))
        {
            var element = uiElements[index];

            if (element is Button btn) btn.onClick.Invoke();
            else if (element is Toggle toggle) toggle.isOn = !toggle.isOn;
        }

        // キャンセル（戻る）
        if (Input.GetButtonDown("Cancel"))
        {
            gameObject.SetActive(false);
            GameStateManager.Instance.ResumeGame();
            FindObjectOfType<GenericUISelector>()?.RestoreSelection();
        }
    }


    private void Highlight(int i)
    {
        for (int j = 0; j < uiElements.Length; j++)
        {
            var colors = uiElements[j].colors;
            colors.normalColor = (j == i) ? Color.yellow : Color.white;
            uiElements[j].colors = colors;
        }

        uiElements[i].Select(); // 見た目も合わせる
    }
}
