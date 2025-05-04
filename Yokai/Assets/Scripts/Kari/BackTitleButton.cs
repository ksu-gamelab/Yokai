using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackTitleButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onClicked_backbutton()
    {
        Invoke("loadtitle", 0.3f);
    }
    public void loadtitle()
    {
        SceneManager.LoadScene("Start");
    }
}
