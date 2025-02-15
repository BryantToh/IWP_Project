using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject pauseObj, abilityObj, settingsObj, gameOverObj, uiCanvas, normCanvas;
    public DetailsHandler details;
    public PauseManager pauseManager;
    public timer timeKeep;
    public int paused = 0;

    private void Start()
    {
        uiCanvas.SetActive(false);
        gameOverObj.SetActive(false);
    }

    private void Update()
    {
        HideNShowUI();
    }

    public void ShowAbility()
    {
        ResetUI();
        abilityObj.SetActive(true);
        AudioManager.instance.PlaySFX("click");
        details.index = 0;
    }

    public void ShowPause()
    {
        ResetUI();
        pauseObj.SetActive(true);
    }

    public void ShowSettings()
    {
        ResetUI();
        settingsObj.SetActive(true);
        AudioManager.instance.PlaySFX("click");
    }

    void ResetUI()
    {
        pauseObj.SetActive(false);
        abilityObj.SetActive(false);
        settingsObj.SetActive(false);
    }

    public void BackToMenu()
    {
        ResetUI();
        SceneManager.LoadScene(0);
    }


    public void ShowGameOver(bool gameover)
    {
        if (gameover)
        {
            ResetUI();
            uiCanvas.SetActive(true);
            gameOverObj.SetActive(gameover);
        }
    }

    void HideNShowUI()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !uiCanvas.activeInHierarchy && !timeKeep.gameOver)
        {
            uiCanvas.SetActive(true);
            pauseObj.SetActive(true);
            abilityObj.SetActive(false);
            settingsObj.SetActive(false);
            normCanvas.SetActive(false);
            AudioManager.instance.PlaySFX("openpause");
            paused++;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && uiCanvas.activeInHierarchy && !timeKeep.gameOver)
        {
            if (settingsObj.activeInHierarchy)
            {
                settingsObj.SetActive(false);
                pauseObj.SetActive(true);
                AudioManager.instance.PlaySFX("openpause");
            }
            else if (abilityObj.activeInHierarchy)
            {
                abilityObj.SetActive(false);
                pauseObj.SetActive(true);
                AudioManager.instance.PlaySFX("openpause");
            }
            else
            {
                uiCanvas.SetActive(false);
                normCanvas.SetActive(true);
                AudioManager.instance.PlaySFX("openpause");
                paused--;
            }
        }
        pauseManager.HandleCursor(paused);
    }
}
