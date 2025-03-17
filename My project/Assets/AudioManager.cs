using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public List<AudioEffects> sfxClips = new List<AudioEffects>();
    public List<AudioBGM> bgmClips = new List<AudioBGM>();
    public GameObject audioPrefab;
    public GameObject audioBGMPrefab;

    private AudioSource audioSourceBGM, audioSource;

    private Dictionary<string, GameObject> activeSFX = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        GameObject obj2 = Instantiate(audioBGMPrefab);
        if (obj2 != null)
        {
            audioSourceBGM = obj2.GetComponent<AudioSource>();
        }
    }

    public AudioSource GetAudioSource(string audioType)
    {
        switch (audioType)
        {
            case "BGM":
                return audioSourceBGM;
            case "SFX":
                return audioSource;
            default:
                return null;
        }
    }

    public void PlaySFX(string tag)
    {
        if (activeSFX.ContainsKey(tag)) return;

        for (int i = 0; i < sfxClips.Count; i++)
        {
            if (sfxClips[i].tag == tag)
            {
                GameObject sfxObj = new GameObject("SFX_" + tag);
                AudioSource newSource = sfxObj.AddComponent<AudioSource>();
                newSource.clip = sfxClips[i].effect;
                newSource.Play();

                activeSFX[tag] = sfxObj;

                Destroy(sfxObj, newSource.clip.length);
                StartCoroutine(RemoveFromDictionary(tag, newSource.clip.length));
            }
        }
    }

    public void StopSFX(string tag)
    {
        if (activeSFX.TryGetValue(tag, out GameObject sfxObj))
        {
            Destroy(sfxObj);
            activeSFX.Remove(tag);
        }
    }

    private IEnumerator RemoveFromDictionary(string tag, float delay)
    {
        yield return new WaitForSeconds(delay);
        activeSFX.Remove(tag);
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

    public void StopBGM()
    {
        audioSourceBGM.Stop();
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
