using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class BGMManager : MonoBehaviour
{
    private AudioMixer bgmMixer;
    private const string FadeParam = "BGMFadeVolume";

    void Start()
    {
        if (AudioManager.instance != null)
        {
            bgmMixer = AudioManager.instance.bgmMixer;
        }
        else
        {
            Debug.LogError("[BGMManager] AudioManager.instance が見つかりませんでした。");
        }
    }

    public void Handle(Command cmd)
    {
        if (cmd.bgm == "stop")
        {
            StartCoroutine(FadeBGM(0f, cmd.fade_time));
            StartCoroutine(StopAfter(cmd.fade_time));
        }
        else
        {
            AudioClip clip = LoadClip(cmd.bgm);
            if (clip == null)
            {
                Debug.LogWarning($"[BGMManager] BGM '{cmd.bgm}' が見つかりません（Resources/Audio/BGM/{cmd.bgm}）");
                return;
            }

            float volume = Mathf.Clamp01(cmd.bgm_volume <= 0f ? 1f : cmd.bgm_volume);
            float targetDb = Mathf.Log10(volume) * 20f;

            AudioManager.instance.PlayBGM(clip);

            if (cmd.fade == "in")
            {
                // 最初に音量を0（無音）にしてからスタート
                bgmMixer.SetFloat(FadeParam, -80f);
                StartCoroutine(FadeBGM(targetDb, cmd.fade_time));
            }
            else
            {
                bgmMixer.SetFloat(FadeParam, targetDb);
            }
        }
    }

    private IEnumerator FadeBGM(float targetDb, float duration)
    {
        float currentDb;
        bgmMixer.GetFloat(FadeParam, out currentDb);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float newDb = Mathf.Lerp(currentDb, targetDb, t);
            bgmMixer.SetFloat(FadeParam, newDb);
            yield return null;
        }

        bgmMixer.SetFloat(FadeParam, targetDb);
    }

    private IEnumerator StopAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (AudioManager.instance.audioSourceBGM.isPlaying)
        {
            AudioManager.instance.audioSourceBGM.Stop();
        }
    }

    private AudioClip LoadClip(string name)
    {
        return Resources.Load<AudioClip>($"Audio/BGM/{name}");
    }
}
