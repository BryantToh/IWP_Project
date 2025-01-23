using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject pauseObj, abilityObj, /*settingsObj,*/ gameOverObj, uiCanvas;
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

    public void HidePause()
    {
        ResetUI();
        abilityObj.SetActive(true);
        details.index = 0;
    }
    public void HideAbility()
    {
        ResetUI();
        pauseObj.SetActive(true);
    }
    void ResetUI()
    {
        pauseObj.SetActive(false);
        abilityObj.SetActive(false);
        //settingsObj.SetActive(false);
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
            paused++;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && uiCanvas.activeInHierarchy && !timeKeep.gameOver)
        {
            uiCanvas.SetActive(false);
            paused--;
        }
        pauseManager.HandleCursor(paused);
    }
}
