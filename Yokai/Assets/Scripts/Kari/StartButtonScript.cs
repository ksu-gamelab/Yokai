using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonScript : MonoBehaviour
{
    public GameObject startButton;
    public GameObject fadein;

    public AudioClip buttonclip;
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
        AudioManager.instance.PlaySE(buttonclip);
        fadein.SetActive(true);
        Invoke("loadstart", 1.5f);
    }
    public void loadstart()
    {
        SceneManager.LoadScene("StoryScene");
    }
}
