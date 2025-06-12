using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonScript : MonoBehaviour
{
    public GameObject startButton;
    public GameObject fadein;

    public AudioClip buttonclip;
    public GameObject creditPanel;
    [SerializeField] private GenericUISelector selector;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onClicked_startbutton()
    {
        GameStateManager.Instance.TriggerGameStart();
        AudioManager.instance.PlaySE(buttonclip);
        fadein.SetActive(true);
        Invoke("loadstart", 1.5f);
    }
    public void loadstart()
    {
        GameStateManager.Instance.SetState(GameState.Playing);
        GameStateManager.Instance.SetTutorialStage(TutorialStage.Stage1);
        GameStateManager.Instance.SetTutorialMode(TutorialMode.Novel);
        CSVReader.SetCSV("Tutorial1");
        SceneManager.LoadScene("Tutorial1");
    }

    public void onClicked_opencredit()
    {
        selector.SwitchGroup("credit");
        creditPanel.SetActive(true);
    }

    public void onClicked_closecredit()
    {
        selector.SwitchGroup("main");
        creditPanel.SetActive(false);
    }
}
