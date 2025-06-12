using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject novelUI;

    public AudioClip playBGM;

    void Start()
    {
        // 起動時の状態に応じて表示を切り替え
        if (GameStateManager.Instance.IsTutorialNovel())
        {
            ActivateNovelMode();
        }
        else if (GameStateManager.Instance.IsTutorialPlay())
        {
            ActivatePlayMode();
        }
    }

    public void StartNovel(TutorialStage stage)
    {
        Debug.Log($"チュートリアルノベル開始: {stage}");
        GameStateManager.Instance.SetTutorialStage(stage);
        GameStateManager.Instance.SetTutorialMode(TutorialMode.Novel);
        ActivateNovelMode();

        var story = FindObjectOfType<PlayStory>();
        if (story != null)
        {
            story.ReloadStory(); // CSVが事前にセットされている前提
        }
    }

    public void StartTutorial()
    {
        GameStateManager.Instance.SetTutorialStage(GameStateManager.Instance.CurrentTutorialStage);
        GameStateManager.Instance.SetTutorialMode(TutorialMode.Play);
        ActivatePlayMode();
    }

    private void ActivateNovelMode()
    {
        if (novelUI != null) novelUI.SetActive(true);
        if (playerObject != null) playerObject.SetActive(false);
    }

    private void ActivatePlayMode()
    {
        AudioManager.instance.PlayBGM(playBGM);
        if (novelUI != null) novelUI.SetActive(false);
        if (playerObject != null) playerObject.SetActive(true);
    }

    public void RestartTutorial()
    {
        Debug.Log("チュートリアルを再読み込みします");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnPlayerFailed()
    {
        RestartTutorial();
    }
}
