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

public enum TutorialStage
{
    None,
    Stage1,
    Stage2,
    Stage3,
    Complete
}

public enum TutorialMode
{
    None,
    Novel,
    Play
}

public enum NormalStage
{
    None,
    Stage1,
    Stage2,
    Stage3
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public GameState CurrentState { get; private set; } = GameState.Title;

    public NormalStage CurrentNormalStage { get; private set; } = NormalStage.None;

    // チュートリアル情報
    public TutorialStage CurrentTutorialStage { get; private set; } = TutorialStage.None;
    public TutorialMode CurrentTutorialMode { get; private set; } = TutorialMode.None;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;
        Debug.Log("ゲーム状態: " + newState);

        switch (newState)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.GameOver:
                SceneManager.LoadScene("GameOver");
                break;
            case GameState.Clear:
                SceneManager.LoadScene("GameClear");
                break;
        }
    }

    public void TriggerGameOver()
    {
        if (CurrentState != GameState.GameOver)
            SetState(GameState.GameOver);
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

    // ---------- Tutorial関連 ----------
    public void SetTutorialStage(TutorialStage stage)
    {
        CurrentTutorialStage = stage;
        Debug.Log("チュートリアルステージ設定: " + stage);
    }

    public void SetTutorialMode(TutorialMode mode)
    {
        CurrentTutorialMode = mode;
        Debug.Log("チュートリアルモード設定: " + mode);
    }

    // ---------NormalStage関連------------
    public void SetNormalStage(NormalStage stage)
    {
        CurrentNormalStage = stage;
    }
    

    public bool IsTutorialPlay() => CurrentTutorialMode == TutorialMode.Play;
    public bool IsTutorialNovel() => CurrentTutorialMode == TutorialMode.Novel;

}
