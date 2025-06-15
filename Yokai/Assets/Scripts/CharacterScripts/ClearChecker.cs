using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearChecker : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Clear"))
        {

        }
        else if (other.CompareTag("Clear_Tutorial"))
        {
            var controller = FindObjectOfType<TutorialController>();
            if (controller != null)
            {
                if(GameStateManager.Instance.CurrentTutorialStage == TutorialStage.Stage1)
                {
                    // éüÇ…çƒê∂Ç∑ÇÈCSVÇÉZÉbÉg
                    CSVReader.SetCSV("Tutorial1End");
                    controller.StartNovel(TutorialStage.Stage2);
                }

            }
        }
    }
}
