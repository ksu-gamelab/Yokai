using UnityEngine;
using System.Collections.Generic;

public class PlayerEffectManager : MonoBehaviour
{
    [Header("エフェクト一覧（プレハブ名で管理）")]
    [SerializeField] private GameObject[] effectPrefabs;

    private Dictionary<string, GameObject> effectDict;

    private void Awake()
    {
        effectDict = new Dictionary<string, GameObject>();

        foreach (var prefab in effectPrefabs)
        {
            if (prefab != null && !effectDict.ContainsKey(prefab.name))
            {
                effectDict[prefab.name] = prefab;
            }
        }
    }

    /// <summary>
    /// 名前指定でエフェクトを再生（ローカル座標で再生）
    /// </summary>
    public void PlayEffect(string effectName)
    {
        if (!effectDict.TryGetValue(effectName, out var prefab))
        {
            Debug.LogWarning($"Effect '{effectName}' が見つかりませんでした。");
            return;
        }

        GameObject instance = Instantiate(prefab, this.transform); // 自身の子に生成（ローカル位置保持）
                                                                   // 親のscaleがマイナス（左右反転）されていたら、子で反転を打ち消す
        Vector3 parentScale = transform.lossyScale;
        Vector3 adjustedScale = prefab.transform.localScale;

        // X軸の反転補正
        if (parentScale.x < 0)
            adjustedScale.x *= -1;

        instance.transform.localScale = adjustedScale;
        Destroy(instance, 2f); // 寿命は必要に応じて調整
    }
}
