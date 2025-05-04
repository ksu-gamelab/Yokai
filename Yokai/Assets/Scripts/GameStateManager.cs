using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Title,
    Playing,
    Paused,
    GameOver,
    Clear
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public GameState CurrentState { get; private set; } = GameState.Title;

    void Awake()
    {
        // Singleton化（複数生成を防ぐ）
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // シーンをまたいでも残す
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;
        Debug.Log("ゲーム状態: " + newState);

        // 状態に応じた処理
        switch (newState)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.GameOver:
                //Time.timeScale = 0f;
                SceneManager.LoadScene("GameOver");
                break;
            case GameState.Clear:
                //Time.timeScale = 0f;
                SceneManager.LoadScene("GameClear");
                break;
        }
    }

    public void TriggerGameOver()
    {
        if (CurrentState != GameState.GameOver)
        {
            SetState(GameState.GameOver);
        }
    }

    public void TriggerGameStart()
    {
        if (CurrentState != GameState.Playing)
            SetState(GameState.Playing);
    }

    public void TriggerGameClear()
    {
        if (CurrentState != GameState.Clear)
            SetState(GameState.Clear);
    }

    public void TriggerGamePause()
    {
        if (CurrentState != GameState.Paused)
            SetState(GameState.Paused);
    }

    public void ResumeGame()
    {
        if (CurrentState == GameState.Paused)
            SetState(GameState.Playing);
    }


    public bool IsPlaying() => CurrentState == GameState.Playing;
    public bool IsPaused() => CurrentState == GameState.Paused;
}
