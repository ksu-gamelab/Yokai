using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GenericUISelector : MonoBehaviour
{
    [System.Serializable]
    public class ButtonGroup
    {
        public string groupName;
        public Selectable[] uiElements;
    }

    [SerializeField] private List<ButtonGroup> groups;
    [SerializeField] private string defaultGroup = "main";
    [SerializeField] private float inputDelay = 0.2f;

    [Header("Highlight Color")]
    [SerializeField] private Color selectedColor = Color.yellow;
    [SerializeField] private Color defaultColor = Color.white;

    private Selectable[] currentElements;
    private int index = 0;
    private float inputTimer = 0f;

    private void Start()
    {
        SwitchGroup(defaultGroup);
    }

    private void Update()
    {
        if (GameStateManager.Instance.IsPaused()) return;

        inputTimer -= Time.unscaledDeltaTime;

        if (currentElements == null || currentElements.Length == 0) return;

        if (inputTimer <= 0f)
        {
            float v = Input.GetAxisRaw("Vertical");
            float h = Input.GetAxisRaw("Horizontal");

            if (v < -0.5f)
            {
                index = (index + 1) % currentElements.Length;
                Highlight(index);
                inputTimer = inputDelay;
            }
            else if (v > 0.5f)
            {
                index = (index - 1 + currentElements.Length) % currentElements.Length;
                Highlight(index);
                inputTimer = inputDelay;
            }

            if (currentElements[index] is Slider slider)
            {
                float step = 0.05f;
                if (h > 0.5f)
                {
                    slider.value = Mathf.Clamp(slider.value + step, slider.minValue, slider.maxValue);
                    inputTimer = inputDelay;
                }
                else if (h < -0.5f)
                {
                    slider.value = Mathf.Clamp(slider.value - step, slider.minValue, slider.maxValue);
                    inputTimer = inputDelay;
                }
            }

            if (Input.GetButtonDown("Submit"))
            {
                var sel = currentElements[index];
                if (sel is Button btn) btn.onClick.Invoke();
                else if (sel is Toggle toggle) toggle.isOn = !toggle.isOn;
            }
        }
    }

    private void Highlight(int i)
    {
        for (int j = 0; j < currentElements.Length; j++)
        {
            var colors = currentElements[j].colors;
            colors.normalColor = (j == i) ? selectedColor : defaultColor;
            currentElements[j].colors = colors;
        }

        currentElements[i].Select();
    }

    public void SwitchGroup(string groupName)
    {
        var found = groups.Find(g => g.groupName == groupName);
        if (found == null || found.uiElements.Length == 0)
        {
            Debug.LogWarning($"Group '{groupName}' not found or empty.");
            return;
        }

        currentElements = found.uiElements;
        index = 0;
        Highlight(index);
    }

    public void RestoreSelection()
    {
        if (currentElements != null && currentElements.Length > 0)
        {
            index = 0;
            Highlight(index);
        }
    }

}
