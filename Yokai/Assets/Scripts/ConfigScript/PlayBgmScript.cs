using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBgmScript : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayBGM(audioClip);
    }
}
