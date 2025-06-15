// PlayStory.cs（StoryLine対応・元機能維持バージョン）
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayStory : MonoBehaviour
{
    public Text dialogueText;
    public Text nameText;
    public GameObject characterImage;
    public Image backgroundImage;
    public GameObject generateBase;

    [SerializeField] private GameObject[] characterPrefabs;
    private Dictionary<string, GameObject> characterDictionary;
    private Dictionary<string, GameObject> activeCharacters = new();

    [SerializeField] private Sprite[] Sprites;
    private Dictionary<string, Sprite> spriteDictionary;

    [SerializeField] private GameObject[] generateObjects;
    private Dictionary<string, GameObject> generateObjectDictionary;
    private GameObject generatedObj;

    [SerializeField] AudioClip[] audioclips;
    [SerializeField] AudioClip fadeSE;
    private Dictionary<string, AudioClip> audioclipDictionary;

    public GameObject selectBack;
    public Text select1Text;
    public Text select2Text;

    private List<StoryLine> storyLines;
    private int currentLine = 1;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private string currentDialogue = "";
    private string nextscene = "";

    public GameObject fadein;
    public GameObject fadeout;
    private int currentIndex = 0; // 現時点では未使用
    public GameObject configPanel;

    private GenericUISelector genericUISelector;

    void Start()
    {
        genericUISelector = FindObjectOfType<GenericUISelector>();

        characterDictionary = new();
        foreach (var prefab in characterPrefabs)
            characterDictionary[prefab.name] = prefab;

        spriteDictionary = new();
        foreach (var sprite in Sprites)
            spriteDictionary[sprite.name] = sprite;

        audioclipDictionary = new();
        foreach (var audio in audioclips)
            audioclipDictionary[audio.name] = audio;

        generateObjectDictionary = new();
        foreach (var obj in generateObjects)
            generateObjectDictionary[obj.name] = obj;

        selectBack.SetActive(false);

        if (GameStateManager.Instance.CurrentTutorialMode != TutorialMode.Novel) return;
        StartCoroutine(LoadStoryAndStart());
    }

    private IEnumerator LoadStoryAndStart()
    {
        var reader = GetComponent<CSVReader>();
        yield return new WaitUntil(() => reader.GetStoryData() != null);
        storyLines = reader.GetStoryData();

        if (storyLines.Count > 1)
            ShowNextDialogue();
        else
            Debug.LogError("CSVデータが正しく読み込まれていません");
    }

    public void ShowNextDialogue()
    {
        if (GameStateManager.Instance.CurrentTutorialMode == TutorialMode.Play) return;
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentDialogue;
            isTyping = false;
            return;
        }

        if (currentLine >= storyLines.Count)
        {
            Debug.Log("ストーリーが終了しました");
            return;
        }

        var line = storyLines[currentLine];
        currentDialogue = line.MainText;

        UpdateCharacter(line.CharaImage);
        UpdateGenerateObj(line.Generate);
        UpdateBackground(line.BackImage);
        LoadScene(line.NextScene);
        ChangeBGM(line.BGM);
        ChangeSE(line.SE);
        AnimatorSet(line.Animation, line.CharaImage);
        ShowName(line.NameText);

        if (!string.IsNullOrEmpty(line.SelectText1) && !string.IsNullOrEmpty(line.SelectText2))
        {
            ShowChoices(line.SelectText1, line.SelectText2);
            currentLine++;
            return;
        }
        else
        {
            genericUISelector.RestoreSelection();
        }

        typingCoroutine = StartCoroutine(TypeText(currentDialogue));
        currentLine++;
    }

    private void ShowName(string name)
    {
        Debug.Log(name);
        nameText.text = string.IsNullOrEmpty(name) ? "" : name;
    }

    private void ShowChoices(string choice1, string choice2)
    {
        select1Text.text = choice1;
        select2Text.text = choice2;
        selectBack.SetActive(true);
        genericUISelector.SwitchGroup("select");
    }

    public void onClicked_selectbutton()
    {
        selectBack.SetActive(false);
        ShowNextDialogue();
    }

    public void onClicked_screenbutton()
    {
        ShowNextDialogue();
    }

    private IEnumerator TypeText(string dialogue)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in dialogue)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        isTyping = false;
    }

    private void UpdateCharacter(string charaNames)
    {
        if (currentLine > 0 && charaNames == storyLines[currentLine - 1].CharaImage)
            return;

        foreach (Transform child in characterImage.transform)
            Destroy(child.gameObject);

        activeCharacters.Clear();

        if (!string.IsNullOrEmpty(charaNames))
        {
            foreach (var name in charaNames.Split('/'))
            {
                if (characterDictionary.TryGetValue(name, out var prefab))
                {
                    var character = Instantiate(prefab, characterImage.transform);
                    character.name = name;
                    activeCharacters[name] = character;
                }
                else
                {
                    Debug.LogWarning($"キャラプレハブ '{name}' が登録されていません");
                }
            }
        }
    }

    private void UpdateGenerateObj(string objName)
    {
        if (!string.IsNullOrEmpty(objName) && generateObjectDictionary.TryGetValue(objName, out var obj))
        {
            generatedObj = Instantiate(obj, generateBase.transform);
        }
        else if (objName == "clear" && generatedObj != null)
        {
            Destroy(generatedObj);
        }
    }

    private void UpdateBackground(string backgroundName)
    {
        if (!string.IsNullOrEmpty(backgroundName) && spriteDictionary.TryGetValue(backgroundName, out var sprite))
        {
            backgroundImage.sprite = sprite;
        }
        else if (!string.IsNullOrEmpty(backgroundName))
        {
            Debug.LogWarning($"背景画像 '{backgroundName}' が登録されていません");
        }
    }

    private void ChangeBGM(string bgm)
    {
        Debug.Log($"BGMを変更: {bgm}");
        if (bgm == "stop")
            AudioManager.instance.audioSourceBGM.Stop();
        else if (!string.IsNullOrEmpty(bgm))
            AudioManager.instance.PlayBGM(audioclipDictionary[bgm]);
    }

    private void ChangeSE(string se)
    {
        if (!string.IsNullOrEmpty(se))
            AudioManager.instance.PlaySE(audioclipDictionary[se]);
    }

    private void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) return;

        if (sceneName == "Start")
        {
            var controller = FindObjectOfType<TutorialController>();
            if (controller != null)
                controller.StartTutorial();
            else
                Debug.LogWarning("TutorialController がシーン上に存在しません");
        }
        else
        {
            try
            {
                var stage = (NormalStage)Enum.Parse(typeof(NormalStage), sceneName);
                GameStateManager.Instance.SetNormalStage(stage);
            }
            catch (ArgumentException)
            {
                Debug.LogError($"NormalStage に '{sceneName}' は存在しません");
            }

            nextscene = sceneName;
            CSVReader.SetCSV(nextscene);
            fadein.SetActive(true);
            AudioManager.instance.PlaySE(fadeSE);
            AudioManager.instance.audioSourceBGM.Stop();
            Invoke("WaitFade", 2.0f);
        }
    }

    private void WaitFade()
    {
        SceneManager.LoadScene(nextscene);
    }

    private void AnimatorSet(string animData, string charaNames)
    {
        if (string.IsNullOrEmpty(animData) || string.IsNullOrEmpty(charaNames)) return;

        string[] animTriggers = animData.Split('/');
        string[] charaArray = charaNames.Split('/');

        for (int i = 0; i < charaArray.Length; i++)
        {
            if (!activeCharacters.TryGetValue(charaArray[i], out var charaObj)) continue;
            Animator animator = charaObj.GetComponent<Animator>();
            if (animator == null) continue;
            string trigger = (i < animTriggers.Length) ? animTriggers[i] : "";
            if (!string.IsNullOrEmpty(trigger))
                animator.SetTrigger(trigger);
        }
    }

    public void ReloadStory()
    {
        StopAllCoroutines();

        var reader = GetComponent<CSVReader>();
        if (reader == null) return;

        storyLines?.Clear();

        reader.ReloadCSV();
        storyLines = reader.GetStoryData();
        currentLine = 1;
        StartCoroutine(LoadStoryAndStart());
    }
} // end
