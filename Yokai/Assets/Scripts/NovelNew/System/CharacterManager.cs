using System.Collections.Generic;
using Live2D.Cubism.Framework.Motion;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public Transform leftAnchor;
    public Transform centerAnchor;
    public Transform rightAnchor;

    private Dictionary<string, GameObject> activeCharacters = new Dictionary<string, GameObject>();
    private Dictionary<string, string> positionToCharacter = new Dictionary<string, string>(); // position -> characterName

    Live2DMotionPlayer live2DManager;

    public void ShowCharacter(Command cmd)
    {
        string name = cmd.character;
        string position = cmd.position;
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(position)) return;
        bool needReplace = cmd.replace;


        if (needReplace)
        {
            // 同じポジションにいるキャラを削除
            if (positionToCharacter.TryGetValue(position, out string oldCharacterName))
            {
                if (activeCharacters.TryGetValue(oldCharacterName, out GameObject oldChar))
                {
                    GameObject.Destroy(oldChar);
                    activeCharacters.Remove(oldCharacterName);
                }

                positionToCharacter.Remove(position);
            }

            // もし同名キャラが他のポジションにいれば、それも消す（念のため）
            if (activeCharacters.TryGetValue(name, out GameObject sameNameChar))
            {
                GameObject.Destroy(sameNameChar);
                activeCharacters.Remove(name);

                // 対応するpositionも削除
                string keyToRemove = null;
                foreach (var kvp in positionToCharacter)
                {
                    if (kvp.Value == name)
                    {
                        keyToRemove = kvp.Key;
                        break;
                    }
                }
                if (keyToRemove != null)
                    positionToCharacter.Remove(keyToRemove);
            }
            // 新しいキャラを生成
            GameObject prefab = Resources.Load<GameObject>($"Prefab/Story/{name}");
            if (prefab == null)
            {
                Debug.LogError($"キャラPrefabの読み込み失敗: {name}");
                return;
            }
            else
            {
                //Debug.Log($"キャラPrefabの読み込み成功: {name}");
            }

            GameObject character = Instantiate(prefab, GetAnchor(position), false);
            activeCharacters[name] = character;
            positionToCharacter[position] = name;
        }
        else
        {
            // replaceじゃない → 表情やモーションだけ反映
            if (!activeCharacters.ContainsKey(name))
            {
                Debug.LogWarning($"キャラ {name} が存在しません。replace=falseでは生成されません。");
                return;
            }
        }

        //CubismMotionController motionController = activeCharacters[name].GetComponent<CubismMotionController>();
        //        Debug.Log($"キャラ {motionController}");
        //this.gameObject.GetComponent<Live2DMotionPlayer>().PlayLive2DMotion(cmd.motion, motionController);

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
}
