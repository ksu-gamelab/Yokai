using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public UnityEngine.UI.Image backgroundImage;

    // Start is called before the first frame update
    public void SetBackground(string name)
    {
        string path = "Sprites/BackGrounds/" + name;
        Sprite newSprite = Resources.Load<Sprite>(path);

        if (newSprite != null)
        {
            backgroundImage.sprite = newSprite;
        }
        else
        {
            Debug.LogError($"背景画像の読み込みに失敗: {path}");
        }
    }
}
