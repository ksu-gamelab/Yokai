using UnityEngine;

public class PlayerViewManager : MonoBehaviour
{
    [Header("表示モデル")]
    public GameObject normalModelPrefab;
    public GameObject specialModelPrefab;

    private GameObject currentModelInstance;
    private Animator currentAnimator;

    private PlayerStatus status;

    void Start()
    {
        status = GetComponent<PlayerStatus>();
        status.OnModeChanged += OnModeChanged;

        // 初期モデルを生成
        OnModeChanged(status.CurrentMode);
    }

    private void OnDestroy()
    {
        if (status != null)
            status.OnModeChanged -= OnModeChanged;
    }

    private void OnModeChanged(PlayerModeType mode)
    {
        if (currentModelInstance != null)
        {
            Destroy(currentModelInstance);
        }

        GameObject prefabToUse = (mode == PlayerModeType.Special) ? specialModelPrefab : normalModelPrefab;
        currentModelInstance = Instantiate(prefabToUse, transform);

        currentAnimator = currentModelInstance.GetComponent<Animator>();
    }

    public void PlayTrigger(string trigger)
    {
        if (currentAnimator != null)
        {
            currentAnimator.SetTrigger(trigger);
        }
    }

    public void SetAnimatorFloat(string param, float value)
    {
        if (currentAnimator != null)
        {
            currentAnimator.SetFloat(param, value);
        }
    }

    public void SetAnimatorBool(string param, bool value)
    {
        if (currentAnimator != null)
        {
            currentAnimator.SetBool(param, value);
        }
    }

    public string GetCurrentClipName()
    {
        if (currentAnimator == null) return "";

        var clips = currentAnimator.GetCurrentAnimatorClipInfo(0);
        return clips.Length > 0 ? clips[0].clip.name : "";
    }

    public void PlayAnimation(string clipName)
    {
        if (currentAnimator != null)
        {
            currentAnimator.Play(clipName);
        }
    }

}
