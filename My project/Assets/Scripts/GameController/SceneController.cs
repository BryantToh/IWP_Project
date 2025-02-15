using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{ 
    int buildindex = 0;
    public void ChangeToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ChangeToMain()
    {
        AudioManager.instance.PlaySFX("click");
        SceneManager.LoadScene(1);
    }
    public void ChangeToTutorial()
    {
        AudioManager.instance.PlaySFX("click");
        SceneManager.LoadScene(2);
    }
    public void CloseApp()
    {
        AudioManager.instance.PlaySFX("click");
        Application.Quit();
    }
    private void Start()
    {
        buildindex = SceneManager.GetActiveScene().buildIndex;

        if (AudioManager.instance.GetAudioSource("BGM") != null)
        {
            switch (buildindex)
            {
                case 0:
                    AudioManager.instance.PlayBGM("menu");
                    break;
                case 1:
                    AudioManager.instance.PlayBGM("main");
                    break;
                case 2:
                    AudioManager.instance.PlayBGM("tutorial");
                    break;
            }
        }
        
    }
    //private void Update()
    //{
    //    buildindex = SceneManager.GetActiveScene().buildIndex;

    //    switch (buildindex)
    //    {
    //        case 0:
    //            AudioManager.instance.PlayBGM("menu");
    //            AudioManager.instance.audioBGMPrefab.GetComponent<AudioSource>().loop = true;
    //            break;
    //        case 1:
    //            AudioManager.instance.PlayBGM("menu");
    //            AudioManager.instance.audioBGMPrefab.GetComponent<AudioSource>().loop = true;
    //            break;
    //        case 2:
    //            AudioManager.instance.PlayBGM("tutorial");
    //            AudioManager.instance.audioBGMPrefab.GetComponent<AudioSource>().loop = true;
    //            break;
    //    }
    //}
}
