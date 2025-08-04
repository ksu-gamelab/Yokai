using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSourceBGM;
    public AudioSource audioSourceSE;

    public AudioMixer bgmMixer;
    public AudioMixer seMixer;

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
        audioSourceBGM.outputAudioMixerGroup = bgmMixer.FindMatchingGroups("Master")[0];
        audioSourceSE.outputAudioMixerGroup = seMixer.FindMatchingGroups("Master")[0];
        audioSourceBGM.volume = .3f;
        audioSourceBGM.loop = true;
        audioSourceSE.volume = .5f;
    }

    /// <summary>
    /// BGMを再生します.
    /// </summary>
    /// <remarks>引数にAudioClipを渡してください.現在のBGMの再生を停止してから新たに再生します</remarks>
    /// <param name="audioClip">音楽ファイル</param>
    public void PlayBGM(AudioClip audioClip)
    {
        //Debug.Assert(audioClip);
        audioSourceBGM.Stop();
        audioSourceBGM.clip = audioClip;
        audioSourceBGM.Play();
    }

    /// <summary>
    /// SEを再生します.
    /// </summary>
    /// <remarks>引数にAudioClipを渡してください.</remarks>
    /// <param name="audioClip">音楽ファイル</param>
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
