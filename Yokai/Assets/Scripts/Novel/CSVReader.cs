using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    private static string nextCSVName; // 他シーンから渡す一時保持用
    private List<string[]> storyData = new List<string[]>();

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
    /// CSVファイルをResources/CSV内から読み込み
    /// </summary>
    public void LoadCSV(string fileName)
    {
        storyData.Clear();

        TextAsset csvFile = Resources.Load<TextAsset>("CSV/" + fileName);
        if (csvFile == null)
        {
            Debug.LogError("CSVファイルが見つかりません: " + fileName);
            return;
        }

        StringReader reader = new StringReader(csvFile.text);
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            string[] values = line.Split(',');
            storyData.Add(values);
        }
    }

    /// <summary>
    /// 読み込んだストーリーデータを返す
    /// </summary>
    public List<string[]> GetStoryData()
    {
        return storyData;
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
