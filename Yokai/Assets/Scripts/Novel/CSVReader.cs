using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    private static string nextCSVName; // 他シーンから渡す一時保持用
    private List<StoryLine> storyLines = new List<StoryLine>();

    void Start()
    {
        if (string.IsNullOrEmpty(nextCSVName))
        {
            Debug.LogError("CSVファイル名が指定されていません");
            return;
        }

        LoadCSV(nextCSVName);
    }

    /// <summary>
    /// 次に読み込むCSVファイル名を設定（.csv拡張子は不要）
    /// </summary>
    public static void SetCSV(string fileName)
    {
        nextCSVName = fileName;
    }

    /// <summary>
    /// CSVファイルをResources/CSV内から読み込み、StoryLineリストに変換
    /// </summary>
    public void LoadCSV(string fileName)
    {
        storyLines.Clear();

        TextAsset csvFile = Resources.Load<TextAsset>("CSV/" + fileName);
        if (csvFile == null)
        {
            Debug.LogError("CSVファイルが見つかりません: " + fileName);
            return;
        }

        StringReader reader = new StringReader(csvFile.text);


        int lineNum = 0;
        while (reader.Peek() > -1)
        {
            lineNum++;
            string line = reader.ReadLine();
            string[] values = line.Split(',');

            var storyLine = new StoryLine
            {
                Num = lineNum,
                MainText = GetValue(values, 1),
                CharaImage = GetValue(values, 2),
                BackImage = GetValue(values, 3),
                BGM = GetValue(values, 4),
                SE = GetValue(values, 5),
                SelectText1 = GetValue(values, 6),
                SelectText2 = GetValue(values, 7),
                NextScene = GetValue(values, 8),
                Animation = GetValue(values, 9),
                Generate = GetValue(values, 10),
                NameText = GetValue(values, 11)
            };

            storyLines.Add(storyLine);
        }
    }

    /// <summary>
    /// 指定インデックスの値を取得（範囲外なら空文字）
    /// </summary>
    private string GetValue(string[] values, int index)
    {
        return values.Length > index ? values[index] : "";
    }

    /// <summary>
    /// 読み込んだストーリーデータを返す
    /// </summary>
    public List<StoryLine> GetStoryData()
    {
        return storyLines;
    }

    /// <summary>
    /// 外部からCSV再読込を要求する
    /// </summary>
    public void ReloadCSV()
    {
        if (string.IsNullOrEmpty(nextCSVName))
        {
            Debug.LogError("再読込失敗：CSVファイル名が指定されていません");
            return;
        }

        LoadCSV(nextCSVName);
    }
}
