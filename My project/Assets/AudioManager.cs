using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public List<AudioEffects> sfxClips = new List<AudioEffects>();
    public List<AudioBGM> bgmClips = new List<AudioBGM>();
    public GameObject audioPrefab;
    public GameObject audioBGMPrefab;
    private AudioSource audioSource, audioSourceBGM;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        GameObject obj = Instantiate(audioPrefab);
        GameObject obj2 = Instantiate(audioBGMPrefab);

        if (obj != null)
            audioSource = obj.GetComponent<AudioSource>();

        if (obj2 != null)
        {
            audioSourceBGM = obj2.GetComponent<AudioSource>();
        }
    }
    public void PlaySFX(string tag)
    {
        for (int i = 0; i < sfxClips.Count; i++)
        {
            if (sfxClips[i].tag == tag)
            {
                audioSource.clip = sfxClips[i].effect;
                audioSource.Play();
            }
        }
    }

    public void PlayBGM(string tag)
    {
        for (int i = 0; i < bgmClips.Count; i++)
        {
            if (bgmClips[i].tag == tag)
            {
                audioSourceBGM.clip = bgmClips[i].bgm;
                audioSourceBGM.loop = true;
                audioSourceBGM.Play();
            }
        }
    }
}

[System.Serializable]
public class AudioEffects
{
    public AudioClip effect;
    public string tag;
}

[System.Serializable]
public class AudioBGM
{
    public AudioClip bgm;
    public string tag;
}
