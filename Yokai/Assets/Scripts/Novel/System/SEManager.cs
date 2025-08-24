using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SEManager : MonoBehaviour
{
    private AudioSource audioSourceSE;

    void Start()
    {
        if (AudioManager.instance != null)
        {
            audioSourceSE = AudioManager.instance.audioSourceSE;
        }
        else
        {
            Debug.LogError("[SEManager] AudioManager.instance が見つかりません");
        }
    }

    public void Handle(Command cmd)
    {
        StartCoroutine(PlaySE(cmd.se, cmd.se_volume, cmd.delay));
    }

    private IEnumerator PlaySE(string seName, float volume, float delay)
    {
        AudioClip clip = LoadClip(seName);
        if (clip == null)
        {
            Debug.LogWarning($"[SEManager] SE '{seName}' が見つかりません（Resources/Audio/SE/{seName}）");
            yield break;
        }

        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        // 一時的に音量を変更して再生（再生後に戻す）
        float originalVolume = audioSourceSE.volume;
        audioSourceSE.volume = Mathf.Clamp01(volume);
        audioSourceSE.PlayOneShot(clip);
        yield return null; // 次のフレームまで待つ（安全対策）
        audioSourceSE.volume = originalVolume;
    }

    private AudioClip LoadClip(string name)
    {
        return Resources.Load<AudioClip>($"Audio/SE/{name}");
    }
}
