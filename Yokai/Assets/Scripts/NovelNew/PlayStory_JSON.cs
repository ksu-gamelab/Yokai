using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayStory_JSON : MonoBehaviour
{
    string storyFileName = "Tutorial1";
    private StoryData storyData;
    private int currentIndex = 0;

    private TextManager textManager;
    private BackgroundManager backgroundManager;
    private CharacterManager characterManager;

    public Button screenButton;
    private bool screenButtonClicked = false;

    void Start()
    {
        textManager = GetComponent<TextManager>();
        backgroundManager = GetComponent<BackgroundManager>();
        characterManager = GetComponent<CharacterManager>();

        storyData = StoryLoader.LoadStory("JSON/" + storyFileName);

        if (screenButton != null)
        {
            screenButton.onClick.AddListener(() => screenButtonClicked = true);
        }

        if (storyData != null && storyData.items.Count > 0)
        {
            StartCoroutine(PlayStory());
        }
        else
        {
            Debug.LogError("ストーリーデータの読み込みに失敗したか、データが空です。");
        }
    }

    IEnumerator PlayStory()
    {
        while (currentIndex < storyData.items.Count)
        {
            CommandSet set = storyData.items[currentIndex];
            yield return StartCoroutine(ExecuteCommands(set.commands));
            currentIndex++;
        }

        Debug.Log("ストーリーの再生が完了しました。");
    }

    IEnumerator ExecuteCommands(List<Command> commands)
    {
        List<Command> textCommands = new List<Command>();

        foreach (var cmd in commands)
        {
            if (cmd.type == "show_text")
            {
                textCommands.Add(cmd);
                continue;
            }

            yield return StartCoroutine(ExecuteSingleCommand(cmd));
        }

        foreach (var textCmd in textCommands)
        {
            yield return StartCoroutine(HandleShowText(textCmd));
        }
    }

    IEnumerator ExecuteSingleCommand(Command cmd)
    {
        switch (cmd.type)
        {
            case "set_background":
                backgroundManager.SetBackground(cmd.background);
                yield break;

            case "show_character":
                characterManager.ShowCharacter(cmd);
                yield break;

            default:
                Debug.LogWarning($"未対応のコマンド: {cmd.type}");
                yield break;
        }
    }

    IEnumerator HandleShowText(Command cmd)
    {
        textManager.ShowText(cmd);

        if (!cmd.wait_for_click)
        {
            yield return new WaitUntil(() => !textManager.IsTyping());
            yield return new WaitForSeconds(0.5f);
            screenButtonClicked = false;
        }
        else
        {
            bool waiting = true;
            while (waiting)
            {
                if (screenButtonClicked)
                {
                    screenButtonClicked = false;
                    if (textManager.IsTyping())
                    {
                        textManager.SkipText();
                    }
                    else
                    {
                        waiting = false;
                    }
                }

                yield return null;
            }
        }
    }
}
