using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ScenarioController.Instance.StartScenarioForCurrentPhase();
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
