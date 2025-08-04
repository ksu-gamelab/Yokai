using UnityEngine;
using System;

public static class StoryLoader
{
    public static StoryData LoadStory(string filename)
    {
        // Resources フォルダに置いたファイル名から拡張子なしで読み込む
        TextAsset jsonText = Resources.Load<TextAsset>(filename);

        if (jsonText == null)
        {
            Debug.LogError($"JSONファイルが見つかりません: {filename}");
            return null;
        }

        try
        {
            StoryData data = JsonUtility.FromJson<StoryData>(jsonText.text);
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"JSONの読み込みに失敗しました: {e.Message}");
            return null;
        }
    }
}
