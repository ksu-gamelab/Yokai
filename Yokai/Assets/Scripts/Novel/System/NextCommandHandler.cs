using UnityEngine;

public class NextCommandHandler : MonoBehaviour
{
    public AudioClip AudioClip;
    public void Handle(Command cmd)
    {
        Debug.Log("[NextCommandHandler] nextコマンドを処理中");

        switch (cmd.mode)
        {
            case "scene":
                HandleSceneMode();
                break;

            case "game":
                HandleGameMode();
                break;

            default:
                Debug.LogWarning($"[NextCommandHandler] 未定義の next モード: {cmd.mode}");
                break;
        }
    }

    private void HandleSceneMode()
    {
        // 次のフェーズに進み、シナリオモードへ切り替え
        GameStateManager.Instance.AdvancePhase();

        // GamePhase から次のシナリオファイル名を取得
        string nextScenario = GetScenarioFileName(GameStateManager.Instance.CurrentPhase);
        Debug.Log($"[NextCommandHandler] 次のシナリオを再生: {nextScenario}");

        // ScenarioController 経由で再生（PlayStory を内部で呼び出してくれる）
        ScenarioController controller = FindObjectOfType<ScenarioController>();
        if (controller != null)
        {
            controller.ReloadScenario(nextScenario);
        }
        else
        {
            Debug.LogError("[NextCommandHandler] ScenarioController が見つかりませんでした");
        }
    }

    private void HandleGameMode()
    {
        // ゲームモード開始に状態を変更
        GameStateManager.Instance.TriggerGameStart();

        // ノベルUIを非表示にする（例: "NovelSystem" オブジェクト）
        GameObject storyUI = GameObject.Find("NovelSystem");
        if (storyUI != null)
        {
            storyUI.SetActive(false);
            Debug.Log("[NextCommandHandler] NovelSystem を非表示にしました");
        }

        // BGM再生処理（ミキサー音量を通常に戻す）
        if (AudioClip != null)
        {
            if (AudioManager.instance != null && AudioManager.instance.bgmMixer != null)
            {
                AudioManager.instance.bgmMixer.SetFloat("BGMFadeVolume", 0f); // 0dBに戻す
            }

            AudioManager.instance.PlayBGM(AudioClip);
        }

        Debug.Log("[NextCommandHandler] ゲームモード開始");
    }


    private string GetScenarioFileName(GamePhase phase)
    {
        // 各 GamePhase に対応する JSON ファイル名（拡張子なし）を返す
        switch (phase)
        {
            case GamePhase.Tutorial1:
                return "Tutorial1";
            case GamePhase.Tutorial2:
                return "Tutorial2";
            case GamePhase.Stage1:
                return "Stage1";
            case GamePhase.Stage2:
                return "Stage2";
            default:
                Debug.LogWarning("[NextCommandHandler] 未知のフェーズ: " + phase);
                return "UnknownScenario";
        }
    }
}
