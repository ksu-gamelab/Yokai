using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Title,
    Playing,
    InScenario,
    Paused,
    GameOver,
    Clear
}

public enum GamePhase
{
    Tutorial1,
    Tutorial2,
    Stage1,
    Stage2,
    // 必要に応じて追加
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public GameState CurrentState { get; private set; } = GameState.Title;
    public GamePhase CurrentPhase { get; private set; } = GamePhase.Tutorial1;

 

    private void Awake()
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
                ActivatePlayOnObjects();
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.InScenario:
                ActivePlayStoryObjects();
                Time.timeScale = 1f;
                break;
            case GameState.Title:
                Time.timeScale = 1f;
                break;
            case GameState.GameOver:
                Time.timeScale = 1f;
                SceneManager.LoadScene("GameOver");
                break;
            case GameState.Clear:
                Time.timeScale = 1f;
                SceneManager.LoadScene("GameClear");
                break;
        }
    }

    // フェーズ設定
    public void SetPhase(GamePhase phase)
    {
        CurrentPhase = phase;
    }

    // 状態遷移トリガー
    public void TriggerGameStart() => SetState(GameState.Playing);
    public void TriggerScenario() => SetState(GameState.InScenario);
    public void TriggerGameOver() => SetState(GameState.GameOver);
    public void TriggerGameClear() => SetState(GameState.Clear);
    public void TriggerPause() { if (IsPlaying()) SetState(GameState.Paused); }
    public void ResumeGame() { if (IsPaused()) SetState(GameState.Playing); }

    // 状態チェック
    public bool IsPlaying() => CurrentState == GameState.Playing;
    public bool IsPaused() => CurrentState == GameState.Paused;
    public bool IsScenario() => CurrentState == GameState.InScenario;
    public bool IsTitle() => CurrentState == GameState.Title;
    public bool IsGameOver() => CurrentState == GameState.GameOver;
    public bool IsClear() => CurrentState == GameState.Clear;

    // 次のフェーズに進む（任意の順序ロジック）
    public void AdvancePhase()
    {
        switch (CurrentPhase)
        {
            case GamePhase.Tutorial1:
                SetPhase(GamePhase.Tutorial2);
                break;
            case GamePhase.Tutorial2:
                SetPhase(GamePhase.Stage1);
                break;
            case GamePhase.Stage1:
                SetPhase(GamePhase.Stage2);
                break;
            case GamePhase.Stage2:
                Debug.Log("全フェーズ完了");
                break;
        }
    }

    private void ActivatePlayOnObjects()
    {

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag("PlayOn"))
            {
                // シーン上のオブジェクトに限定する
                if (obj.scene.IsValid() && obj.hideFlags == HideFlags.None)
                {
                    obj.SetActive(true);
                    Debug.Log($"[GameStateManager] 非アクティブPlayOnオブジェクト有効化: {obj.name}");
                }
            }
        }

        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag("Story"))
            {
                // シーン上のオブジェクトに限定する
                if (obj.scene.IsValid() && obj.hideFlags == HideFlags.None)
                {
                    obj.SetActive(false);
                }
            }
        }
    }

    private void ActivePlayStoryObjects()
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag("Story"))
            {
                // シーン上のオブジェクトに限定する
                if (obj.scene.IsValid() && obj.hideFlags == HideFlags.None)
                {
                    obj.SetActive(true);
                    Debug.Log($"[GameStateManager] 非アクティブPlayOnオブジェクト有効化: {obj.name}");
                }
            }
        }

        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag("PlayOn"))
            {
                // シーン上のオブジェクトに限定する
                if (obj.scene.IsValid() && obj.hideFlags == HideFlags.None)
                {
                    obj.SetActive(false);
                }
            }
        }
    }


}
