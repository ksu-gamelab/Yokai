using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public Transform leftAnchor;
    public Transform centerAnchor;
    public Transform rightAnchor;

    private Animator animator;
    private float fadeDuration = 0.05f;

    private Dictionary<string, GameObject> activeCharacters = new Dictionary<string, GameObject>();
    private Dictionary<string, string> positionToCharacter = new Dictionary<string, string>();
    private Dictionary<string, bool> isCharacterSpeaking = new Dictionary<string, bool>(); // 追加：キャラごとの発話状態
    private Dictionary<string, Vector3> originalPositions = new Dictionary<string, Vector3>(); // 追加：元の座標

    public void ShowCharacter(Command cmd)
    {
        string name = cmd.character;
        string position = cmd.position;
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(position)) return;
        bool needReplace = cmd.replace;

        if (needReplace)
        {
            // 同じポジションのキャラを削除
            if (positionToCharacter.TryGetValue(position, out string oldCharacterName))
            {
                if (activeCharacters.TryGetValue(oldCharacterName, out GameObject oldChar))
                {
                    Destroy(oldChar);
                    activeCharacters.Remove(oldCharacterName);
                }
                positionToCharacter.Remove(position);
            }

            // 同名キャラが他にいれば削除
            if (activeCharacters.TryGetValue(name, out GameObject sameNameChar))
            {
                Destroy(sameNameChar);
                activeCharacters.Remove(name);

                foreach (var kvp in positionToCharacter)
                {
                    if (kvp.Value == name)
                    {
                        positionToCharacter.Remove(kvp.Key);
                        break;
                    }
                }
            }

            GameObject prefab = Resources.Load<GameObject>($"Prefab/Story/{name}");
            if (prefab == null)
            {
                Debug.LogError($"キャラPrefabの読み込み失敗: {name}");
                return;
            }

            GameObject character = Instantiate(prefab, GetAnchor(position), false);
            activeCharacters[name] = character;
            positionToCharacter[position] = name;
        }
        else if (!activeCharacters.ContainsKey(name))
        {
            Debug.LogWarning($"キャラ {name} が存在しません。replace=falseでは生成されません。");
            return;
        }

        animator = activeCharacters[name].GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError($"キャラ {name} にAnimatorが見つかりません");
            return;
        }

        showAnimation(cmd);
        speakingAnimation(cmd);
    }

    private void showAnimation(Command cmd)
    {
        animator.CrossFade("nagisa_initial", fadeDuration, 0);
        animator.CrossFade(cmd.motion, fadeDuration, 0);
    }

    private Transform GetAnchor(string pos)
    {
        return pos switch
        {
            "left" => leftAnchor,
            "right" => rightAnchor,
            "center" => centerAnchor,
            _ => centerAnchor
        };
    }

    private void speakingAnimation(Command cmd)
    {
        string name = cmd.character;
        if (!activeCharacters.TryGetValue(name, out GameObject character)) return;

        // 初回だけ元の位置を保存
        if (!originalPositions.ContainsKey(name))
            originalPositions[name] = character.transform.localPosition;

        bool wasSpeaking = isCharacterSpeaking.ContainsKey(name) && isCharacterSpeaking[name];

        if (cmd.is_speaking)
        {
            if (!wasSpeaking)
            {
                // 上へ移動
                Vector3 start = character.transform.localPosition;
                Vector3 target = start + new Vector3(0, 30f, 0);
                StartCoroutine(MoveCharacterToPosition(character, start, target, 0.1f));
                isCharacterSpeaking[name] = true;
            }

            // モーション再生（念のため）
            Animator anim = character.GetComponent<Animator>();
            if (anim != null)
                anim.CrossFade(cmd.motion, fadeDuration, 0);
        }
        else
        {
            if (wasSpeaking)
            {
                Vector3 start = character.transform.localPosition;
                Vector3 target = originalPositions[name];
                StartCoroutine(MoveCharacterToPosition(character, start, target, 0.1f));
                isCharacterSpeaking[name] = false;
            }
        }
    }

    private IEnumerator MoveCharacterToPosition(GameObject character, Vector3 startPosition, Vector3 targetPosition, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            character.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        character.transform.localPosition = targetPosition;
    }
}
