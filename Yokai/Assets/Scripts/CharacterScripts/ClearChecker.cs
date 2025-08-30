using UnityEngine;

public class ClearChecker : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Clear"))
        {
            // 例：通常ステージ1クリア → ステージ2開始シナリオ再生へ
            if (GameStateManager.Instance.CurrentPhase == GamePhase.Stage1)
            {
                GameStateManager.Instance.SetPhase(GamePhase.Stage2); // 次のフェーズに更新
                var controller = FindObjectOfType<ScenarioController>();
                if (controller != null)
                {
                    controller.StartScenarioForCurrentPhase(); // ステージ2のシナリオを再生
                }
            }
        }
        else if (other.CompareTag("Clear_Tutorial"))
        {
            if (GameStateManager.Instance.CurrentPhase == GamePhase.Tutorial1)
            {
                GameStateManager.Instance.IsScenario();
                ScenarioController.Instance.ReloadScenario("Tutorial1_2");
            } else if(GameStateManager.Instance.CurrentPhase==GamePhase.Tutorial2)
            {
                ScenarioController.Instance.ReloadScenario("Tutorial2_1");
                GameStateManager.Instance.IsScenario();
                
            }
        }
    }
}
