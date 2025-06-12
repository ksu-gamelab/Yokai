using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                // éüÇ…çƒê∂Ç∑ÇÈCSVÇÉZÉbÉg
                CSVReader.SetCSV("Tutorial1End");
                controller.StartNovel(TutorialStage.Stage2);
            }
        }
    }
}
