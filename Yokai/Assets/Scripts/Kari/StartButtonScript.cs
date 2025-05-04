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
        SceneManager.LoadScene("StoryScene");
    }

    public void onClicked_opencredit()
    {
        creditPanel.SetActive(true);
    }

    public void onClicked_closecredit()
    {
        creditPanel.SetActive(false);
    }
}
