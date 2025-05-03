using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class NewConfigScript : MonoBehaviour
{
    [SerializeField] private GameObject configPanel;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;
    [SerializeField] private Button closeButton;

    [SerializeField] private GameObject bgmSliderHandle;
    [SerializeField] private GameObject seSliderHandle;


    private int colIndex;
    private int rowIndex;
    private Vector3 normal = new Vector3(1, 1, 1);
    private Vector3 small = new Vector3(.8f, .8f, .8f);

    private bool isWaitingMoveScene;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(bgmSlider);
        Debug.Assert(seSlider);
        Debug.Assert(configPanel);
        Debug.Assert(closeButton);
        Debug.Assert(bgmSliderHandle);
        Debug.Assert(seSliderHandle);
        Debug.Assert(AudioManager.instance != null);
        bgmSlider.onValueChanged.AddListener(SetAudioMixerBGMVolume);
        seSlider.onValueChanged.AddListener(SetAudioMixerSEVolume);
        bgmSlider.value = AudioManager.instance.audioSourceBGM.volume;
        seSlider.value = AudioManager.instance.audioSourceSE.volume;
        seSliderHandle.transform.localScale = small;
        configPanel.SetActive(false);
        isWaitingMoveScene = false;
        colIndex = 0;
        rowIndex = 1;
    }

    private void Update()
    {
        if (isWaitingMoveScene) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (configPanel.activeSelf)
            {
                CloseConfigPanel();
            }
            else
            {
                OpenCofigPanel();
            }
        }
        if (!configPanel.activeSelf) return;
        if (Input.GetKeyDown(KeyCode.W))
        {
            colIndex = Mathf.Max(colIndex - 1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            colIndex = Mathf.Min(colIndex + 1, 2);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (colIndex == 2)
            {
                rowIndex = Mathf.Max(rowIndex - 1, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (colIndex == 2)
            {
                rowIndex = Mathf.Min(rowIndex + 1, 1);
            }
        }

        if (colIndex == 0)
        {
            bgmSlider.Select();
            bgmSliderHandle.transform.localScale = small;
            seSliderHandle.transform.localScale = normal;
            bgmSliderHandle.transform.localScale = normal;
            seSliderHandle.transform.localScale = small;
        }
        if (colIndex == 1)
        {
            seSlider.Select();
            bgmSliderHandle.transform.localScale = normal;
            seSliderHandle.transform.localScale = small;
            bgmSliderHandle.transform.localScale = small;
            seSliderHandle.transform.localScale = normal;
        }
        if (colIndex == 2)
        {
            if (rowIndex == 0)
            {
                seSliderHandle.transform.localScale = small;
            }
            else if (rowIndex == 1)
            {
                seSliderHandle.transform.localScale = small;
                closeButton.Select();
            }
        }

    }

    public void OpenCofigPanel()
    {
        Time.timeScale = 0;
        configPanel.SetActive(true);
        bgmSlider.Select();
        colIndex = 0;
    }

    public void CloseConfigPanel()
    {
        if (isWaitingMoveScene) return;
        //Time.timeScale = 1;
        StartCoroutine(InvokeDelayClose());
        EventSystem.current.SetSelectedGameObject(null);
        configPanel.SetActive(false);
    }

    IEnumerator InvokeDelayClose()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1;
    }

    public void MoveToSelectScene()
    {
        if (isWaitingMoveScene) return;
        isWaitingMoveScene = true;
        Time.timeScale = 1;
        Invoke(nameof(MoveScene), 2);
    }

    private void MoveScene()
    {
        SceneManager.LoadScene("Select_Scene");
    }

    public void SetAudioMixerBGMVolume(float value)
    {
        Debug.Assert(0 <= value && value <= 1);
        AudioManager.instance.audioSourceBGM.volume = value;
    }

    public void SetAudioMixerSEVolume(float value)
    {
        Debug.Assert(0 <= value && value <= 1);
        AudioManager.instance.audioSourceSE.volume = value;
    }

    public void EnterBGMSlider()
    {
        colIndex = 0;
    }

    public void EnterSESlider()
    {
        colIndex = 1;
    }

    public void EnterCloseButton()
    {
        colIndex = 2;
        rowIndex = 1;
    }

    public void EnterSelectButton()
    {
        colIndex = 2;
        rowIndex = 0;
    }
}
