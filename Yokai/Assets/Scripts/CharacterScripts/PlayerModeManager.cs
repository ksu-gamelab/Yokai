// PlayerModeManager.cs
using UnityEngine;

public enum PlayerModeType { Normal, Special }

public class PlayerModeManager : MonoBehaviour
{
    private PlayerControllerNormal normalController;
    private PlayerControllerSpecial specialController;
    private PlayerStatus status;

    public float specialModeDuration = 10f;
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

        // 初期モデルを明示的に生成しておく
        UpdateModel(normalModelPrefab);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TrySwitchToSpecial();
        }

        if (currentMode == PlayerModeType.Special)
        {
            specialModeTimer -= Time.deltaTime;
            if (specialModeTimer <= 0f)
            {
                SwitchToNormal();
            }
        }

        if (currentMode == PlayerModeType.Normal)
        {
            normalController.HandleInput();
        }
        else
        {
            specialController.HandleInput();
        }
    }

    void TrySwitchToSpecial()
    {
        if (currentMode == PlayerModeType.Normal && status.CanTransform())
        {
            status.ConsumeHP(1);
            SwitchToSpecial();
        }
    }

    void SwitchToSpecial()
    {
        currentMode = PlayerModeType.Special;
        specialModeTimer = specialModeDuration;
        UpdateModel(specialModelPrefab);
        Debug.Log("スペシャルモードへ!");
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
        {
            normalController.SetGrounded(grounded);
        }
        else
        {
            specialController.SetGrounded(grounded);
        }
    }


    void UpdateModel(GameObject newModelPrefab)
    {
        if (currentModelInstance != null)
        {
            Destroy(currentModelInstance);
        }
        currentModelInstance = Instantiate(newModelPrefab, transform);
    }

}