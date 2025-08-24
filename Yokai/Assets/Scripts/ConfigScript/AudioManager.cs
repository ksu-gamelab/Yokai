using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSourceBGM;
    public AudioSource audioSourceSE;

    public AudioMixer bgmMixer;
    public AudioMixer seMixer;

    [Header("グループ名はAudioMixerに登録されたExpose名と一致させてください")]
    public string bgmFadeGroupName = "BGMFadeVolume";
    public string seGroupName = "SEVolume";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        audioSourceBGM = gameObject.AddComponent<AudioSource>();
        audioSourceSE = gameObject.AddComponent<AudioSource>();

        // ✅ BGM用のMixerGroupを設定（存在確認付き）
        AudioMixerGroup[] bgmGroups = bgmMixer.FindMatchingGroups(bgmFadeGroupName);
        if (bgmGroups.Length > 0)
        {
            audioSourceBGM.outputAudioMixerGroup = bgmGroups[0];
        }
        else
        {
            Debug.LogError($"[AudioManager] BGM Mixer Group '{bgmFadeGroupName}' が見つかりません");
        }

        // ✅ SE用のMixerGroupを設定（存在確認付き）
        AudioMixerGroup[] seGroups = seMixer.FindMatchingGroups(seGroupName);
        if (seGroups.Length > 0)
        {
            audioSourceSE.outputAudioMixerGroup = seGroups[0];
        }
        else
        {
            Debug.LogError($"[AudioManager] SE Mixer Group '{seGroupName}' が見つかりません");
        }

        audioSourceBGM.volume = 0.3f; // 恒常音量（設定スライダーで調整）
        audioSourceBGM.loop = true;
        audioSourceSE.volume = 0.5f;
    }

    public void PlayBGM(AudioClip audioClip)
    {
        audioSourceBGM.Stop();
        audioSourceBGM.clip = audioClip;
        audioSourceBGM.Play();
    }

    public void PlaySE(AudioClip audioClip)
    {
        Debug.Assert(audioClip);
        audioSourceSE.PlayOneShot(audioClip);
    }

    public void SetBGMVolume(float volume)
    {
        audioSourceBGM.volume = volume;
    }

    public void SetSEVolume(float volume)
    {
        audioSourceSE.volume = volume;
    }
}
