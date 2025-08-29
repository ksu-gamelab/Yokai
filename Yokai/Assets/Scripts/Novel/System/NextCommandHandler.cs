using UnityEngine;
using UnityEngine.SceneManagement;

public class NextCommandHandler : MonoBehaviour
{
    public AudioClip AudioClip;
    public GameObject fadeobj;

    private string nextSceneToLoad;

    public void Handle(Command cmd)
    {
        Debug.Log("[NextCommandHandler] nextコマンドを処理中");

        switch (cmd.mode)
        {
            case "scene":
                if (!string.IsNullOrEmpty(cmd.next_target))
                {
                    HandleSceneMode(cmd.next_target);
                }
                else
                {
                    Debug.LogError("[NextCommandHandler] next_target が指定されていません");
                }
                break;

            case "game":
                HandleGameMode();
                break;

            default:
                Debug.LogWarning($"[NextCommandHandler] 未定義の next モード: {cmd.mode}");
                break;
        }
    }

    private void HandleSceneMode(string nextSceneName)
    {
        Debug.Log($"[NextCommandHandler] シーン移動の準備中: {nextSceneName}");

        // 次のフェーズに進める
        GameStateManager.Instance.AdvancePhase();

        // フェードアウト開始
        if (fadeobj != null)
        {
            fadeobj.SetActive(true);
        }

        // 1秒後にシーン遷移
        nextSceneToLoad = nextSceneName;
        Invoke("LoadNextScene", 1.0f);
    }

    private void LoadNextScene()
    {
        Debug.Log($"[NextCommandHandler] Invokeでシーン遷移: {nextSceneToLoad}");
        SceneManager.LoadScene(nextSceneToLoad);
    }

    private void HandleGameMode()
    {
        GameStateManager.Instance.TriggerGameStart();

        GameObject storyUI = GameObject.Find("NovelSystem");
        if (storyUI != null)
        {
            storyUI.SetActive(false);
            Debug.Log("[NextCommandHandler] NovelSystem を非表示にしました");
        }

        if (AudioClip != null)
        {
            if (AudioManager.instance != null && AudioManager.instance.bgmMixer != null)
            {
                AudioManager.instance.bgmMixer.SetFloat("BGMFadeVolume", 0f);
            }

            AudioManager.instance.PlayBGM(AudioClip);
        }

        Debug.Log("[NextCommandHandler] ゲームモード開始");
    }
}
