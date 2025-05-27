using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearChecker : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Clear"))
        {
            GameStateManager.Instance.TriggerGameClear();
        }
        else if (other.CompareTag("Clear_Tutorial1"))
        {
            var controller = FindObjectOfType<TutorialController>();
            if (controller != null)
            {
                CSVReader.SetCSV("Tutorial1End");

                controller.StartNovel();
            }
        }

    }

}
