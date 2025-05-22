using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    [SerializeField] GameObject ConfigPanel;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Setting"))
        {
            ConfigPanel.SetActive(true);
            GameStateManager.Instance.TriggerGamePause();
        }
    }
}
