using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayStory : MonoBehaviour
{
    // 0:num 1:maintext 2:charaimage 3:backimage 4:bgm 5:se 6:selecttext1 7:selecttext2 8:nextscene 9:generateobj
    public Text dialogueText;
    public GameObject characterImage; // キャラプレハブの親（空オブジェクト）
    public Image backgroundImage;
    public GameObject generateBase;

    [SerializeField] private GameObject[] characterPrefabs;
    private Dictionary<string, GameObject> characterDictionary;
    private GameObject currentCharacter;

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

    void Start()
    {
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

            string selectText1 = storyData[currentLine][6];
            string selectText2 = storyData[currentLine][7];

            if (!string.IsNullOrEmpty(selectText1) && !string.IsNullOrEmpty(selectText2))
            {
                ShowChoices(selectText1, selectText2);
                currentLine++;
                return;
            }

            typingCoroutine = StartCoroutine(TypeText(currentDialogue));
            currentLine++;
        }
        else
        {
            Debug.Log("ストーリーが終了しました");
        }
    }

    private void ShowChoices(string choice1, string choice2)
    {
        select1Text.text = choice1;
        select2Text.text = choice2;
        selectBack.SetActive(true);
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
        string charaName = storyData[currentLine][2];

        if (currentCharacter != null)
        {
            Destroy(currentCharacter);
        }

        if (!string.IsNullOrEmpty(charaName) && characterDictionary.ContainsKey(charaName))
        {
            currentCharacter = Instantiate(characterDictionary[charaName], characterImage.transform);
        }
        else
        {
            Debug.LogWarning($"キャラプレハブ '{charaName}' が登録されていません");
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
        if (storyData[currentLine][8] != "")
        {
            nextscene = storyData[currentLine][8];
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
        if (!string.IsNullOrEmpty(storyData[currentLine][9]) && currentCharacter != null)
        {
            Animator animator = currentCharacter.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger(storyData[currentLine][9]);
            }
        }
    }
}
