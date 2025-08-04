using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

    public GameObject textBox;

    private string currentDialogue = "";
    private bool isTyping = false;
    private Coroutine typingCoroutine;


    public void ShowText(Command cmd)
    {
        nameText.text = cmd.character ?? "";
        currentDialogue = cmd.text;

        ShowTextBox(cmd.text_ui);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(currentDialogue, cmd.text_speed));
    }

    private IEnumerator TypeText(string dialogue, float speed)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in dialogue)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(speed);
        }

        isTyping = false;
    }

    public bool IsTyping()
    {
        return isTyping;
    }

    public void SkipText()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentDialogue;
            isTyping = false;
        }
    }

    public void ShowTextBox(string path)
    {
        textBox.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/" + path);
    }
}
