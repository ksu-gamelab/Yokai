using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject novelUI;

    public AudioClip playBGM;

    void Start()
    {
        // �N�����̏�Ԃɉ����ĕ\����؂�ւ�
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
        Debug.Log($"�`���[�g���A���m�x���J�n: {stage}");
        GameStateManager.Instance.SetTutorialStage(stage);
        GameStateManager.Instance.SetTutorialMode(TutorialMode.Novel);
        ActivateNovelMode();

        var story = FindObjectOfType<PlayStory>();
        if (story != null)
        {
            story.ReloadStory(); // CSV�����O�ɃZ�b�g����Ă���O��
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
        Debug.Log("�`���[�g���A�����ēǂݍ��݂��܂�");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnPlayerFailed()
    {
        RestartTutorial();
    }
}
