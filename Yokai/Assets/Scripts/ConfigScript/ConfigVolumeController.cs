using UnityEngine;
using UnityEngine.UI;

public class ConfigVolumeController : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;

    private void Start()
    {
        if (AudioManager.instance != null)
        {
            bgmSlider.value = AudioManager.instance.audioSourceBGM.volume;
            seSlider.value = AudioManager.instance.audioSourceSE.volume;
        }

        // スライダー変更時に音量反映
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        seSlider.onValueChanged.AddListener(SetSEVolume);
    }

    private void SetBGMVolume(float value)
    {
        AudioManager.instance?.SetBGMVolume(value);
    }

    private void SetSEVolume(float value)
    {
        AudioManager.instance?.SetSEVolume(value);
    }
}
