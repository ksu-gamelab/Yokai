using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    public TextAsset[] csvFiles;
    private TextAsset csvFile; // 現在のストーリー
    private List<string[]> storyData = new List<string[]>(); // ストーリーデータを格納

    void Start()
    {
        setCSV();
        LoadCSV();
    }


    void LoadCSV()
    {

        StringReader reader = new StringReader(csvFile.text);
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            string[] values = line.Split(',');
            storyData.Add(values);
        }
    }

    public List<string[]> GetStoryData()
    {
        return storyData;
    }

    public void setCSV()
    {
        csvFile = csvFiles[GManager.instance.storyNum];
    }
}
