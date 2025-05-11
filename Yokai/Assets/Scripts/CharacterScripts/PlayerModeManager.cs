using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerModeType { Normal, Special }

public class PlayerModeManager : MonoBehaviour
{
    private PlayerControllerNormal normalController;
    private PlayerControllerSpecial specialController;
    private PlayerStatus status;

    public float specialModeDuration = 3f;

    private PlayerModeType currentMode = PlayerModeType.Normal;

    public GameObject normalModelPrefab;
    public GameObject specialModelPrefab;
    private GameObject currentModelInstance;

    public AudioClip specialtonormalSE;
    public AudioClip normaltospecialSE;

    void Start()
    {
        normalController = GetComponent<PlayerControllerNormal>();
        specialController = GetComponent<PlayerControllerSpecial>();
        status = GetComponent<PlayerStatus>();

        UpdateModel(normalModelPrefab);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            TrySwitchToSpecial();

        if (currentMode == PlayerModeType.Special)
            UpdateSpecialMode();

        if (currentMode == PlayerModeType.Normal)
            normalController.HandleInput();
        else
            specialController.HandleInput();
    }

    void UpdateSpecialMode()
    {
        status.UpdateHeroTime(Time.deltaTime);

        if (status.HeroTimeRemaining <= 0f)
        {
            string state = specialController.GetCurrentStateClipName();
        }
    }

    void TrySwitchToSpecial()
    {

        if (currentMode != PlayerModeType.Normal) return;

        string state = normalController.GetCurrentStateClipName();

        status.ConsumeHP(1);
        SwitchToSpecial();
        Debug.Log("スペシャルモードへ!");
    }

    void SwitchToSpecial()
    {
        if (normaltospecialSE != null)
            AudioManager.instance.PlaySE(normaltospecialSE);

        currentMode = PlayerModeType.Special;
        status.SetHeroTime(specialModeDuration);
        status.SetMode(PlayerModeType.Special);
        UpdateModel(specialModelPrefab);
    }

    void SwitchToNormal()
    {
        if (specialtonormalSE != null)
            AudioManager.instance.PlaySE(specialtonormalSE);
        currentMode = PlayerModeType.Normal;
        status.SetMode(PlayerModeType.Normal);
        UpdateModel(normalModelPrefab);
    }

    public void NotifyGrounded(bool grounded)
    {
        if (currentMode == PlayerModeType.Normal)
            normalController.SetGrounded(grounded);
        else
            specialController.SetGrounded(grounded);
    }

    void UpdateModel(GameObject newModelPrefab)
    {
        if (currentModelInstance != null)
            Destroy(currentModelInstance);

        currentModelInstance = Instantiate(newModelPrefab, transform);
        bool pastGrounded = currentMode == PlayerModeType.Normal ? specialController.IsGrounded() : normalController.IsGrounded();
        NotifyGrounded(pastGrounded);

    }


    public void ForceTransformToSpecial()
    {
        if (currentMode == PlayerModeType.Normal)
        {
            if (status.HP <= 0)
            {
                GoToGameOver();
            }
            else
            {
                status.ConsumeHP(1);
                SwitchToSpecial();
            }
        }
    }

    private void GoToGameOver()
    {
        Debug.Log("ゲームオーバー！");
        GameStateManager.Instance.TriggerGameOver();
    }

}
