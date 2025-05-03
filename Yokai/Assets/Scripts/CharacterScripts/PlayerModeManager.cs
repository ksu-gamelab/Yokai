using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerModeType { Normal, Special }

public class PlayerModeManager : MonoBehaviour
{
    private PlayerControllerNormal normalController;
    private PlayerControllerSpecial specialController;
    private PlayerStatus status;

    public float specialModeDuration = 3f;
    private float specialModeTimer = 0f;

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

        // 入力処理
        if (currentMode == PlayerModeType.Normal)
            normalController.HandleInput();
        else
            specialController.HandleInput();
    }

    void UpdateSpecialMode()
    {
        specialModeTimer -= Time.deltaTime;

        if (specialModeTimer <= 0f)
        {
            string state = specialController.GetCurrentStateClipName();
            if (CanTransformBack(state))
                SwitchToNormal();
        }
    }

    void TrySwitchToSpecial()
    {
        if (currentMode != PlayerModeType.Normal)
            return;

        string state = normalController.GetCurrentStateClipName();
        if (!CanTransform(state))
            return;

        if (!status.CanTransform())
            return;

        status.ConsumeHP(1);
        SwitchToSpecial();
        Debug.Log("スペシャルモードへ!");
    }

    void SwitchToSpecial()
    {
        currentMode = PlayerModeType.Special;
        specialModeTimer = specialModeDuration;
        UpdateModel(specialModelPrefab);
    }

    void SwitchToNormal()
    {
        currentMode = PlayerModeType.Normal;
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

    // "run" or "New State" のときだけ変身許可
    bool CanTransform(string stateName)
    {
        return stateName == "" || stateName == "N_run" || stateName == "N_New State";
    }

    bool CanTransformBack(string stateName)
    {
        return stateName == "" || stateName == "H_run" || stateName == "H_New State";
    }

    // 強制的に変身
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
        SceneManager.LoadScene("GameOver");  // ← ゲームオーバーシーン名に合わせて変更してください
    }

}
