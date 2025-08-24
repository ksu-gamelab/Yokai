using UnityEngine;

public class ScenarioController : MonoBehaviour
{
    public static ScenarioController Instance { get; private set; }

    [SerializeField] private PlayStory_JSON playStory;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // 初期フェーズに対応するシナリオを再生する場合
        StartScenarioForCurrentPhase();
    }

    /// <summary>
    /// 現在のGamePhaseに対応するシナリオを開始
    /// </summary>
    public void StartScenarioForCurrentPhase()
    {
        var phase = GameStateManager.Instance.CurrentPhase;
        string fileName = PhaseToFileName(phase);

        if (!string.IsNullOrEmpty(fileName))
        {
            ReloadScenario(fileName);
        }
        else
        {
            Debug.LogWarning("対応するシナリオファイルが存在しません: " + phase);
        }
    }

    /// <summary>
    /// 任意のファイル名でシナリオを再生（再ロード含む）
    /// </summary>
    /// <param name="fileName">JSONファイル名</param>
    public void ReloadScenario(string fileName)
    {
        if (playStory != null)
        {
            GameStateManager.Instance.TriggerScenario();
            playStory.PlayNewStory(fileName);
        }
        else
        {
            Debug.LogError("PlayStory_JSON が設定されていません");
        }
    }

    /// <summary>
    /// ノベルパート終了時に呼び出される（アクション再開など）
    /// </summary>
    public void OnScenarioEnd()
    {
        GameStateManager.Instance.TriggerGameStart();
    }

    /// <summary>
    /// GamePhase → 対応するJSONファイル名
    /// </summary>
    private string PhaseToFileName(GamePhase phase)
    {
        switch (phase)
        {
            case GamePhase.Tutorial1: return "Tutorial1";
            case GamePhase.Tutorial2: return "Tutorial2";
            case GamePhase.Stage1: return "Scenario_Stage1";
            case GamePhase.Stage2: return "Scenario_Stage2";
            default: return null;
        }
    }
}
