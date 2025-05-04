using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public float maxTime = 60f; // タイマーの初期値（秒）
    private float currentTime;

    private Text timerText;

    void Start()
    {
        timerText = GetComponent<Text>();
        currentTime = maxTime;
    }

    void Update()
    {
        //if (!GameStateManager.Instance.IsPlaying()) return;

        currentTime -= Time.deltaTime;
        currentTime = Mathf.Max(0, currentTime); // マイナスにならないように

        UpdateDisplay();

        if (currentTime <= 0f)
        {
            GameStateManager.Instance.TriggerGameOver();
        }
    }

    void UpdateDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
