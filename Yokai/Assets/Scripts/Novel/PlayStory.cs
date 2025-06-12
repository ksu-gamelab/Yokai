using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayStory : MonoBehaviour
{
    // 0:num 1:maintext 2:charaimage 3:backimage 4:bgm 5:se 6:selecttext1 7:selecttext2 8:nextscene 9:generateobj 12:name
    public Text dialogueText;
    public Text nameText;
    public GameObject characterImage; // キャラプレハブの親（空オブジェクト）
    public Image backgroundImage;
    public GameObject generateBase;

    [SerializeField] private GameObject[] characterPrefabs;
    private Dictionary<string, GameObject> characterDictionary;
    Dictionary<string, GameObject> activeCharacters = new Dictionary<string, GameObject>();

    [SerializeField] private Sprite[] Sprites;
    private Dictionary<string, Sprite> spriteDictionary;

    [SerializeField] private GameObject[] generateObjects;
    private Dictionary<string, GameObject> generateObjectDictionary;
    GameObject generatedObj;

    [SerializeField] AudioClip[] audioclips;
    [SerializeField] AudioClip fadeSE;
    private Dictionary<string, AudioClip> audioclipDictionary;

    public GameObject selectBack;
    public Text select1Text;
    public Text select2Text;

    private List<string[]> storyData;
    private int currentLine = 1;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private string currentDialogue = "";

    private string nextscene = "";

    public GameObject fadein;
    public GameObject fadeout;
    private int currentIndex = 0;
    public GameObject configPanel;

    GenericUISelector genericUISelector;

    void Start()
    {
        genericUISelector = FindObjectOfType<GenericUISelector>();
        // キャラプレハブ登録
        characterDictionary = new Dictionary<string, GameObject>();
        foreach (GameObject prefab in characterPrefabs)
        {
            characterDictionary[prefab.name] = prefab;
        }

        spriteDictionary = new Dictionary<string, Sprite>();
        foreach (Sprite sprite in Sprites)
        {
            spriteDictionary[sprite.name] = sprite;
        }

        audioclipDictionary = new Dictionary<string, AudioClip>();
        foreach (AudioClip audioclip in audioclips)
        {
            audioclipDictionary[audioclip.name] = audioclip;
        }

        generateObjectDictionary = new Dictionary<string, GameObject>();
        foreach (GameObject gameObject in generateObjects)
        {
            generateObjectDictionary[gameObject.name] = gameObject;
        }

        selectBack.SetActive(false);
        if (GameStateManager.Instance.CurrentTutorialMode != TutorialMode.Novel) return;
        StartCoroutine(LoadStoryAndStart());
    }

    private IEnumerator LoadStoryAndStart()
    {
        yield return new WaitUntil(() => this.gameObject.GetComponent<CSVReader>().GetStoryData() != null);
        storyData = this.gameObject.GetComponent<CSVReader>().GetStoryData();

        if (storyData.Count > 1)
        {
            ShowNextDialogue();
        }
        else
        {
            Debug.LogError("CSVデータが正しく読み込まれていません");
        }
    }

    private void Update() { }

    public void ShowNextDialogue()
    {
        if (GameStateManager.Instance.CurrentTutorialMode == TutorialMode.Play) return;
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentDialogue;
            isTyping = false;
        }
        else if (currentLine < storyData.Count)
        {
            currentDialogue = storyData[currentLine][1];

            updateCharacter();
            updateGenerateObj();
            updateBackgroud();
            loadScene();
            changeBGM();
            changeSE();
            AnimatorSet();
            ShowName();

            string selectText1 = storyData[currentLine][6];
            string selectText2 = storyData[currentLine][7];

            if (!string.IsNullOrEmpty(selectText1) && !string.IsNullOrEmpty(selectText2))
            {
                ShowChoices(selectText1, selectText2);
                currentLine++;
                return;
            } else
            {
                //選択肢が無いとき
                genericUISelector.RestoreSelection();
            }

            typingCoroutine = StartCoroutine(TypeText(currentDialogue));
            currentLine++;
        }
        else
        {
            Debug.Log("ストーリーが終了しました");
        }
    }

    private void ShowName()
    {
        Debug.Log(storyData[currentLine][11]);
        if (!string.IsNullOrEmpty(storyData[currentLine][11]))
        {
            nameText.text = storyData[currentLine][11];
        }
        else
        {
            nameText.text = "";
        }
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

    private IEnumerator TypeText(string dialogue)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        isTyping = false;
    }

    public void onClicked_screenbutton()
    {
        ShowNextDialogue();
    }

    public void updateCharacter()
    {
        string charaNames = storyData[currentLine][2];

        // 一つ前の行と同じ場合は更新しない
        if (currentLine > 0 && charaNames.Equals(storyData[currentLine - 1][2]))
        {
            return;
        }

        // 前のキャラを全削除
        foreach (Transform child in characterImage.transform)
        {
            Destroy(child.gameObject);
        }

        // activeCharacters辞書も初期化
        activeCharacters.Clear();

        // 現在表示すべきキャラ名が空でなければ処理
        if (!string.IsNullOrEmpty(charaNames))
        {
            string[] nameArray = charaNames.Split('/');

            foreach (string name in nameArray)
            {
                if (characterDictionary.ContainsKey(name))
                {
                    GameObject character = Instantiate(characterDictionary[name], characterImage.transform);
                    character.name = name;

                    // 登録（アニメーション発火で使用するため）
                    activeCharacters[name] = character;
                }
                else
                {
                    Debug.LogWarning($"キャラプレハブ '{name}' が登録されていません");
                }
            }
        }
    }




    public void updateGenerateObj()
    {
        string generateObjName = storyData[currentLine][9];

        if (!string.IsNullOrEmpty(generateObjName) && generateObjectDictionary.ContainsKey(generateObjName))
        {
            generatedObj = Instantiate(generateObjectDictionary[generateObjName], generateBase.transform);
        }
        else
        {
            if (generateObjName == "clear" && generatedObj != null)
            {
                Destroy(generatedObj);
            }
            Debug.LogWarning($"生成オブジェクト '{generateObjName}' が登録されていません");
        }
    }

    public void updateBackgroud()
    {
        string backImageFile = storyData[currentLine][3];

        if (!string.IsNullOrEmpty(backImageFile) && spriteDictionary.ContainsKey(backImageFile))
        {
            backgroundImage.sprite = spriteDictionary[backImageFile];
        }
        else
        {
            Debug.LogWarning($"背景画像 '{backImageFile}' が登録されていません");
        }
    }

    public void changeBGM()
    {
        if (storyData[currentLine][4] == "stop")
        {
            AudioManager.instance.audioSourceBGM.Stop();
        }
        else if (storyData[currentLine][4] != "")
        {
            AudioManager.instance.PlayBGM(audioclipDictionary[storyData[currentLine][4]]);
        }
    }

    public void changeSE()
    {
        if (storyData[currentLine][5] != "")
        {
            AudioManager.instance.PlaySE(audioclipDictionary[storyData[currentLine][5]]);
        }
    }

    public void loadScene()
    {
        string sceneName = storyData[currentLine][8];

        if (string.IsNullOrEmpty(sceneName)) return;

        // チュートリアル再生用キーワード
        if (sceneName == "Start")
        {

            // チュートリアル起動処理
            TutorialController controller = FindObjectOfType<TutorialController>();
            if (controller != null)
            {
                controller.StartTutorial();
            }
            else
            {
                Debug.LogWarning("TutorialController がシーン上に存在しません");
            }
        }
        else
        {
            try
            {
                NormalStage stage = (NormalStage)Enum.Parse(typeof(NormalStage), sceneName);
                GameStateManager.Instance.SetNormalStage(stage);
            }
            catch (ArgumentException)
            {
                Debug.LogError($"NormalStage に '{sceneName}' という名前のステージは存在しません。");
            }

            // 通常のシーン遷移
            nextscene = sceneName;
            CSVReader.SetCSV(nextscene);
            fadein.SetActive(true);
            AudioManager.instance.PlaySE(fadeSE);
            AudioManager.instance.audioSourceBGM.Stop();
            Invoke("waitFade", 2.0f);
        }

    }


    public void waitFade()
    {
        SceneManager.LoadScene(nextscene);
    }

    void AnimatorSet()
    {
        string animData = storyData[currentLine][9];      // animation列
        string charaNames = storyData[currentLine][2];    // charaimage列

        if (string.IsNullOrEmpty(animData) || string.IsNullOrEmpty(charaNames))
            return;

        string[] animTriggers = animData.Split('/');
        string[] charaArray = charaNames.Split('/');

        for (int i = 0; i < charaArray.Length; i++)
        {
            string charaName = charaArray[i];

            if (activeCharacters.ContainsKey(charaName))
            {
                GameObject charaObj = activeCharacters[charaName];
                Animator animator = charaObj.GetComponent<Animator>();

                if (animator != null)
                {
                    // アニメーションが足りない場合は空扱い
                    string trigger = (i < animTriggers.Length) ? animTriggers[i] : "";

                    if (!string.IsNullOrEmpty(trigger))
                    {
                        animator.SetTrigger(trigger);
                    }
                }
            }
            else
            {
                Debug.LogWarning($"キャラ '{charaName}' に対するアニメーション対象が見つかりません");
            }
        }
    }

    public void ReloadStory()
    {
        StopAllCoroutines(); // 文字送りなどを止める
        if(storyData != null)
        {
             storyData.Clear();
        }

        var reader = GetComponent<CSVReader>();
        if (reader == null) return;

        reader.ReloadCSV();
        storyData = reader.GetStoryData();

        currentLine = 1;
        StartCoroutine(LoadStoryAndStart());
    }


}
