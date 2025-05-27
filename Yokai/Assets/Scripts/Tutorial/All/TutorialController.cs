using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject novelUI;

    void Start()
    {
    }

    public void StartNovel()
    {
        if (novelUI != null) novelUI.SetActive(true);
        if (playerObject != null) playerObject.SetActive(false);

        var story = FindObjectOfType<PlayStory>();
        if (story != null)
        {
            story.ReloadStory();
        }
    }

    public void StartTutorial()
    {
        Debug.Log("�`���[�g���A���J�n");
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
