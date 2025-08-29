using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class PlayStory_JSON : MonoBehaviour
{
    public string storyFileName;
    private StoryData storyData;
    private int currentIndex = 0;

    private TextManager textManager;
    private BackgroundManager backgroundManager;
    private CharacterManager characterManager;
    private BGMManager bgmManager;
    private SEManager seManager;
    private NextCommandHandler nextHandler;

    public Button screenButton;
    private bool screenButtonClicked = false;

    private Coroutine storyCoroutine;  // 現在のストーリー再生を記録

    [SerializeField] private bool debugMode = false;

    [SerializeField] GameObject StorySystem;


    void Start()
    {
        textManager = GetComponent<TextManager>();
        backgroundManager = GetComponent<BackgroundManager>();
        characterManager = GetComponent<CharacterManager>();
        bgmManager = GetComponent<BGMManager>();
        seManager = GetComponent<SEManager>();
        nextHandler = GetComponent<NextCommandHandler>();
        if (!GameStateManager.Instance.IsScenario())
        {
            GameStateManager.Instance.TriggerGameStart();
            // ストーリー再生しない（たとえば敵にぶつかって再読み込みされた時など）
            StorySystem.SetActive(false);
        } else
        {
            GameStateManager.Instance.TriggerScenario();
        }



        if (screenButton != null)
        {
            screenButton.onClick.AddListener(() => screenButtonClicked = true);
        }

        if (debugMode)
        {
            LoadAndStartStory();  // デバッグ時のみ起動直後に再生
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
            case "play_bgm":
                bgmManager.Handle(cmd);
                yield break;
            case "play_se":
                seManager.Handle(cmd);
                yield break;
            case "next":
                nextHandler.Handle(cmd);
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
    public void LoadAndStartStory()
    {
        // 現在の再生を止める
        if (storyCoroutine != null)
        {
            StopCoroutine(storyCoroutine);
            storyCoroutine = null;
        }

        // インデックスとステータスのリセット
        currentIndex = 0;
        screenButtonClicked = false;

        // 新しいストーリーデータを読み込み
        storyData = StoryLoader.LoadStory("JSON/" + storyFileName);

        if (storyData != null && storyData.items.Count > 0)
        {
            storyCoroutine = StartCoroutine(PlayStory());
        }
        else
        {
            Debug.LogError("新しいストーリーデータの読み込みに失敗、またはデータが空です。");
        }
    }

    public void PlayNewStory(string fileName)
    {
        if (debugMode)
        {
            // デバッグモード時は外部からの上書きを無視して初期値を使う
            Debug.Log("[DebugMode] 外部からのストーリー変更を無視し、" + storyFileName + " を再生します");
            LoadAndStartStory(); // 初期設定のファイル名で再生
        }
        else
        {
            storyFileName = fileName;
            LoadAndStartStory();
        }
    }


}
