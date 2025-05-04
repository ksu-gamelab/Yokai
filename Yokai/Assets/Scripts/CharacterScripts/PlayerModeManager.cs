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
            if (CanTransformBack(state))
                SwitchToNormal();
        }
    }

    void TrySwitchToSpecial()
    {

        if (currentMode != PlayerModeType.Normal) return;

        string state = normalController.GetCurrentStateClipName();
        Debug.Log(CanTransform(state));
        if (!CanTransform(state)) return;

        if (!status.CanTransform()) return;

        status.ConsumeHP(1);
        SwitchToSpecial();
        Debug.Log("スペシャルモードへ!");
    }

    void SwitchToSpecial()
    {
        currentMode = PlayerModeType.Special;
        status.SetHeroTime(specialModeDuration);
        status.SetMode(PlayerModeType.Special);
        UpdateModel(specialModelPrefab);
    }

    void SwitchToNormal()
    {
        currentMode = PlayerModeType.Normal;
        status.SetMode(PlayerModeType.Normal);
        UpdateModel(normalModelPrefab);
        Debug.Log("ノーマルモードへ戻る");
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
        Animator modelAnimator = currentModelInstance.GetComponentInChildren<Animator>();

        if (currentMode == PlayerModeType.Normal)
        {
            normalController.SetAnimator(modelAnimator);
            normalController.SetGrounded(true);
        }
        else
        {
            specialController.SetAnimator(modelAnimator);
            specialController.SetGrounded(true);
        }
    }

    //将来的には地面に接地しているときに条件を変更したい
    bool CanTransform(string stateName)
    {
        //Debug.Log(stateName);
        return stateName == "" || stateName == "N_run" || stateName == "N_New State" || stateName == "N_mabataki";
    }

    bool CanTransformBack(string stateName)
    {
        return stateName == "" || stateName == "H_run" || stateName == "H_New State" || stateName == "H_mabataki";
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
