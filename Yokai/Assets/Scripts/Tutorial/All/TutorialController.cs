using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [Header("ストーリー再生コンポーネント")]
    public PlayStory_JSON storyPlayer;

    [Header("ゲーム状態マネージャー（任意）")]
    public GameStateManager gameStateManager;

    // 再生済みシナリオの記録
    private HashSet<string> playedStories = new HashSet<string>();

    // Start時にチュートリアルを確認して再生する
    void Start()
    {
    }

    // 初回のみチュートリアル再生
    public void PlayTutorialIfFirstTime()
    {
        if (!PlayerPrefs.HasKey("tutorial_played"))
        {
            PlayStory("Tutorial1");
            PlayerPrefs.SetInt("tutorial_played", 1);
        }
    }

    // 任意のストーリー再生（何度も再生されない）
    public void PlayStory(string storyFileName)
    {
        Debug.Log("よばれた");
        if (playedStories.Contains(storyFileName))
        {
            Debug.Log($"ストーリー '{storyFileName}' はすでに再生済みです。");
            return;
        }

        if (gameStateManager != null)
        {
            gameStateManager.SetState(GameState.InScenario);  // 状態を「ストーリー中」に変更
        }

        storyPlayer.PlayNewStory(storyFileName);  // 実際の再生処理
        playedStories.Add(storyFileName);         // 再生記録
    }

    // ストーリー再生が完了したときに呼ばれる（PlayStory_JSON から呼び出す）
    public void OnStoryFinished()
    {
        if (gameStateManager != null)
        {
            gameStateManager.SetState(GameState.Playing);  // 通常状態に戻す
        }
    }

    // 強制的に再生させたいとき用（再生済みチェックを無視）
    public void ForcePlayStory(string storyFileName)
    {
        if (gameStateManager != null)
        {
            gameStateManager.SetState(GameState.InScenario);
        }

        storyPlayer.PlayNewStory(storyFileName);
    }
}
